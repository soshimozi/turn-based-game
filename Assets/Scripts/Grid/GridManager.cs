using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : SingletonMonoBehaviorBase<GridManager>
{
    [SerializeField] private Transform gridDebugObjectPrefab;

    private GridSystem gridSystem;

    public event EventHandler AnyUnitChangedGridPosition;

    protected override void Awake()
    {
        base.Awake();

        gridSystem = new GridSystem(10, 10, 2f);
        gridSystem.CreateDebugObjects(gridDebugObjectPrefab);

    }

    public void AddUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        var gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.AddUnit(unit);
    }

    public List<Unit> GetUnitListAtGridPosition(GridPosition gridPosition)
    {
        var gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnitList();
    }

    public void RemoveUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        var gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.RemoveUnit(unit);
    }

    public void UnitMovedGridPosition(Unit unit, GridPosition fromGridPosition, GridPosition toGridPosition)
    {
        RemoveUnitAtGridPosition(fromGridPosition, unit);
        AddUnitAtGridPosition(toGridPosition, unit);

        OnAnyUnitChangedGridPosition();
    }

    public GridPosition GetGridPosition(Vector3 worldPosition) => gridSystem.GetGridPosition(worldPosition);

    public Vector3 GetWorldPosition(GridPosition gridPosition) => gridSystem.GetWorldPosition(gridPosition);

    public bool IsValidGridPosition(GridPosition gridPosition) => gridSystem.IsValidGridPosition(gridPosition);

    public bool HasAnyUnitOnGridPosition(GridPosition gridPosition)
    {
        var gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.HasAnyUnit();
    }

    public bool IsValidActionGridPositionForUnit(GridPosition position, Unit unit) =>
        gridSystem.IsValidActionGridPositionForUnit(position, unit);

    public int Width
    {
        get => gridSystem.Width;
    }

    public int Height
    {
        get => gridSystem.Height;
    }

    public Unit GetUnitAtGridPosition(GridPosition gridPosition)
    {
        var gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnit();
    }


    protected virtual void OnAnyUnitChangedGridPosition()
    {
        AnyUnitChangedGridPosition?.Invoke(this, EventArgs.Empty);
    }
}
