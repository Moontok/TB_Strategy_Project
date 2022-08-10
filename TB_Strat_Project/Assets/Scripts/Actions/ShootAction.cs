using System;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : BaseAction
{
    public event EventHandler OnShoot;

    [SerializeField] int maxRange = 7;
    [SerializeField] float aimTime = 1f;
    [SerializeField] float shootTime = 0.1f;
    [SerializeField] float coolOffTime = 0.5f;
    [SerializeField] Transform projectilePrefab = null;
    [SerializeField] Transform projectileSpawnPoint = null;
    
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

                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                    continue;

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);

                if (targetUnit.IsEnemy() == unit.IsEnemy())
                    continue;

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        ActionStart(onActionComplete);

        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        
        state = State.Aiming;
        stateTimer = aimTime;

        canShootBullet = true;
    }

    public Transform GetProjectilePrefab()
    {
        return projectilePrefab;
    }

    public Transform GetProjectileSpawnPoint()
    {
        return projectileSpawnPoint;
    }
}
