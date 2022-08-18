using System;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{
    public event EventHandler OnStartMoving;
    public event EventHandler OnStopMoving;

    [SerializeField] int maxMoveDistance = 4;

    float moveSpeed = 4f;
    float stoppingDistance = 0.1f;
    float rotationSpeed = 10f;
    List<Vector3> positions = null;
    int currentPosIndex = 0;

    void Update()
    {
        if (!isActive) return;


        Vector3 targetPosition = positions[currentPosIndex];
        Vector3 moveDirection = (targetPosition - transform.position).normalized;

        transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotationSpeed);

        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }
        else
        {
            currentPosIndex++;
            if (currentPosIndex >= positions.Count)
            {
                OnStopMoving?.Invoke(this, EventArgs.Empty);
                ActionComplete();
            }
        }
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        List<GridPosition> gridPositions = Pathfinding.Instance.FindPath(unit.GetGridPosition(), gridPosition, out int pathLength);

        currentPosIndex = 0;
        positions = new List<Vector3>();
        foreach (GridPosition position in gridPositions)
        {
            positions.Add(LevelGrid.Instance.GetWorldPosition(position));
        }
        positions.Add(LevelGrid.Instance.GetWorldPosition(gridPosition));

        OnStartMoving?.Invoke(this, EventArgs.Empty);

        ActionStart(onActionComplete);
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if(!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                    continue;

                int testDistance = Mathf.RoundToInt(Mathf.Sqrt(Mathf.Abs(x * x) + Mathf.Abs(z * z)));

                if (testDistance > maxMoveDistance)
                    continue;

                if (unitGridPosition == testGridPosition)
                    continue;

                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                    continue;

                if (!Pathfinding.Instance.IsWalkableGridPosition(testGridPosition))
                    continue;

                if(!Pathfinding.Instance.HasPath(unitGridPosition, testGridPosition))
                    continue;

                if (Pathfinding.Instance.GetPathLength(unitGridPosition, testGridPosition) > maxMoveDistance * 10)
                    continue;

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override string GetActionName()
    {
        return "Move";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        int targetCountAtGridPosition = unit.GetAction<ShootAction>().GetTargetCountAtPosition(gridPosition);
        return new EnemyAIAction
        {
            GridPosition = gridPosition,
            actionValue = this.actionValue * targetCountAtGridPosition
        };
    }
}
