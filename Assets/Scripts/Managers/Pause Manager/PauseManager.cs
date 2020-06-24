using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour, IManager
{
    #region Properties
    [SerializeField] protected TextMeshProUGUI pauseText;
    [SerializeField] protected Image pauseBackground;

    private GameController gameController;
    #endregion

    #region MonoBehaviour
    private void Start()
    {
        gameController = GetComponentInParent<GameController>();
        //pauseBackground.enabled = false;
    }
    #endregion

    #region IManager
    public void Init()
    {
        pauseBackground.enabled = false;
    }
    #endregion

    #region Public
    public void Pause()
    {
        System.Type stateType = gameController.GetCurrentState().GetType();
        if (stateType == typeof(GameOverState))
            return;

        if (stateType != typeof(PauseState))
        {
            pauseText.text = "RESUME";
            gameController.ChangeState(new PauseState(gameController, gameController.stateMachine));
            pauseBackground.enabled = true;
            Time.timeScale = 0;
            gameController.PauseSound();
            return;
        }
        
        gameController.ChangeState(new ValidateState(gameController, gameController.stateMachine, true));
        pauseText.text = "PAUSE";
        pauseBackground.enabled = false;
        Time.timeScale = 1;
        gameController.ResumeSound();
    }
    #endregion
}
