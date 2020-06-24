using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseState : State
{
    #region Constructor
    public PauseState(GameController gameController, StateMachine stateMachine) : base(gameController, stateMachine) { }
    #endregion

    #region State
    public override void Init()
    {
        base.Init();
        gameController.ResetContent();
    }
    #endregion
}
