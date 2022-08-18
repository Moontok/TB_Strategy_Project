using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public static Pathfinding Instance { get; private set; }

    const int MOVE_STRAIGHT_COST = 10;
    const int MOVE_DIAGONAL_COST = 14;

    [SerializeField] Transform gridDebugObjectPrefab = null;
    [SerializeField] LayerMask obstacleLayerMask = new LayerMask();

    int width = 10;
    int height = 10;
    float cellSize = 2f;
    GridSystem<PathNode> gridSystem = null;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError($"There is more than one Pathfinding System! {transform} - {Instance}");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void Setup(int width, int height, float cellSize)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridSystem = new GridSystem<PathNode>(width, height, cellSize,
            (GridSystem<PathNode> gameObject, GridPosition gridPosition) => new PathNode(gridPosition)
        );

        //gridSystem.CreateDebugObjects(gridDebugObjectPrefab);

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GridPosition gridPos = new GridPosition(x, z);
                Vector3 worldPos = LevelGrid.Instance.GetWorldPosition(gridPos);
                if (Physics.Raycast(worldPos + Vector3.down * 5f, Vector3.up, 10f, obstacleLayerMask))
                {
                    GetNode(x, z).SetIsWalkable(false);
                }
            }
        }
    }

    public List<GridPosition> FindPath(GridPosition startPos, GridPosition endPos, out int pathLength)
    {
        List<PathNode> openList = new List<PathNode>();
        List<PathNode> closedList = new List<PathNode>();

        PathNode startNode = gridSystem.GetGridObject(startPos);
        PathNode endNode = gridSystem.GetGridObject(endPos);
        openList.Add(startNode);

        for (int x = 0; x < gridSystem.GetWidth(); x++)
        {
            for (int z = 0; z < gridSystem.GetHeight(); z++)
            {
                GridPosition gridPos = new GridPosition(x, z);
                PathNode pathNode = gridSystem.GetGridObject(gridPos);

                pathNode.SetGCost(int.MaxValue);
                pathNode.SetHCost(0);
                pathNode.CalculateFCost();
                pathNode.ResetPreviousPathNode();
            }
        }

        startNode.SetGCost(0);
        startNode.SetHCost(CalculateDistance(startPos, endPos));
        startNode.CalculateFCost();

        while (openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostPathNode(openList);

            if (currentNode == endNode)
            {
                pathLength = endNode.GetFCost();
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);
            closedList.Add(currentNode);

            foreach (PathNode neighbor in GetNeighbors(currentNode))
            {
                if (closedList.Contains(neighbor))
                    continue;

                if (!neighbor.IsWalkable())
                {
                    closedList.Add(neighbor);
                    continue;
                }

                int tentativeGCost = currentNode.GetGCost() + CalculateDistance(currentNode.GetGridPos(), neighbor.GetGridPos());
                if (tentativeGCost < neighbor.GetGCost())
                {
                    neighbor.SetPreviousPathNode(currentNode);
                    neighbor.SetGCost(tentativeGCost);
                    neighbor.SetHCost(CalculateDistance(neighbor.GetGridPos(), endPos));
                    neighbor.CalculateFCost();

                    if (!openList.Contains(neighbor))
                        openList.Add(neighbor);
                }
            }
        }
        pathLength = 0;
        return null;
    }

    public int CalculateDistance(GridPosition gridPosA, GridPosition gridPosB)
    {
        GridPosition gridPosDistance = gridPosA - gridPosB;
        int xDistance = Mathf.Abs(gridPosDistance.x);
        int zDistance = Mathf.Abs(gridPosDistance.z);
        int remaining = Mathf.Abs(xDistance - zDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, zDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    PathNode GetLowestFCostPathNode(List<PathNode> list)
    {
        PathNode lowestFCostPathNode = list[0];

        foreach (PathNode node in list)
        {
            if(node.GetFCost() < lowestFCostPathNode.GetFCost())
            {
                lowestFCostPathNode = node;
            }
        }

        return lowestFCostPathNode;
    }

    PathNode GetNode(int x, int z)
    {
        return gridSystem.GetGridObject(new GridPosition(x, z));
    }

    List<PathNode> GetNeighbors(PathNode currentNode)
    {
        List<PathNode> neighbors = new List<PathNode>();

        GridPosition gridPos = currentNode.GetGridPos();

        if (gridPos.x - 1 >= 0)
        {
            neighbors.Add(GetNode(gridPos.x - 1, gridPos.z));
            if (gridPos.z - 1 >= 0)
                neighbors.Add(GetNode(gridPos.x - 1, gridPos.z - 1));
            if (gridPos.z + 1 < gridSystem.GetHeight())
                neighbors.Add(GetNode(gridPos.x - 1, gridPos.z + 1));
        }
        if (gridPos.x + 1 < gridSystem.GetWidth())
        {
            neighbors.Add(GetNode(gridPos.x + 1, gridPos.z));
            if (gridPos.z - 1 >= 0)
                neighbors.Add(GetNode(gridPos.x + 1, gridPos.z - 1));
            if (gridPos.z + 1 < gridSystem.GetHeight())
                neighbors.Add(GetNode(gridPos.x + 1, gridPos.z + 1));
        }
        if (gridPos.z - 1 >= 0)
            neighbors.Add(GetNode(gridPos.x, gridPos.z - 1));
        if (gridPos.z + 1 < gridSystem.GetHeight())
            neighbors.Add(GetNode(gridPos.x, gridPos.z + 1));

        return neighbors;
    }

    List<GridPosition> CalculatePath(PathNode endNode)
    {
        List<PathNode> pathNodes = new List<PathNode>();

        pathNodes.Add(endNode);

        PathNode currentNode = endNode;

        while (currentNode.GetPreviousPathNode() != null)
        {
            pathNodes.Add(currentNode.GetPreviousPathNode());
            currentNode = currentNode.GetPreviousPathNode();
        }

        pathNodes.Reverse();

        List<GridPosition> gridPosList = new List<GridPosition>();

        foreach (PathNode pathNode in pathNodes)
        {
            gridPosList.Add(pathNode.GetGridPos());
        }

        return gridPosList;
    }

    public bool IsWalkableGridPosition(GridPosition position)
    {
        return gridSystem.GetGridObject(position).IsWalkable();
    }

    public bool HasPath(GridPosition start, GridPosition end)
    {
        return FindPath(start, end, out int pathLength) != null;
    }

    public int GetPathLength(GridPosition start, GridPosition end)
    {
        FindPath(start, end, out int pathLength);
        return pathLength;
    }
}
