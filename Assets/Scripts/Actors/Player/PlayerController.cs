using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerController : MonoBehaviour
{
    public enum PlayerState
    {
        Kinematic,
        AboutToRepairFence,
        Interacting,
        AboutToInteract,
        Dead
    }

    public static PlayerState PlayerCurrentState;

    [SerializeField] private RootPopUpController _rootPopUpController;
    [SerializeField] private InteractableObjectController _interactableManager;
    [SerializeField] private LayerMask _moveableLayers;
    [SerializeField] private LayerMask _interactibleLayers;
    [SerializeField] private float _interactionRange;

    [SerializeField] private GameObject _gameOverCanvas;

    private NavMeshAgent _playerAgent;

    private KinematicObject _kinematicObject;
    private bool _bInteractableRaycastSucceeded;
    private bool _bMoveRaycastSucceeded;

    private RaycastHit _interactableHit;

    public static Vector3 PlayerPosition { private set; get; }

    private void Start()
    {
        _kinematicObject = GetComponent<KinematicObject>();
        _playerAgent = GetComponent<NavMeshAgent>();

        PlayerCurrentState = PlayerState.Kinematic;

        PlayerPosition = transform.position;

        _kinematicObject.onPlayerDied += PlayerDied;
        RepairFencePopUpController.RepairFencePopUpControllerInstance.OnFencePopUpClicked += FencePopUpClicked;
        GameManager.OnModeChanged += GameModeChanged;
    }

    private void Update()
    {
        //TODO find a better way to pass players positions
        PlayerPosition = transform.position;

        if (Input.GetMouseButtonDown(0))
        {
            if ((Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began))
            {
                if (EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId))
                {
                    return;
                }     
            }

            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            if (GameManager.CurrentGameMode == GameManager.GameMode.BuildingMode)
            {
                return;
            }
            
            if (!_playerAgent.enabled)
            {
                _playerAgent.path.ClearCorners();
                _kinematicObject.SetPlayerCanMove(true);
            }

            _rootPopUpController.DisablePopUps();

            RaycastHit moveHit = _kinematicObject.GetClickPosition(_moveableLayers, out _bMoveRaycastSucceeded);

            if (_bMoveRaycastSucceeded)
            {
                _kinematicObject.Move(moveHit.point);
            }

            RaycastHit interactableHit = _kinematicObject.GetClickPosition(_interactibleLayers, out _bInteractableRaycastSucceeded);
           
            if (_bInteractableRaycastSucceeded)
            {
                PlayerCurrentState = PlayerState.AboutToInteract;

                _kinematicObject.Move(interactableHit.point);

                _interactableManager.InteractableObject = interactableHit.transform.gameObject;
                _interactableHit = interactableHit;
            }
            else
            {
                PlayerCurrentState = PlayerState.Kinematic;
                
                _interactableManager.InteractableObject = null;
            }          
        }

        if (PlayerCurrentState == PlayerState.AboutToInteract)
        {
            RaycastHit hit = _kinematicObject.IsPlayerReadyToInteract(_interactionRange, _interactibleLayers, out var hitsomething);

            if (hitsomething)
            {
                if (hit.collider.gameObject == _interactableManager.InteractableObject)
                {
                    PlayerInteracting();
                }
            }
        }

        if (PlayerCurrentState == PlayerState.AboutToRepairFence)
        {
            if (Math.Abs(PlayerPosition.x - _playerAgent.destination.x) < 0.1f && Math.Abs(PlayerPosition.z - _playerAgent.destination.z) < 0.1f)
            {
                _kinematicObject.SetPlayerCanMove(false);
                _interactableManager.InitiateObjectCreation(PopUp.PopUpType.RepairFence);
            }
        }
    }

    private void PlayerInteracting()
    {
        PlayerCurrentState = PlayerState.Interacting;

        _rootPopUpController.InitiateController();
        _interactableManager.InitiateObjectInteraction(_interactableHit, _kinematicObject);
    }

    private void PlayerDied()
    {
        PlayerCurrentState = PlayerState.Dead;

        GameSettings.GameOver();
    }

    private void FencePopUpClicked(GameObject fence, Transform playerRepairTargetTransform)
    {
        PlayerCurrentState = PlayerState.AboutToRepairFence;

        if (!_playerAgent.enabled)
        {
            _playerAgent.path.ClearCorners();
            _kinematicObject.SetPlayerCanMove(true);
        }

        _kinematicObject.Move(playerRepairTargetTransform.position);
        _interactableManager.InteractableObject = fence;
    }

    private void GameModeChanged(GameManager.GameMode gameMode)
    {
        switch (gameMode)
        {
            case GameManager.GameMode.MainGame:
                _kinematicObject.SetPlayerCanMove(true);
                break;
            case GameManager.GameMode.BuildingMode:
                _kinematicObject.SetPlayerCanMove(false);
                break;
        }
    }
}
