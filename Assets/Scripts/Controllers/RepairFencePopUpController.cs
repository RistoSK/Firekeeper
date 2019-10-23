using System;
using UnityEngine;

public class RepairFencePopUpController : MonoBehaviour
{
    [SerializeField] private RepairFencePopUp[] _repairFencePopUps;

    public static RepairFencePopUpController RepairFencePopUpControllerInstance { get; private set; }

    public event Action<GameObject, Transform> OnFencePopUpClicked;

    private void Awake()
    {
        if (RepairFencePopUpControllerInstance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            RepairFencePopUpControllerInstance = this;
        }
    }

    private void Start()
    {
        for(int i = 0; i < _repairFencePopUps.Length; i++)
        {
            _repairFencePopUps[i].OnFencePopUpClicked += FencePopUpClicked;
        }
    }

    public void FencePopUpClicked(GameObject fence, Transform playerRepairTargetTransform)
    {
        OnFencePopUpClicked?.Invoke(fence, playerRepairTargetTransform);
    }
}
