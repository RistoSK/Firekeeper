using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RepairFencePopUp : MonoBehaviour
{
    private const PopUp.PopUpType _type = PopUp.PopUpType.RepairFence;

    [SerializeField] private InteractableObjectController _interactableManager;
    [SerializeField] private ResourceManager _resourceManager;
    [SerializeField] private Transform _playerRepairTargetTransform;
    [SerializeField] private GameObject _fence;

    private RepairFencePopUpController _repairFencePopUpController;
    private Button _button;
    private Text _amountText;

    public event Action<GameObject, Transform> OnFencePopUpClicked;

    private void Start()
    {
        _button = GetComponentInChildren<Button>();
        _amountText = GetComponentInChildren<Text>();
        _repairFencePopUpController = GetComponentInParent<RepairFencePopUpController>();

        _button.onClick.AddListener(RepairFencePopUpClicked);

        _amountText.text = _resourceManager.FenceRepairingCost.ToString();

        SetUpText();

        _resourceManager.OnWoodAmountChanged += SetUpText;       
    }

    public Fence.FenceStatus GetFenceStatus()
    {
        return _fence.GetComponent<Fence>().status;
    }

    private void RepairFencePopUpClicked()
    {
        OnFencePopUpClicked?.Invoke(_fence, _playerRepairTargetTransform);

        _interactableManager.OnFenceRepaired += PayResourceFee;        
    }

    private void SetUpText()
    {
        if (_resourceManager.HasEnoughResources(_type))
        {
            _amountText.color = Color.black;
            _button.interactable = true;
        }
        else
        {
            _amountText.color = Color.red;
            _button.interactable = false;
        }
    }

    private void PayResourceFee()
    {
        _interactableManager.OnFenceRepaired -= PayResourceFee;

        _resourceManager.RemoveResources(_type);
    }
}
