using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DrawingWallState : BuildingBaseState
{
    private bool _isDrag;
    private HexDirection _dragDirection;

    public DrawingWallState() : base()
    {
       
    }

    public override void OnEnter(HexCell currentCell)
    {
        Cell = currentCell;
    }

    public override Type Tick()
    {
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Ray inputRay = HexMapCamera.MainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(inputRay, out var hit, 1000f, BuildModeSettings.CellLayerMask))
            {
                Cell = HexGrid.Instance.GetCell(hit.point);
                //Cell.color = BuildModeSettings.DrawColor;

                if (PreviousCell && PreviousCell != Cell)
                {
                    ValidateDrag(Cell);
                }
                else
                {
                    _isDrag = false;
                }

                if (_isDrag)
                {
                    HexCell otherCell = Cell.GetNeighbor(_dragDirection.Opposite());
                    
                    // TODO: check if removing the null check breaks the logic, otherCell == null
                    if (!otherCell)
                    {
                        return null;
                    }

                    PreviousCell.AddWall(_dragDirection, BuildModeSettings.Wall);
                    //PreviousCell.color = BuildModeSettings.DrawColor;
                }

                PreviousCell = Cell;
            }
        }
        return null;
    }

    public override void OnExit()
    {
        StateExit();
    }

    private void ValidateDrag(HexCell currentCell)
    {
        for (_dragDirection = HexDirection.NorthEast; _dragDirection <= HexDirection.NorthWest; _dragDirection++)
        {
            if (PreviousCell.GetNeighbor(_dragDirection) == currentCell)
            {
                _isDrag = true;
                return;
            }
        }
        _isDrag = false;
    }
}
