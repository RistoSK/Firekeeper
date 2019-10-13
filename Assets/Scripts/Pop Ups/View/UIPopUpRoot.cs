using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPopUpRoot : UIRoot
{
    [SerializeField] private UIPopUpView _buildView;

    public UIPopUpView BuildView => _buildView;

    public override void ShowRoot()
    {
        base.ShowRoot();

        _buildView.ShowView();
    }

    public override void HideRoot()
    {
        _buildView.HideView();

        base.HideRoot();
    }
}