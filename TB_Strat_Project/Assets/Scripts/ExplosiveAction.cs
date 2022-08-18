using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveAction : BaseAction
{
    [SerializeField] int maxRange = 7;
    //[SerializeField] float weaponHeight = 1.7f;

    void Update()
    {
        if (!isActive)
            return;

        ActionComplete();
    }

    public override string GetActionName()
    {
        return "Explosive";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            GridPosition = gridPosition,
            actionValue = 0
        };
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxRange; x <= maxRange; x++)
        {
            for (int z = -maxRange; z <= maxRange; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                    continue;

                int testDistance = Mathf.RoundToInt(Mathf.Sqrt(Mathf.Abs(x * x) + Mathf.Abs(z * z)));

                if (testDistance > maxRange)
                    continue;

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        Debug.Log("Explosive Action");
        ActionStart(onActionComplete);
    }
}
