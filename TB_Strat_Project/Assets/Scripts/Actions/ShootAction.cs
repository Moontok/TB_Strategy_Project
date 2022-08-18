using System;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : BaseAction
{
    public static event EventHandler OnAnyShoot;
    public event EventHandler OnShoot;

    [SerializeField] int maxRange = 7;
    [SerializeField] float aimTime = 1f;
    [SerializeField] float shootTime = 0.1f;
    [SerializeField] float coolOffTime = 0.5f;
    [SerializeField] float weaponHeight = 1.7f;
    [SerializeField] Transform projectilePrefab = null;
    [SerializeField] Transform projectileSpawnPoint = null;
    [SerializeField] LayerMask obstacleLayerMask = new LayerMask();

    enum State
    {
        Done,
        Aiming,
        Shooting,
        Cooloff
    }

    State state = State.Done;
    float stateTimer = 0f;
    Unit targetUnit = null;
    bool canShootBullet = false;
    float rotationSpeed = 10f;

    void Update()
    {
        if (!isActive) return;

        stateTimer -= Time.deltaTime;

        switch (state)
        {
            case State.Aiming:
                Vector3 aimDirection = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
                transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * rotationSpeed);

                break;
            case State.Shooting:
                if(canShootBullet)
                {
                    Shoot();
                    canShootBullet = false;
                }
                break;
            case State.Cooloff:
                break;
        }

        if (stateTimer <= 0f)
        {
            NextState();
        }
    }

    void Shoot()
    {
        OnAnyShoot?.Invoke(this, EventArgs.Empty);
        OnShoot?.Invoke(this, EventArgs.Empty);

        Transform projectileObject = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
        Vector3 targetPosition = targetUnit.GetWorldPosition();
        targetPosition.y = projectileSpawnPoint.position.y;

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.SetTarget(targetPosition);
        targetUnit.Damage(projectile.GetDamageAmount());
    }

    void NextState()
    {
        switch (state)
        {
            case State.Aiming:
                state = State.Shooting;
                stateTimer = shootTime;
                break;
            case State.Shooting:
                state = State.Cooloff;
                stateTimer = coolOffTime;
                break;
            case State.Cooloff:
                state = State.Done;
                ActionComplete();
                break;
        }
    }

    public override string GetActionName()
    {
        return "Shoot";
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();

        return GetValidActionGridPositionList(unitGridPosition);
    }

    public List<GridPosition> GetValidActionGridPositionList(GridPosition unitGridPosition)
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

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

                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                    continue;

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);

                if (targetUnit.IsEnemy() == unit.IsEnemy())
                    continue;

                Vector3 unitWorldPosition = LevelGrid.Instance.GetWorldPosition(unitGridPosition);
                Vector3 aimPosition = unitWorldPosition + Vector3.up * weaponHeight;
                Vector3 aimDirection = (targetUnit.GetWorldPosition() - unitWorldPosition).normalized;
                float distance = Vector3.Distance(unitWorldPosition, targetUnit.GetWorldPosition());

                if (Physics.Raycast(aimPosition, aimDirection, distance, obstacleLayerMask))
                    continue;

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
        
        state = State.Aiming;
        stateTimer = aimTime;

        canShootBullet = true;

        ActionStart(onActionComplete);
    }

    public Transform GetProjectilePrefab()
    {
        return projectilePrefab;
    }

    public Transform GetProjectileSpawnPoint()
    {
        return projectileSpawnPoint;
    }

    public Unit GetTargetUnit()
    {
        return targetUnit;
    }

    public int GetMaxRange()
    {
        return maxRange;
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
        
        return new EnemyAIAction
        {
            GridPosition = gridPosition,
            actionValue = this.actionValue + Mathf.RoundToInt((1 - targetUnit.GetHealthNormalized()) * this.actionValue)
    };
    }

    public int GetTargetCountAtPosition(GridPosition gridPosition)
    {
        return GetValidActionGridPositionList(gridPosition).Count;
    }
}
