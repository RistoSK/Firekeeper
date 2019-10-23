using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class KinematicObject : MonoBehaviour
{
    [SerializeField] private GameObject CUBE;

    private NavMeshAgent _playerAgent;
    private Vector3 _forward;

    public UnityAction onPlayerDied;

    private void Start()
    {
        _playerAgent = GetComponent<NavMeshAgent>();
        _forward = new Vector3(0, 0, 1f);
    }

    public RaycastHit GetClickPosition(LayerMask layer, out bool hitSomething)
    {
        Ray ray = CameraFollow.MainCamera.ScreenPointToRay(Input.mousePosition);
        
        // Need to pass a maxDistance float in order to pass the layer as a parameter
        hitSomething = Physics.Raycast(ray, out var hitPosition, 1000f, layer);

        return hitPosition;
    }

    public RaycastHit IsPlayerReadyToInteract(float interactionRange, LayerMask layer, out bool hitSomething)
    {
        Transform playerTransform;
        Vector3 direction = (playerTransform = transform).TransformDirection(_forward);

        hitSomething = Physics.Raycast(playerTransform.position, direction, out var hit, interactionRange, layer);
        
        return hit;  
    }

    public void Move(Vector3 targetPosition)
    {
        CUBE.transform.position = targetPosition;
        _playerAgent.SetDestination(targetPosition);
        transform.LookAt(_playerAgent.steeringTarget);
    }

    public void StopMoving()
    {
        _playerAgent.enabled = false;
    }

    public bool IsPlayerWithinInteractionRange(Vector3 interactableObjectPosition, float interactionRange)
    {
        return Vector3.Distance(interactableObjectPosition, transform.position) < interactionRange;       
    }

    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();

        if (enemy != null)//&& enemy.IsEnemyAlive())
        {
            onPlayerDied?.Invoke();
        }
    }
}
