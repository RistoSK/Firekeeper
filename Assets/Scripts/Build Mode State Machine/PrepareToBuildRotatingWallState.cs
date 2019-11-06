using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PrepareToBuildRotatingWallState : BuildingBaseState
{
    private readonly Dictionary<HexDirection, GameObject> _tempWalls;
    
    private HexDirection _dragDirection;
    private HexDirection _currentCellDirection;
    
    private HexCell _cellToBeBuilt;
    
    private bool _isDrag;
    private bool _isDragConfirmed;
    
    public PrepareToBuildRotatingWallState(Dictionary<HexDirection, GameObject> tempWalls) : base()
    {
        _tempWalls = tempWalls;
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
                _cellToBeBuilt = HexGrid.Instance.GetCell(hit.point);

                if (PreviousCell && PreviousCell != _cellToBeBuilt)
                {
                    ValidateDrag(_cellToBeBuilt);
                }
                else
                {
                    _isDrag = false;
                    //_isDragConfirmed = false;
                }

                if (_isDrag)
                {
                    HexCell otherCell = Cell.GetNeighbor(_dragDirection.Opposite());

                    // TODO: check if removing the null check breaks the logic, otherCell == null
                    if (!otherCell)
                    {
                        return null;
                    }

                    DeactivateTempWalls();

                    _currentCellDirection = _dragDirection;
                    Cell.ShowContainedTempWall(_tempWalls, _dragDirection);
                    _isDragConfirmed = true;
                }
            }
        }
        
        if (Input.GetMouseButtonUp(0) && _isDragConfirmed)
        {
            Ray inputRay = HexMapCamera.MainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(inputRay, out var hit, 1000f, BuildModeSettings.CellLayerMask))
            {
                _isDragConfirmed = false;
                
                if (HexGrid.Instance.GetCell(hit.point) == Cell)
                {
                    DeactivateTempWalls();
                    ChangeCellColor(Cell, Color.white);
                    return typeof(BuildWallState);
                }
            }
            
            Cell.AddRotatingWall(_currentCellDirection, BuildModeSettings.Wall);
            ChangeCellColor(Cell, Color.white);
            Cell = Cell.GetNeighbor(_currentCellDirection);
            ChangeCellColor(Cell, Color.gray);
        }
        else
        {
            PreviousCell = _cellToBeBuilt;
        }
        
        return null;
    }

    public override void OnExit()
    {
        
    }
    
    private void ValidateDrag(HexCell currentCell)
    {
        for (_dragDirection = HexDirection.NorthEast; _dragDirection <= HexDirection.NorthWest; _dragDirection++)
        {
            if (Cell.GetNeighbor(_dragDirection) == currentCell)
            {
                _isDrag = true;
                return;
            }
        }
        _isDrag = false;
    }
    
    private void DeactivateTempWalls()
    {
        for (HexDirection direction = HexDirection.NorthEast; direction <= HexDirection.NorthWest; direction++)
        {
            _tempWalls[direction].SetActive(false);
        }
    }
}
