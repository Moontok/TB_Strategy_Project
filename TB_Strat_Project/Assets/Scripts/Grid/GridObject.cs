using System.Collections.Generic;

public class GridObject
{
    GridSystem gridSystem = null;
    GridPosition gridPosition = new GridPosition(); 
    List<Unit> unitList = null;
    
    public GridObject(GridSystem gridSystem, GridPosition gridPosition)
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
}
