using System.Collections.Generic;

public class GridObject
{
    GridSystem<GridObject> gridSystem = null;
    GridPosition gridPosition = new GridPosition(); 
    List<Unit> unitList = null;
    Door door = null;
    DestructableObject destructable = null;

    public GridObject(GridSystem<GridObject> gridSystem, GridPosition gridPosition)
    {
        this.gridSystem = gridSystem;
        this.gridPosition = gridPosition;
        unitList = new List<Unit>();
    }

    public override string ToString()
    {
        string unitString = "";
        foreach (Unit unit in unitList)
        {
            unitString = $"{unitString}{unit}\n";
        }
        return $"{gridPosition}\n{unitString}";
    }

    public void AddUnit(Unit unit)
    {
        unitList.Add(unit);
    }

    public void RemoveUnit(Unit unit)
    {
        unitList.Remove(unit);
    }

    public List<Unit> GetUnitList()
    {
        return unitList;
    }

    public bool HasAnyUnit()
    {
        return unitList.Count > 0;
    }

    public Unit GetUnit()
    {
        if (HasAnyUnit())
            return unitList[0];
        else
            return null;
    }

    public Door GetDoor()
    {
        return door;
    }

    public void SetDoor(Door door)
    {
        this.door = door;
    }

    public DestructableObject GetDestructable()
    {
        return destructable;
    }

    public void SetDestructable(DestructableObject destructable)
    {
        this.destructable = destructable;
    }
}
