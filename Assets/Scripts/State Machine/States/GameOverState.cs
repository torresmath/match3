using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverState : State
{
    #region Constructor
    public GameOverState(GameController gameController, StateMachine stateMachine) : base(gameController, stateMachine) { }
    #endregion

    #region State
    public override void Init()
    {
        base.Init();
    }
    #endregion
}
