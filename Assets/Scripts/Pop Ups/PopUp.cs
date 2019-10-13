using UnityEngine;
using UnityEngine.UI;

public class PopUp : MonoBehaviour
{
    public enum PopUpType
    {
        RepairFence = 1 << 0,
        BuildFence = 1 << 1,
        HarvestWood = 1 << 2,
        SpaceshipPart = 1 << 3,
        RepairSpaceship = 1 << 4
    }

    [SerializeField] private PopUpType _type;
    [SerializeField] private InteractableObjectController _interactableManager;
    [SerializeField] private ResourceManager _resourceManager;

    private Button _button;
    private Text _text;

    private void Awake()
    {
        _button = gameObject.GetComponentInChildren<Button>();

        if (_type != PopUpType.RepairFence)
        {
            _button.onClick.AddListener(PopUpButtonClicked);
        }      

        _text = gameObject.GetComponentInChildren<Text>();

        _resourceManager.OnWoodAmountChanged += SetUpText;
    }

    private void OnEnable()
    {
        if (_text == null)
        {
            return;
        }

        SetUpText();

        if (_type == PopUpType.RepairSpaceship)
        {
            _text.text = _resourceManager.GetSpaceshipParts().ToString();
        }
    }

    private void SetUpText()
    {
        switch (_type)
        {
            case PopUpType.BuildFence:
                _text.text = _resourceManager.BaseBuildingCost.ToString();
                ChangeFontColor(_text);
                break;
            case PopUpType.HarvestWood:
                _text.text = _resourceManager.TreeHarvestingGains.ToString();
                break;
            case PopUpType.RepairFence:
                _text.text = _resourceManager.FenceRepairingCost.ToString();
                ChangeFontColor(_text);
                break;
            case PopUpType.RepairSpaceship:
                ChangeFontColor(_text);
                break;
            case PopUpType.SpaceshipPart:
                break;
        }
    }

    public void PopUpButtonClicked()
    {
        _interactableManager.InitiateObjectCreation(_type);

        switch (_type)
        {
            case PopUpType.RepairFence:
                _resourceManager.RemoveResources(_type);
                break;
            case PopUpType.BuildFence:
                _resourceManager.RemoveResources(_type);
                break;
            case PopUpType.HarvestWood:
                _resourceManager.AddResources(_type);
                break;
            case PopUpType.SpaceshipPart:
                _resourceManager.AddResources(_type);
                break;
            case PopUpType.RepairSpaceship:
                _resourceManager.AddResources(_type);
                _resourceManager.RemoveResources(_type);
                break;
            default:
                Debug.LogError("Pop Up type not supported");
                break;
        }

        gameObject.SetActive(false);
    }

    private void ChangeFontColor(Text amountText)
    {
        if (_resourceManager.HasEnoughResources(_type))
        {
            amountText.color = Color.black;
            _button.interactable = true;
        }
        else
        {
            amountText.color = Color.red;
            _button.interactable = false;
        }
    }
}
