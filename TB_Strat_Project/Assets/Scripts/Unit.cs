using System;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public static event EventHandler OnAnyActionPointsChanged;

    [SerializeField] int actionPointsMax = 2;

    int currentActionPoints = 0;
    GridPosition gridPosition = new GridPosition();
    MoveAction moveAction = new MoveAction();
    SpinAction spinAction = new SpinAction();
    BaseAction[] baseActionArray = null;

    void Awake()
    {
        moveAction = GetComponent<MoveAction>();
        spinAction = GetComponent<SpinAction>();
        baseActionArray = GetComponents<BaseAction>();
    }

    void Start()
    {
        currentActionPoints = actionPointsMax;
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);

        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
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

    public BaseAction[] GetBaseActionArray()
    {
        return baseActionArray;
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

    public int GetActionPoints()
    {
        return currentActionPoints;
    }

    void SpendActionPoints(int amount)
    {
        currentActionPoints -= amount;

        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }

    void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        currentActionPoints = actionPointsMax;

        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }
}
