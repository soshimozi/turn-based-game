using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    protected bool isActive;
    protected Unit unit;

    protected Action onActionComplete;

    public static event EventHandler AnyActionStarted;
    public static event EventHandler AnyActionComplete;

    public abstract void Update();

    public virtual bool CanPerformAction(GridPosition position)
    {
        return IsValidActionGridPosition(position);
    }
    public abstract void DoAction(GridPosition position, Action callbackAction);

    public abstract List<GridPosition> GetValidActionGridPositionList();
    public abstract string GetActionName();
    public virtual bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositionList = GetValidActionGridPositionList();
        return validGridPositionList.Contains(gridPosition);
    }

    public string ActionName => GetActionName();

    public virtual int GetActionPointsCost()
    {
        return 1;
    }

    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();
    }

    protected void ActionStart(Action onActionComplete)
    {
        isActive = true;
        this.onActionComplete = onActionComplete;

        OnAnyActionStarted(this);
    }

    protected void ActionComplete()
    {
        isActive = false;
        onActionComplete();

        OnAnyActionComplete(this);
    }

    public bool IsActive => isActive;


    private static void OnAnyActionStarted(BaseAction action)
    {
        AnyActionStarted?.Invoke(action, EventArgs.Empty);
    }

    private static void OnAnyActionComplete(BaseAction action)
    {
        AnyActionComplete?.Invoke(action, EventArgs.Empty);

    }

    public Unit Unit => unit;
}
