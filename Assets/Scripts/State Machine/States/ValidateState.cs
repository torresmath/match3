using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValidateState : State
{
    #region Constructor
    private bool validate;
    public ValidateState(GameController gameController, StateMachine stateMachine, bool validate) : base(gameController, stateMachine) {
        this.validate = validate;
    }
    #endregion

    #region State
    public override void Init()
    {
        base.Init();
        if (validate)
            gameController.ValidateMatchThree();
    }
    #endregion
}
