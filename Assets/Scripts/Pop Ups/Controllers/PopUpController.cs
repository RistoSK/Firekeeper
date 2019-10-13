using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpController : SubController<UIPopUpRoot>
{
    private Ray _ray;

    public override void InitiateController()
    {
        ui.BuildView.OnInteractiveObjectClicked += InitiatePopUp;
        ui.BuildView.OnClickElsewhere += DisablePopUp;

        base.InitiateController();
    }

    public override void DisableController()
    {
        base.DisableController();

        ui.BuildView.OnClickElsewhere -= DisablePopUp;
        ui.BuildView.OnInteractiveObjectClicked -= InitiatePopUp;
    }

    private void InitiatePopUp(RootPopUpController.PopUpType type, RaycastHit hit)
    {
        root.DisablePopUps();

        switch (type)
        {
            case RootPopUpController.PopUpType.Base:
                root.InstantiatePopUp(root.buildPopUpGameObject, root.buildPopUpGameObjectContent, hit);
                break;
            case RootPopUpController.PopUpType.Tree:
                root.InstantiatePopUp(root.treePopUpGameObject, root.treePopUpGameObjectContent, hit);
                break;
            case RootPopUpController.PopUpType.SpaceshipPart:
                root.InstantiatePopUp(root.spaceshipPartPopUpGameObject, root.spaceshipPartPopUpGameObjectContent, hit);
                break;
            case RootPopUpController.PopUpType.Spaceship:
                root.InstantiatePopUp(root.spaceshipPopUpGameObject, root.spaceshipPopUpGameObjectContent, hit);
                break;
        }
    }

    private void DisablePopUp()
    {
        root.DisablePopUps();
    }
}
