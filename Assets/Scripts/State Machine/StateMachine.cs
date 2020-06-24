using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    #region Properties
    protected State currentState;
    #endregion

    #region Public
    public void Initialize(State startingState)
    {
        currentState = startingState;
        currentState.Init();
    }

    public void ChangeState(State newState)
    {
        if (newState == currentState)
            return;

        currentState = newState;
        currentState.Init();
    }

    public State GetCurrentState()
    {
        return currentState;
    }
    #endregion
}
