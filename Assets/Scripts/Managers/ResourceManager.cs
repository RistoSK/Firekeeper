using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class ResourceManager : MonoBehaviour
{
    private Dictionary<ResourceType, int> _currentResources;

    [SerializeField] private int _woodAmount = 0;
    [SerializeField] private int _spaceshipPartsAmount = 0;
    [SerializeField] private int _attahedSpaceshipParts = 0;

    [SerializeField] private Text _woodAmountText;
    [SerializeField] private Text _spaceshipAmountText;
    [SerializeField] private TextMeshProUGUI _spaceshipHudAmountText;

    [SerializeField] private int _fenceRepairingPrice;
    [SerializeField] private int _baseBuildingPrice;
    [SerializeField] private int _treeHarvestingGaining;

    private int _maxSpaceshipParts;

    public UnityAction OnWoodAmountChanged;

    private void Awake()
    {
        _currentResources = new Dictionary<ResourceType, int>();

        _currentResources.Add(ResourceType.Wood, _woodAmount);
        _currentResources.Add(ResourceType.SpaceshipParts, _spaceshipPartsAmount);
        _currentResources.Add(ResourceType.AttachedSpaceshipParts, _attahedSpaceshipParts);

        UpdateDisplay();
    }

    public int FenceRepairingCost => _fenceRepairingPrice;

    public int BaseBuildingCost => _baseBuildingPrice;

    public int TreeHarvestingGains => _treeHarvestingGaining;

    private void UpdateDisplay()
    {
        _woodAmountText.text = _currentResources[ResourceType.Wood].ToString();
        _spaceshipAmountText.text = _currentResources[ResourceType.SpaceshipParts].ToString() + " / " + _maxSpaceshipParts;
        _spaceshipHudAmountText.text = _currentResources[ResourceType.AttachedSpaceshipParts].ToString() + " / " + _maxSpaceshipParts;
    }

    public int GetResourcesAmount(ResourceType type)
    {
        if (!_currentResources.ContainsKey(type))
        {
            Debug.LogError("Resource type " + type + " does not exist");
            return 0;
        }

        return _currentResources[type];
    }


    public void AddResources(PopUp.PopUpType popUpType)
    {
        switch (popUpType)
        {
            case PopUp.PopUpType.HarvestWood:
                _currentResources[ResourceType.Wood] += _treeHarvestingGaining;
                OnWoodAmountChanged?.Invoke();
                break;
            case PopUp.PopUpType.SpaceshipPart:
                _currentResources[ResourceType.SpaceshipParts]++;
                break;
            case PopUp.PopUpType.RepairSpaceship:
                _currentResources[ResourceType.AttachedSpaceshipParts] += _currentResources[ResourceType.SpaceshipParts];
                break;
        }

        UpdateDisplay();
    }

    public void RemoveResources(PopUp.PopUpType popUpType)
    {
        switch (popUpType)
        {
            case PopUp.PopUpType.BuildFence:
                _currentResources[ResourceType.Wood] -= _baseBuildingPrice;
                OnWoodAmountChanged?.Invoke();
                break;
            case PopUp.PopUpType.RepairFence:
                _currentResources[ResourceType.Wood] -= _fenceRepairingPrice;
                OnWoodAmountChanged?.Invoke();
                break;
            case PopUp.PopUpType.RepairSpaceship:
                _currentResources[ResourceType.SpaceshipParts] = 0;
                break;
        }

        UpdateDisplay();
    }

    public void ClearResources()
    {
        _woodAmountText.text = 0.ToString();
        _spaceshipAmountText.text = 0.ToString();

        _currentResources.Clear();
    }

    public bool HasEnoughResources(PopUp.PopUpType popUpType)
    {
        switch(popUpType)
        {
            case PopUp.PopUpType.BuildFence:
                return _baseBuildingPrice <= _currentResources[ResourceType.Wood];
            case PopUp.PopUpType.RepairFence:
                return _fenceRepairingPrice <= _currentResources[ResourceType.Wood];
            case PopUp.PopUpType.RepairSpaceship:
                return _currentResources[ResourceType.SpaceshipParts] >= 1;
            default:
                return true;
        }
    }

    public int GetSpaceshipParts()
    {
        return _currentResources[ResourceType.SpaceshipParts];
    }

    public int GiveSpaceshipParts()
    {
        int parts = _currentResources[ResourceType.SpaceshipParts];
        _currentResources[ResourceType.SpaceshipParts] = 0;
        return parts;
    }

    public void SetMaxSpaceshipParts(int maxParts)
    {
        _maxSpaceshipParts = maxParts;

        UpdateDisplay();
    }
}
