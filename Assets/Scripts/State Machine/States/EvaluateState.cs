using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvaluateState : State
{
    #region Constructor
    public EvaluateState(GameController gameController, StateMachine stateMachine, bool validate) : base(gameController, stateMachine)
    {
        
    }
    #endregion

    #region State
    public override void Init()
    {
        base.Init();
    }
    #endregion
}
