using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitManager : SingletonMonoBehaviorBase<UnitManager>
{
    public event EventHandler SelectedUnitChanged;
    public event EventHandler SelectedActionChanged;
    public event EventHandler ActionStarted;
    public event EventHandler <bool> BusyStarted;

    [SerializeField] private Unit selectedUnit;
    [SerializeField] private LayerMask unitLayerMask;
    [SerializeField] private LayerMask uiLayerMask;


    private BaseAction selectedAction;
    private bool isBusy;

    // Update is called once per frame
    void Update()
    {
        if (isBusy) return;


        if (!TurnManager.Instance.IsPlayerTurn()) return;

        //if (EventSystem.current.IsPointerOverGameObject()) return;
        if (!Input.GetMouseButtonDown(0)) return;

        if (TryHandleUnitSelection()) return;

        if (IsOverGameObject()) return;
        if (selectedUnit == null || selectedAction == null) return;

        HandleSelectedAction();
    }

    private bool IsOverGameObject()
    {
        if (!EventSystem.current.IsPointerOverGameObject()) return false;


        var eventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        return results.Any(result => uiLayerMask == (uiLayerMask | (1 << result.gameObject.layer)));
    }

    private void HandleSelectedAction()
    {
        MouseWorld.GetPosition(out var newPoint);

        var mouseGridPosition = GridManager.Instance.GetGridPosition(newPoint);

        if (!selectedAction.CanPerformAction(mouseGridPosition))
            return;

        if (!selectedUnit.TrySpendActionPointsToTakeAction(selectedAction))
        {
            return;
        }

        SetBusy();
        selectedAction.DoAction(mouseGridPosition, ClearBusy);
        OnActionStarted();
        

    }

    private bool TryHandleUnitSelection()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out var raycastHit, float.MaxValue, unitLayerMask)) return false;
        if (!raycastHit.transform.TryGetComponent<Unit>(out var unit)) return false;

        if (unit == selectedUnit) return false;

        if (unit.IsEnemy()) return false;

        SetSelectedUnit(unit);
        SetSelectedAction(null);
        return true;
    }

    private void SetBusy()
    {
        isBusy = true;
        OnBusyStarted(true);
    }

    private void ClearBusy()
    {
        isBusy = false;

        OnBusyStarted(false);
    }

    public Unit GetSelectedUnit()
    {
        return selectedUnit;
    }

    private void SetSelectedUnit(Unit unit)
    {
        if (selectedUnit != null)
        {
            selectedUnit.Unselect();
        }

        selectedUnit = unit;
        selectedUnit?.Select();

        OnSelectedUnitChanged();
    }

    public void SetSelectedAction(BaseAction baseAction)
    {
        selectedAction = baseAction;
        OnSelectedActionChanged();
    }


    protected virtual void OnSelectedUnitChanged()
    {
        SelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    protected virtual void OnSelectedActionChanged()
    {
        SelectedActionChanged?.Invoke(this, EventArgs.Empty);
    }

    public BaseAction GetSelectedAction()
    {
        return selectedAction;
    }


    protected virtual void OnActionStarted()
    {
        ActionStarted?.Invoke(this, EventArgs.Empty);
    }

    protected virtual void OnBusyStarted(bool e)
    {
        BusyStarted?.Invoke(this, e);
    }
}
