using UnityEngine;
using System;

public abstract class BuildingBaseState
{
    protected BuildingBaseState()
    {

    }

    public event Action<HexCell> OnStateExit;
    public event Action<HexCell> OnBuildModeCellSelected;

    protected HexCell Cell { get; set; }
    protected HexCell PreviousCell { get; set; }

    protected Transform transform;

    protected void StateExit()
    {
        OnStateExit?.Invoke(Cell);
    }

    protected void CellSelected()
    {
        OnBuildModeCellSelected?.Invoke(Cell);    
    }

    protected void ChangeCellColor(HexCell cell, Color color)
    {
        cell.Color = color;
    }
    
    public abstract void OnEnter(HexCell currentCell);
    public abstract Type Tick();
    public abstract void OnExit();
}
