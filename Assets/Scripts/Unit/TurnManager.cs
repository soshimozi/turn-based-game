using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : SingletonMonoBehaviorBase<TurnManager>
{

    private int turnNumber = 1;
    public event EventHandler TurnChanged;

    private bool isPlayerTurn = true;


    public void NextTurn()
    {
        //turnNumber++;
        isPlayerTurn = !isPlayerTurn;

        // when it's the players turn we will increase the turn number
        if (isPlayerTurn) turnNumber++;

        OnTurnChanged();
    }

    public int GetTurnNumber()
    {
        return turnNumber;
    }

    protected virtual void OnTurnChanged()
    {
        TurnChanged?.Invoke(this, EventArgs.Empty);
    }

    public bool IsPlayerTurn()
    {
        return isPlayerTurn;
    }
}
