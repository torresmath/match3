using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReorderState : State
{
    #region Constructor
    public ReorderState(GameController gameController, StateMachine stateMachine) : base(gameController, stateMachine) { }
    #endregion

    #region State
    public override void Init()
    {
        base.Init();
        gameController.ResetContent();
        gameController.ReorderGem();
    }
    #endregion
}
