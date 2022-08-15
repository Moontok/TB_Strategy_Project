using System;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    enum State
    {
        WaitingForEnemyTurn,
        TakingTurn,
        Busy
    }

    State state = State.WaitingForEnemyTurn;
    float turn_time = 2f;
    float timer = 0f;

    void Start()
    {
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }

    void Update()
    {
        if (TurnSystem.Instance.IsPlayerTurn()) return;

        switch (state)
        {
            case State.WaitingForEnemyTurn:
                break;
            case State.TakingTurn:
                timer -= Time.deltaTime;

                if (timer <= 0)
                {
                    if (TryTakeEnemyAIAction(SetStateTakingTurn))
                        state = State.Busy;
                    else
                        TurnSystem.Instance.NextTurn();
                }
                break;
            case State.Busy:
                break;
        }
    }

    void SetStateTakingTurn()
    {
        timer = turn_time;
        state = State.TakingTurn;
    }

    void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if (!TurnSystem.Instance.IsPlayerTurn())
        {
            state = State.TakingTurn;
            timer = turn_time;
        }
    }

    bool TryTakeEnemyAIAction(Action onEnemyAIActionComplete)
    {
        foreach (Unit enemy in UnitManager.Instance.GetEnemyUnitList())
        {
            if (TryTakeEnemyAIAction(enemy, onEnemyAIActionComplete))
                return true;
        }
        return false;
    }

    bool TryTakeEnemyAIAction(Unit enemy, Action onEnemyAIActionComplete)
    {
        EnemyAIAction bestEnemyAIAction = null;
        BaseAction bestBaseAction = null;

        foreach (BaseAction baseAction in enemy.GetBaseActionArray())
        {
            if (!enemy.CanSpendActionPointsToTakeAction(baseAction))
                continue;

            if (bestEnemyAIAction == null)
            {
                bestEnemyAIAction = baseAction.GetBestEnemyAIAction();
                bestBaseAction = baseAction;
            }
            else
            {
                EnemyAIAction testEnemyAIAction = baseAction.GetBestEnemyAIAction();
                if (testEnemyAIAction != null && testEnemyAIAction.actionValue > bestEnemyAIAction.actionValue)
                {
                    bestEnemyAIAction = testEnemyAIAction;
                    bestBaseAction = baseAction;
                }
            }
        }

        if (bestEnemyAIAction != null && enemy.TrySpendActionPointsToTakeAction(bestBaseAction))
        {
            bestBaseAction.TakeAction(bestEnemyAIAction.GridPosition, onEnemyAIActionComplete);
            return true;
        }
        else
        {
            return false;
        }
    }
}
