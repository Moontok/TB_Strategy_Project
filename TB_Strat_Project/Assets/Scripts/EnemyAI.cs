using System;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    float turn_time = 2f;

    float timer = 0f;

    void Start()
    {
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }

    void Update()
    {
        if (TurnSystem.Instance.IsPlayerTurn()) return;

        timer -= Time.deltaTime;

        if(timer <= 0)
        {
            TurnSystem.Instance.NextTurn();
        }
    }

    void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        timer = turn_time;
    }
}
