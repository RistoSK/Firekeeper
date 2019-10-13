using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class KinematicObject : MonoBehaviour
{
    [SerializeField] private GameObject CUBE;

    private NavMeshAgent _playerAgent;
    private Vector3 _forward;

    public UnityAction OnPlayerDied;

    private void Start()
    {
        _playerAgent = GetComponent<NavMeshAgent>();
        _forward = new Vector3(0, 0, 1f);
    }

    public RaycastHit GetClickPosition(LayerMask layer, out bool hitSomething)
    {
        Ray ray = CameraFollow.MainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitPosition;

        // Need to pass a maxDistance float in order to pass the layer as a parameter
        hitSomething = Physics.Raycast(ray, out hitPosition, 1000f, layer);

        return hitPosition;
    }

    public RaycastHit IsPlayerReadyToInteract(float interactionRange, LayerMask layer, out bool hitSomething)
    {
        Vector3 direction = transform.TransformDirection(_forward);
        RaycastHit hit;

        hitSomething = Physics.Raycast(transform.position, direction, out hit, interactionRange, layer);
        if (hitSomething)
        {
            Debug.DrawRay(transform.position, direction, Color.red, 1f);
        }
        else
        {
            Debug.DrawRay(transform.position, direction, Color.blue, 1f);
        }
        
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
            OnPlayerDied?.Invoke();
        }
    }
}
