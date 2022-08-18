using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    GridPosition gridPosition = new GridPosition();
    int gCost = 0;
    int hCost = 0;
    int fCost = 0;
    PathNode previousNode = null;
    bool isWalkable = true;

    public PathNode(GridPosition gridPosition)
    {
        this.gridPosition = gridPosition;
    }

    public override string ToString()
    {
        return gridPosition.ToString();
    }

    public int GetGCost()
    {
        return gCost;
    }

    public int GetHCost()
    {
        return hCost;
    }

    public int GetFCost()
    {
        return fCost;
    }

    public void SetGCost(int cost)
    {
        gCost = cost;
    }

    public void SetHCost(int cost)
    {
        hCost = cost;
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    public void ResetPreviousPathNode()
    {
        previousNode = null;
    }

    public void SetPreviousPathNode(PathNode pathNode)
    {
        previousNode = pathNode;
    }

    public PathNode GetPreviousPathNode()
    {
        return previousNode;
    }

    public GridPosition GetGridPos()
    {
        return gridPosition;
    }

    public bool IsWalkable()
    {
        return isWalkable;
    }

    public void SetIsWalkable(bool isWalkable)
    {
        this.isWalkable = isWalkable;
    }
}
