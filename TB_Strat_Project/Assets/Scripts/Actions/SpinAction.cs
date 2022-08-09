using System;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction
{
    float spinAmount = 360f;
    float totalSpinAmount = 0f;

    void Update()
    {
        if (!isActive) return;

        float spinAddAmount = spinAmount * Time.deltaTime;
        transform.eulerAngles += new Vector3(0, spinAddAmount, 0);

        totalSpinAmount += spinAddAmount;

        if (totalSpinAmount >= spinAmount)
        {
            ActionComplete();
        }
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        ActionStart(onActionComplete);
        totalSpinAmount = 0f;
    }

    public override string GetActionName()
    {
        return "Spin";
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();

        return new List<GridPosition> { unitGridPosition };
    }
}
