using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    public static event EventHandler OnAnyActionStarted;
    public static event EventHandler OnAnyActionCompleted;

    [SerializeField] int actionPointCost = 1;
    [SerializeField] protected int actionValue = 0;

    protected Unit unit = null;
    protected bool isActive = false;
    protected Action onActionComplete = null;

    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();
    }

    public abstract string GetActionName();

    public abstract void TakeAction(GridPosition gridPosition, Action onActionComplete);

    public virtual bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositionList = GetValidActionGridPositionList();
        return validGridPositionList.Contains(gridPosition);
    }

    public abstract List<GridPosition> GetValidActionGridPositionList();

    public int GetActionPointCost()
    {
        return actionPointCost;
    }

    public Unit GetUnit()
    {
        return unit;
    }

    protected void ActionStart(Action onActionComplete)
    {
        isActive = true;
        this.onActionComplete = onActionComplete;

        OnAnyActionStarted?.Invoke(this, EventArgs.Empty);
    }

    protected void ActionComplete()
    {
        isActive = false;
        onActionComplete();

        OnAnyActionCompleted?.Invoke(this, EventArgs.Empty);
    }

    public EnemyAIAction GetBestEnemyAIAction()
    {
        List<EnemyAIAction> enemyAIActions = new List<EnemyAIAction>();

        List<GridPosition> validActionGridPositions = GetValidActionGridPositionList();

        foreach (GridPosition position in validActionGridPositions)
        {
            EnemyAIAction enemyAiAction = GetEnemyAIAction(position);
            enemyAIActions.Add(enemyAiAction);
        }

        if (enemyAIActions.Count <= 0)
            return null;

        enemyAIActions.Sort((EnemyAIAction a, EnemyAIAction b) => b.actionValue - a.actionValue);

        return enemyAIActions[0];
    }

    public abstract EnemyAIAction GetEnemyAIAction(GridPosition gridPosition);
}
