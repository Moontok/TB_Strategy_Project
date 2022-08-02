using System;
using UnityEngine;

public class UnitSelectedVisual : MonoBehaviour
{
    [SerializeField] Unit unit = null;

    MeshRenderer meshRender = null;

    void Awake()
    {
        meshRender = GetComponent<MeshRenderer>();
    }

    void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;

        UpdateVisual();
    }

    void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs empty)
    {
        UpdateVisual();
    }

    void UpdateVisual()
    {
        if (UnitActionSystem.Instance.GetSelectedUnit() == unit)
            meshRender.enabled = true;
        else
            meshRender.enabled = false;
    }
}
