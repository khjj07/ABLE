using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : Singleton<GameStateManager>
{
    public GameState _currentState;
    public GameState currentState
    {
        get
        {
            return instance._currentState;
        }
        set
        {
            instance._currentState = value;
        }
    }

    public void Change(GameState newState)
    {
        if (currentState != null)
        {
            currentState.OnStateExit();
        }

        currentState = newState;

        if (currentState != null)
        {
            currentState.OnStateEnter();
        }
    }
    public void Next()
    {
        if (currentState == null)
        {
            return;
        }

        if (currentState.nextState == null)
        {
            return;
        }

        Change(currentState.nextState);
    }
    public void Previous()
    {
        if (currentState == null)
        {
            return;
        }

        if (currentState.previousState == null)
        {
            return;
        }

        Change(currentState.previousState);
    }
}
