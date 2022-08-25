using System;
using UnityEngine;

public class PathfindingUpdater : MonoBehaviour
{
    void Start()
    {
        DestructableObject.OnAnyDestroyed += DestructableObject_OnAnyDestroyed;
    }

    void DestructableObject_OnAnyDestroyed(object sender, EventArgs e)
    {
        DestructableObject destructable = sender as DestructableObject;
        Pathfinding.Instance.SetIsWalkableGridPosition(destructable.GetGridPosition(), true);
    }
}
