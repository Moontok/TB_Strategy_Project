using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurnSystemUI : MonoBehaviour
{
    [SerializeField] Button endTurnButton = null;
    [SerializeField] TextMeshProUGUI turnNumberText = null;

    void Start()
    {
        endTurnButton.onClick.AddListener(() => { 
            TurnSystem.Instance.NextTurn(); 
        });

        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;

        UpdateTurnText();
    }

    void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        UpdateTurnText();
    }

    void UpdateTurnText()
    {
        turnNumberText.text = $"Turn {TurnSystem.Instance.GetTurnNumber()}";
    }
}
