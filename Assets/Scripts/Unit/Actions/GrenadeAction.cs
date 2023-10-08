using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeAction : BaseAction
{
    public override void Update()
    {
        
    }

    public override bool CanPerformAction(GridPosition position)
    {
        return true;
    }

    public override void DoAction(GridPosition position, Action callbackAction)
    {
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        return new List<GridPosition>();
    }

    public override string GetActionName()
    {
        return "Grenade";
    }

    public override bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        return false;
    }
}
