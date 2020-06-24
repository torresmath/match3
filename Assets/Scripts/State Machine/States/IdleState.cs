using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    #region Constructor
    public IdleState(GameController gameController, StateMachine stateMachine) : base(gameController, stateMachine) { }
    #endregion

    #region State
    public override void Init()
    {
        base.Init();
        gameController.ResetContent();
        gameController.Validate();
        gameController.Evaluate();
    }
    #endregion
}
