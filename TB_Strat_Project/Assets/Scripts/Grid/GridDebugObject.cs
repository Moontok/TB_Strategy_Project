using UnityEngine;
using TMPro;

public class GridDebugObject : MonoBehaviour
{
    [SerializeField] TextMeshPro textMeshPro = null;

    GridObject gridObject = null;

    public void SetGridObject(GridObject gridObject)
    {
        this.gridObject = gridObject;
    }

    void Update()
    {
        textMeshPro.text = gridObject.ToString();
    }
}
