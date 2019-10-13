using UnityEngine;

public class RootPopUpController : MonoBehaviour
{
    public enum PopUpType
    {
        Base,
        Fence,
        Tree,
        SpaceshipPart,
        Spaceship
    }

    [SerializeField] private PopUpController _popUpController;
    [SerializeField] public ResourceManager resourceManager;
    [SerializeField] private Canvas _popUpCanvas;

    [Header("Build PopUp")]
    [SerializeField] public GameObject buildPopUpGameObject;
    [SerializeField] public GameObject buildPopUpGameObjectContent;

    [Header("Tree PopUp")]
    [SerializeField] public GameObject treePopUpGameObject;
    [SerializeField] public GameObject treePopUpGameObjectContent;

    [Header("Spaceship parts PopUp")]
    [SerializeField] public GameObject spaceshipPartPopUpGameObject;
    [SerializeField] public GameObject spaceshipPartPopUpGameObjectContent;

    [Header("Spaceship PopUp")]
    [SerializeField] public GameObject spaceshipPopUpGameObject;
    [SerializeField] public GameObject spaceshipPopUpGameObjectContent;

    private void Start()
    {
        _popUpController.root = this;

        DisablePopUps();
    }

    public void InitiateController()
    {
        _popUpController.InitiateController();
    }

    public void DisablePopUps()
    {
        buildPopUpGameObject.SetActive(false);
        treePopUpGameObject.SetActive(false);
        spaceshipPartPopUpGameObject.SetActive(false);
        spaceshipPopUpGameObject.SetActive(false);
    }

    public void InstantiatePopUp(GameObject popUp, GameObject popUpContent, RaycastHit hit)
    {
        popUp.SetActive(true);

        Vector3 tempHitPoint = hit.point;
        Vector3 viewPortPoint = CameraFollow.MainCamera.WorldToViewportPoint(tempHitPoint);

        Vector3 screenSpaceCord = CameraFollow.MainCamera.WorldToScreenPoint(hit.point);
        RectTransform canvasRectTransform = _popUpCanvas.GetComponent<RectTransform>();
        Vector3 SpawnPosition;

        RectTransformUtility.ScreenPointToWorldPointInRectangle(canvasRectTransform, screenSpaceCord,
            CameraFollow.MainCamera, out SpawnPosition);

        popUp.transform.position = SpawnPosition;

        RotatePopUp(viewPortPoint, popUp, popUpContent);
    }

    private void RotatePopUp(Vector3 viewPortPoint, GameObject popUp, GameObject popUpContent)
    {
        if (viewPortPoint.x > 0.8f ||  (viewPortPoint.x > 0.2f && viewPortPoint.x < 0.5f))
        {
            Vector3 popUpEulerAngles = popUp.transform.rotation.eulerAngles;
            popUpEulerAngles.z = 180f;
            popUp.transform.rotation = Quaternion.Euler(popUpEulerAngles);

            Vector3 popUpContentEulerAngles = popUpContent.transform.rotation.eulerAngles;
            popUpContentEulerAngles.z = 0f;

            popUpContent.transform.rotation = Quaternion.Euler(popUpContentEulerAngles);
        }
        else
        {
            Vector3 popUpEulerAngles = popUp.transform.rotation.eulerAngles;
            popUpEulerAngles.z = 0;
            popUp.transform.rotation = Quaternion.Euler(popUpEulerAngles);

            Vector3 popUpContentEulerAngles = popUpContent.transform.rotation.eulerAngles;
            popUpContentEulerAngles.z = 0f;

            popUpContent.transform.rotation = Quaternion.Euler(popUpContentEulerAngles);
        }
    }

}
