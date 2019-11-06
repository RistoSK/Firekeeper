using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RemoveWallState : BuildingBaseState
{
    public RemoveWallState() : base()
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

            if (Physics.Raycast(inputRay, out var hitWall, 1000f, BuildModeSettings.WallLayerMask))
            {
                Cell = HexGrid.Instance.GetCell(hitWall.point);

                //Cell.color = BuildModeSettings.RemoveColor;
                Cell.RemoveWall(hitWall.collider.gameObject);
            }
        }
        return null;
    }

    public override void OnExit()
    {
        StateExit();
    }
}
