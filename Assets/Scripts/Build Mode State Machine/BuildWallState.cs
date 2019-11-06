using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildWallState : BuildingBaseState
{
    private bool _isDrag;
    private bool _readyToBuild;
    private HexDirection _dragDirection;
    
    public BuildWallState() : base()
    {

    }
    
    public override void OnEnter(HexCell currentCell)
    {
        Cell = currentCell;
    }

    public override Type Tick()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Ray inputRay = HexMapCamera.MainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(inputRay, out var hit, 1000f, BuildModeSettings.CellLayerMask))
            {
                Cell = HexGrid.Instance.GetCell(hit.point);
                ChangeCellColor(Cell, Color.gray);
                
                return typeof(PrepareToBuildWallState);
            }
        }

        return null;
    }

    public override void OnExit()
    {
        StateExit();
    }
}
