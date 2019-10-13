using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerAnimationController : MonoBehaviour
{ 
    private Animator _playerAnimator;
    private Vector3 _vectorZero;
    private float _playerSpeed;
    private NavMeshAgent _playerAgent;

    private void Start()
    {
        _playerAnimator = GetComponentInChildren<Animator>();   
        _playerAgent = GetComponent<NavMeshAgent>();

        _vectorZero = new Vector3(0, 0, 0);
    }

    private void Update()
    {
        switch(PlayerController.PlayerCurrentState)
        {
            case PlayerController.PlayerState.Kinematic:
            case PlayerController.PlayerState.AboutToInteract:
            case PlayerController.PlayerState.AboutToRepairFence:
                _playerSpeed = Vector3.Distance(_playerAgent.velocity, _vectorZero);
                _playerAnimator.SetBool(GameText.PlayerRunning, _playerSpeed > 0f);
                break;

            case PlayerController.PlayerState.Interacting:
                _playerAnimator.SetBool(GameText.PlayerRunning, false);
                break;

            case PlayerController.PlayerState.Dead:
                _playerAnimator.SetTrigger(GameText.PlayerDied);
                break;
        }
    }
}
