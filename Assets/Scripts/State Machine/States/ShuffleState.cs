using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShuffleState : State
{
    #region Constructor
    public ShuffleState(GameController gameController, StateMachine stateMachine) : base(gameController, stateMachine) { }
    #endregion

    #region State
    public override void Init()
    {
        base.Init();
        gameController.Shuffle();
    }
    #endregion
}
