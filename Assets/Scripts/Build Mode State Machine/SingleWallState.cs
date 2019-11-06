using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SingleWallState : BuildingBaseState
{
    private readonly Dictionary<HexDirection, GameObject> _tempWalls;

    public SingleWallState(Dictionary<HexDirection, GameObject> tempWalls) : base()
    {
        _tempWalls = tempWalls;
    }

    public override void OnEnter(HexCell currentCell)
    {
        Cell = currentCell;

        Cell.ShowTempWalls(_tempWalls);
    }

    public override Type Tick()
    {
        Ray inputRay = HexMapCamera.MainCamera.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (Physics.Raycast(inputRay, out var hitWall, 1000f, BuildModeSettings.TempWallsLayerMask))
            {
                WallProperties clickedWallToBeBuilt = hitWall.collider.gameObject.GetComponent<WallProperties>();

                //Cell.color = BuildModeSettings.SingleColor;
                Cell.AddWall(clickedWallToBeBuilt.Direction, BuildModeSettings.Wall);

                UpdateCurrentAndPreviousCell(clickedWallToBeBuilt.Direction);
            }
            else if (Physics.Raycast(inputRay, out hitWall, 1000f, BuildModeSettings.CellLayerMask))
            {
                Cell = HexGrid.Instance.GetCell(hitWall.point);              
            }
            Cell.ShowTempWalls(_tempWalls);
            
        }
        
        return null;
    }

    public override void OnExit()
    {
        DeactivateTempWalls();
        StateExit();
    }

    private void DeactivateTempWalls()
    {
        for (HexDirection direction = HexDirection.NorthEast; direction <= HexDirection.NorthWest; direction++)
        {
            _tempWalls[direction].SetActive(false);
        }
    }

    private void UpdateCurrentAndPreviousCell(HexDirection direction)
    {
        PreviousCell = Cell;
        Cell = Cell.GetNeighbor(direction);
    }
}
