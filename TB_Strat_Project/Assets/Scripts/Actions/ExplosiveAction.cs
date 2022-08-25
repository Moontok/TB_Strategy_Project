using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ExplosiveAction : BaseAction
{
    [SerializeField] Transform explosivePrefab = null;
    [SerializeField] int maxRange = 7;
    [SerializeField] float weaponHeight = 1.7f;
    [SerializeField] LayerMask obstacleLayerMask = new LayerMask();

    void Update()
    {
        if (!isActive)
            return;

    }

    public override string GetActionName()
    {
        return "Explosive";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
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

                Vector3 unitWorldPosition = LevelGrid.Instance.GetWorldPosition(unitGridPosition);
                Vector3 aimPosition = unitWorldPosition + Vector3.up * weaponHeight;
                Vector3 targetPosition = LevelGrid.Instance.GetWorldPosition(testGridPosition) + Vector3.up * weaponHeight;
                Vector3 aimDirection = (targetPosition - unitWorldPosition).normalized;
                float distance = Vector3.Distance(unitWorldPosition, targetPosition);

                if (Physics.Raycast(aimPosition, aimDirection, distance, obstacleLayerMask))
                    continue;

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        Transform explosiveTransform = Instantiate(explosivePrefab, unit.GetWorldPosition() + Vector3.up * weaponHeight, Quaternion.identity);
        ExplosiveProjectile projectile = explosiveTransform.GetComponent<ExplosiveProjectile>();
        projectile.Setup(gridPosition, OnExplosiveBehaviorComplete, weaponHeight);

        ActionStart(onActionComplete);
    }

    void OnExplosiveBehaviorComplete()
    {
        ActionComplete();
    }
}
