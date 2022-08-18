using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PathfindingGridDebugObject : GridDebugObject
{
    [SerializeField] TextMeshPro gCostText = null;
    [SerializeField] TextMeshPro hCostText = null;
    [SerializeField] TextMeshPro fCostText = null;
    [SerializeField] SpriteRenderer isWalkableRenderer = null;

    PathNode pathNode = null;

    public override void SetGridObject(object gridObject)
    {
        base.SetGridObject(gridObject);
        pathNode = (PathNode)gridObject;
    }

    protected override void Update()
    {
        base.Update();
        gCostText.text = pathNode.GetGCost().ToString();
        hCostText.text = pathNode.GetHCost().ToString();
        fCostText.text = pathNode.GetFCost().ToString();
        isWalkableRenderer.color = pathNode.IsWalkable() ? Color.green : Color.red;
    }
}
