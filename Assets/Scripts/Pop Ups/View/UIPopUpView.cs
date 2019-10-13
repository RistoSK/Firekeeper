using UnityEngine;
using UnityEngine.Events;

public class UIPopUpView : UIView
{
    public UnityAction<RootPopUpController.PopUpType, RaycastHit> OnInteractiveObjectClicked;

    public UnityAction OnClickElsewhere;

    public void BaseClicked(RaycastHit hit)
    {
        OnInteractiveObjectClicked?.Invoke(RootPopUpController.PopUpType.Base, hit);
    }

    public void TreeClicked(RaycastHit hit)
    {
        OnInteractiveObjectClicked?.Invoke(RootPopUpController.PopUpType.Tree, hit);
    }

    public void SpaceshipPartClicked(RaycastHit hit)
    {
        OnInteractiveObjectClicked?.Invoke(RootPopUpController.PopUpType.SpaceshipPart, hit);
    }

    public void SpaceshipClicked(RaycastHit hit)
    {
        OnInteractiveObjectClicked?.Invoke(RootPopUpController.PopUpType.Spaceship, hit);
    }

    public void ClickedElsewhere()
    {
        OnClickElsewhere?.Invoke();
    }
}
