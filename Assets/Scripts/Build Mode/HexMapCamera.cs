using UnityEngine;

public class HexMapCamera : MonoBehaviour
{
    [SerializeField] private HexGrid _grid;

    [Header("Camera Movement Speed")]
    [SerializeField] private float _movementMinZoom;
    [SerializeField] private float _movementMaxZoom;
    [SerializeField] private float _rotationSpeed;

    [Header("Total Zoom")]
    [SerializeField] private float stickMinZoom;
    [SerializeField] private float stickMaxZoom;

    [Header("Zoom Angle")]
    [SerializeField] private float _swivelMinZoom;
    [SerializeField] private float _swivelMaxZoom;

    private Transform _swivel;
    private Transform _stick;

    private float _zoom = 1f;
    private float _rotationAngle;

    private Camera _mainCamera;
    private static HexMapCamera HexMapCameraInstance { get; set; }
    
    public static Camera MainCamera => HexMapCameraInstance._mainCamera;
    
    private void Awake()
    {
        _swivel = transform.GetChild(0);
        _stick = _swivel.GetChild(0);

        // TODO: check if removing the null check breaks the logic, HexMapCameraInstance != null
        if (HexMapCameraInstance)
        {
            Destroy(gameObject);
        }
        else
        {
            HexMapCameraInstance = this;
        }

        _mainCamera = GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        float zoomDelta = Input.GetAxis("Mouse ScrollWheel");
        float xDelta = Input.GetAxis("Horizontal");
        float zDelta = Input.GetAxis("Vertical");
        float rotationDelta = Input.GetAxis("Rotation");

        if (zoomDelta != 0f)
        {
            AdjustZoom(zoomDelta);
        }    

        if (xDelta != 0f || zDelta != 0f)
        {
            AdjustPosition(xDelta, zDelta);
        }

        if (rotationDelta != 0f)
        {
            AdjustRotation(rotationDelta);
        }
    }

    private void AdjustZoom(float zoomDelta)
    {
        _zoom = Mathf.Clamp01(_zoom + zoomDelta);

        float distance = Mathf.Lerp(stickMinZoom, stickMaxZoom, _zoom);
        _stick.localPosition = new Vector3(0f, 0f, distance);

        float angle = Mathf.Lerp(_swivelMinZoom, _swivelMaxZoom, _zoom);
        _swivel.localRotation = Quaternion.Euler(angle, 0f, 0f);
    }

    private void AdjustPosition(float xDelta, float zDelta)
    {
        Vector3 direction = transform.localRotation * new Vector3(xDelta, 0f, zDelta).normalized;
        float damping = Mathf.Max(Mathf.Abs(xDelta), Mathf.Abs(zDelta));
        float distance = Mathf.Lerp(_movementMinZoom, _movementMaxZoom, _zoom) * damping * Time.deltaTime;

        Vector3 position = transform.localPosition;
        position += direction * distance;
        transform.localPosition = ClampPosition(position);
    }

    private Vector3 ClampPosition(Vector3 position)
    {
        float xMax = (_grid.ChunkCountX * HexMetrics.ChunkSizeX - 0.5f) * (2f * HexMetrics.InnerRadius);
        position.x = Mathf.Clamp(position.x, 0f, xMax);

        float zMax = (_grid.ChunkCountZ * HexMetrics.ChunkSizeZ - 1) * (1.5f * HexMetrics.OuterRadius);
        position.z = Mathf.Clamp(position.z, 0f, zMax);

        return position;
    }

    private void AdjustRotation(float rotationDelta)
    {
        _rotationAngle += rotationDelta * _rotationSpeed * Time.deltaTime;

        if (_rotationAngle < 0f)
        {
            _rotationAngle += 360f;
        }
        else if (_rotationAngle >= 360f)
        {
            _rotationAngle -= 360f;
        }

        transform.localRotation = Quaternion.Euler(0f, _rotationAngle, 0f);
    }
}