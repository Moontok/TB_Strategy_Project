using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurnSystemUI : MonoBehaviour
{
    [SerializeField] Button endTurnButton = null;
    [SerializeField] TextMeshProUGUI turnNumberText = null;
    [SerializeField] GameObject enemyTurnVisualGameObject = null;

    void Start()
    {
        endTurnButton.onClick.AddListener(() => { 
            TurnSystem.Instance.NextTurn(); 
        });

        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;

        UpdateTurnText();
        UpdateEnemyTurnVisual();
        UpdateEndTurnButtonVisibility();
    }

    void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        UpdateTurnText();
        UpdateEnemyTurnVisual();
        UpdateEndTurnButtonVisibility();
    }

    void UpdateTurnText()
    {
        turnNumberText.text = $"Turn {TurnSystem.Instance.GetTurnNumber()}";
    }

    void UpdateEnemyTurnVisual()
    {
        enemyTurnVisualGameObject.SetActive(!TurnSystem.Instance.IsPlayerTurn());
    }

    void UpdateEndTurnButtonVisibility()
    {
        endTurnButton.gameObject.SetActive(TurnSystem.Instance.IsPlayerTurn());
    }
}
