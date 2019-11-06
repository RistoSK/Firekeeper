using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.EventSystems;

public class BuildingStateMachine : MonoBehaviour
{
    private Dictionary<Type, BuildingBaseState> _buildingModeStates;
    private HexCell _currentCell;

    private BuildingBaseState CurrentState { get; set; }

    public void SetState(Dictionary<Type, BuildingBaseState> states)
    {
        _buildingModeStates = states;

        CurrentState = _buildingModeStates[typeof(DefaultState)];
        CurrentState.OnStateExit += StateChanged;

        if (CurrentState == _buildingModeStates[typeof(BuildWallState)])
        {
            CurrentState.OnBuildModeCellSelected += CellSelected;
        }
    }

    public void SetWallBuildingMode(int mode)
    {
        // TODO: check if removing the null check breaks the logic, _currentCell == null
        if (!_currentCell)
        {
            _currentCell = HexGrid.Instance.GetCell(new Vector3(0, 0, 0));
        }

        if (_buildingModeStates == null)
        {
            return;
        }
        CurrentState?.OnExit();

        switch (mode)
        {
            case 1:
                SwitchToNewState(typeof(SingleWallState), _currentCell);
                break;
            case 2:
                SwitchToNewState(typeof(DrawingWallState), _currentCell);
                break;
            case 3:
                SwitchToNewState(typeof(RemoveWallState), _currentCell);
                break;
            case 4:
                SwitchToNewState(typeof(DefaultState), _currentCell);
                break;
            case 5:
                SwitchToNewState(typeof(BuildWallState), _currentCell);
                break;
            case 6:
                SwitchToNewState(typeof(RotatingWallState), _currentCell);
                break;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Ray inputRay = HexMapCamera.MainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(inputRay, out var hit, 1000f, BuildModeSettings.TempWallsLayerMask))
            {

            }
            else if (Physics.Raycast(inputRay, out hit, 1000f, BuildModeSettings.CellLayerMask))
            {
                _currentCell = HexGrid.Instance.GetCell(hit.point);
            }
        }     
        var nextState = CurrentState?.Tick();
       
        if (nextState != null && nextState != CurrentState?.GetType())
        {
            SwitchToNewState(nextState, _currentCell);
        }
    }

    private void SwitchToNewState(Type nextState, HexCell currentCell)
    {     
        CurrentState = _buildingModeStates[nextState];
        CurrentState.OnEnter(currentCell);      
    }

    private void StateChanged(HexCell currentCell)
    {
        _currentCell = currentCell;
    }

    private void CellSelected(HexCell currentCell)
    {
        _currentCell = currentCell;
        SwitchToNewState(typeof(PrepareToBuildWallState), currentCell);
    }
}
