using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SteeringBehavior))]
[RequireComponent(typeof(Unit))]
public class MoveAction : BaseAction
{
    [SerializeField] private float speed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private int maxMoveDistance = 4;

    private Vector3 targetPosition;

    private SteeringBehavior _steeringBehavior;

    public event EventHandler StartMoving;
    public event EventHandler StopMoving;


    //private Action callbackAction;

    protected override void Awake()
    {
        base.Awake();

        _steeringBehavior = GetComponent<SteeringBehavior>();
        targetPosition = transform.position;
    }

    public override void Update()
    {
        if (!IsActive) return;

        var currentSpeed = 0f;

        // TODO: call correct behavior based on our state later in game
        if (!_steeringBehavior.Arrive(targetPosition, out var newPosition))
        {
            transform.position += newPosition * Time.deltaTime * speed;

            transform.forward = Vector3.Lerp(transform.forward, (targetPosition - transform.position).normalized,
                Time.deltaTime * rotateSpeed);

            currentSpeed = newPosition.magnitude;
        }
        else
        {
            OnStopMoving();
            ActionComplete();
        }

    }

    public override bool CanPerformAction(GridPosition payload)
    {
        return IsValidActionGridPosition(payload);
    }

    public override void DoAction(GridPosition payload, Action onActionComplete)
    {
        Move(payload);
        OnStartMoving();
        ActionStart(onActionComplete);
    }

    public override string GetActionName()
    {
        return "MOVE";
    }

    private void Move(GridPosition gridPosition)
    {
        targetPosition = GridManager.Instance.GetWorldPosition(gridPosition);
    }


    public override bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        var validGridPositionList = GetValidActionGridPositionList();
        return validGridPositionList.Contains(gridPosition);
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        var validGridPositionList = new List<GridPosition>();

        var unitGridPosition = unit.GetGridPosition();

        for (var x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for (var z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                var offsetGridPosition = new GridPosition(x, z);
                var testGridPosition = unitGridPosition + offsetGridPosition;

                if (!GridManager.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                if (unitGridPosition == testGridPosition)
                {
                    // Same Grid Position where the unit is already at
                    continue;
                }

                if (GridManager.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    // Grid Position already occupied with another Unit
                    continue;
                }

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }


    protected virtual void OnStartMoving()
    {
        StartMoving?.Invoke(this, EventArgs.Empty);
    }

    protected virtual void OnStopMoving()
    {
        StopMoving?.Invoke(this, EventArgs.Empty);
    }
}
