using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class GameState : MonoBehaviour
{
    public GameState previousState;
    public GameState nextState;
    public UnityEvent onStateEnableEvent;
    public UnityEvent onStateExitEvent;
    /*
    protected void OnEnable()
    {
        ChangeState(this);
    }
    */
    public void OnStateEnter()
    {
        gameObject.SetActive(true);
        onStateEnableEvent.Invoke();
    }

    public void OnStateExit()
    {
        gameObject.SetActive(false);
        onStateExitEvent.Invoke();
    }

    public void ChangeState(GameState newState)
    {
        GameStateManager.instance.Change(newState);
    }
}
