using UnityEngine;
using UnityEngine.Events;

public class InteractableObjectController : MonoBehaviour
{
    [SerializeField] private UIPopUpView _buildView;

    public UnityAction OnFenceRepaired;

    public GameObject InteractableObject { get; set; }

    public void InitiateObjectInteraction(RaycastHit interactableHit, KinematicObject kinematicObject)
    {
        if (InteractableObject.GetComponent<Tree>() != null)
        {
            kinematicObject.StopMoving();
            _buildView.TreeClicked(interactableHit);
        }
        else if (InteractableObject.GetComponent<Base>() != null)
        {
            kinematicObject.StopMoving();
            _buildView.BaseClicked(interactableHit);
        }
        else if (InteractableObject.GetComponent<SpaceshipPart>() != null)
        {
            _buildView.SpaceshipPartClicked(interactableHit);
        }
        else if (InteractableObject.GetComponent<Spaceship>() != null)
        {
            _buildView.SpaceshipClicked(interactableHit);
        }
    }

    public void InitiateObjectCreation(PopUp.PopUpType type)
    {
        if (type != PopUp.PopUpType.RepairSpaceship)
        {
            InteractableObject.GetComponent<BoxCollider>().enabled = false;
        }
        
        switch (type)
        {
            case PopUp.PopUpType.BuildFence:
                StartCoroutine(InteractableObject.GetComponent<Base>().Build());
                break;
            case PopUp.PopUpType.HarvestWood:
                StartCoroutine(InteractableObject.GetComponent<Tree>().Harvest());
                break;
            case PopUp.PopUpType.RepairFence:
                StartCoroutine(InteractableObject.GetComponent<Fence>().Fix());
                OnFenceRepaired?.Invoke();
                break;
            case PopUp.PopUpType.SpaceshipPart:
                StartCoroutine(InteractableObject.GetComponent<SpaceshipPart>().PickUpPart());
                break;
            case PopUp.PopUpType.RepairSpaceship:
                StartCoroutine(InteractableObject.GetComponent<Spaceship>().FixSpaceship());
                break;
        }
    }
}
