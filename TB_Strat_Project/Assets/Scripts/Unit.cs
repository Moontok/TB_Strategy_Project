using System;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public static event EventHandler OnAnyActionPointsChanged;

    [SerializeField] bool isEnemy = false;
    [SerializeField] int actionPointsMax = 2;

    int currentActionPoints = 0;
    HealthSystem healthSystem = null;
    GridPosition gridPosition = new GridPosition();
    MoveAction moveAction = null;
    SpinAction spinAction = null;
    BaseAction[] baseActionArray = null;

    void Awake()
    {
        currentActionPoints = actionPointsMax;

        healthSystem = GetComponent<HealthSystem>();
        moveAction = GetComponent<MoveAction>();
        spinAction = GetComponent<SpinAction>();
        baseActionArray = GetComponents<BaseAction>();
    }

    void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);

        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;

        healthSystem.OnDead += HealthSystem_OnDead;
    }

    void Update()
    {
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if(newGridPosition != gridPosition)
        {
            LevelGrid.Instance.UnitMovedGridPosition(this, gridPosition, newGridPosition);
            gridPosition = newGridPosition;
        }
    }

    public MoveAction GetMoveAction()
    {
        return moveAction;
    }

    public SpinAction GetSpinAction()
    {
        return spinAction;
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    public Vector3 GetWorldPosition()
    {
        return transform.position;
    }

    public BaseAction[] GetBaseActionArray()
    {
        return baseActionArray;
    }

    public int GetActionPoints()
    {
        return currentActionPoints;
    }

    public bool TrySpendActionPointsToTakeAction(BaseAction baseAction)
    {
        if(CanSpendActionPointsToTakeAction(baseAction))
        {
            SpendActionPoints(baseAction.GetActionPointCost());
            return true;
        }

        return false;
    }

    public bool CanSpendActionPointsToTakeAction(BaseAction baseAction)
    {
        return currentActionPoints >= baseAction.GetActionPointCost();
    }

    void SpendActionPoints(int amount)
    {
        currentActionPoints -= amount;

        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }

    void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if ((IsEnemy() && !TurnSystem.Instance.IsPlayerTurn()) ||
            (!IsEnemy() && TurnSystem.Instance.IsPlayerTurn()))
        {
            currentActionPoints = actionPointsMax;

            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    void HealthSystem_OnDead(object sender, EventArgs e)
    {
        LevelGrid.Instance.RemoveUnitAtGridPosition(gridPosition, this);
        Destroy(gameObject);
    }

    public bool IsEnemy()
    {
        return isEnemy;
    }
    
    public void Damage(int damageAmount)
    {
        healthSystem.Damage(damageAmount);
    }
}
