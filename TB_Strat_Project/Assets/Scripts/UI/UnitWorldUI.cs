using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitWorldUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI actionPointsText = null;
    [SerializeField] Unit unit = null;
    [SerializeField] Image healthBar = null;
    [SerializeField] HealthSystem healthSystem = null;

    void Start()
    {
        Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;
        healthSystem.OnDamaged += HealthSystem_OnDamaged;

        UpdateActionPointsText();
        UpdateHealthbar();
    }

    void UpdateActionPointsText()
    {
        actionPointsText.text = unit.GetActionPoints().ToString();
    }

    void UpdateHealthbar()
    {
        healthBar.fillAmount = healthSystem.GetHealthNormalized();
    }

    void Unit_OnAnyActionPointsChanged(object sender, EventArgs e)
    {
        UpdateActionPointsText();
    }

    void HealthSystem_OnDamaged(object sender, EventArgs e)
    {
        UpdateHealthbar();
    }
}
