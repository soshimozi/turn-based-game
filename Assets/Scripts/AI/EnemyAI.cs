using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{


    private float timer;

    private enum State
    {
        WaitingForEnemyTurn,
        TakingTurn,
        Busy
    }

    private State state;


    private void Awake()
    {
        state = State.WaitingForEnemyTurn;
    }

    private void Start()
    {
        TurnManager.Instance.TurnChanged += TurnSystem_OnTurnChanged;
    }

    private void Update()
    {
        if (TurnManager.Instance.IsPlayerTurn())
        {
            return;
        }

        switch (state)
        {
            case State.WaitingForEnemyTurn:
                break;
            case State.TakingTurn:
                timer -= Time.deltaTime;
                if (timer <= 0f)
                {
                    state = State.Busy;
                    TakeEnemyAIAction(() =>
                    {
                        timer = 0.5f;
                        state = State.TakingTurn;
                    });

                }
                break;
            case State.Busy:
                break;
        }

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            TurnManager.Instance.NextTurn();
        }
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if (!TurnManager.Instance.IsPlayerTurn())
        {
            state = State.TakingTurn;
            timer = 2f;
        }
    }

    private void TakeEnemyAIAction(Action onEnemyActionComplete)
    {

    }


}