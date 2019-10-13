using System;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float _invalidBound = 0.4f;
    [SerializeField] private float _campSnapDistance = 6f;

    private Vector3 _offset;
    private Vector3 _movementOffset;
    private Vector3 _currentOffset;
    private Vector3 _smoothDampVelocity;
    private Camera _mainCamera;

    private bool _canFollow;
    private float _time;

    public event Action OnCameraMoved;
    public event Action OnCameraOnCamp;

    public static CameraFollow CameraFollowInstance { get; private set; }

    public static Camera MainCamera => CameraFollowInstance._mainCamera;

    private void Awake()
    {
        if (CameraFollowInstance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            CameraFollowInstance = this;
        }

        _mainCamera = GetComponent<Camera>();
    }

    private void Start()
    {
        _offset = transform.position - Vector3.zero;
    }

    private void LateUpdate()
    {
        if (PlayerController.PlayerCurrentState == PlayerController.PlayerState.Interacting)
        {
            return;
        }

        float distanceToCamp = Vector3.Distance(Vector3.zero, PlayerController.PlayerPosition);

        if (distanceToCamp < _campSnapDistance)
        {
           OnCameraOnCamp?.Invoke();

           transform.position = Vector3.zero + _offset; 
        }
        else
        {
            Vector2 targetScreenSpace = _mainCamera.WorldToScreenPoint(PlayerController.PlayerPosition);
            targetScreenSpace = _mainCamera.ScreenToViewportPoint(targetScreenSpace);
            float distanceToCenter = Vector2.Distance(new Vector2(0.5f, 0.5f), targetScreenSpace);

            if (distanceToCenter > _invalidBound)
            {
                _canFollow = true;
            }
            else
            {
                _canFollow = false;
            }

            if (distanceToCenter > 1)
            {
                //reset
                _movementOffset = _offset;
                transform.position = PlayerController.PlayerPosition + _movementOffset;
            }

            if (_canFollow)
            {
                transform.position = PlayerController.PlayerPosition + _offset;

                OnCameraMoved?.Invoke();
            }
        }
    }
}
