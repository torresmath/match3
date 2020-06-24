using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour, IManager
{
    #region Properties
    private GameController gameController;
    [SerializeField] protected Image pauseBackground;
    [SerializeField] protected Canvas gameOverCanvas;
    [SerializeField] protected Button pauseButton;
    #endregion

    #region MonoBehaviour
    void Start()
    {
        gameController = GetComponentInParent<GameController>();
        gameOverCanvas.enabled = false;
    }
    #endregion

    #region IManager
    public void Init()
    {        
        gameOverCanvas.enabled = false;
        pauseBackground.enabled = false;
        pauseButton.enabled = true;
    }
    #endregion

    #region Public
    public void ExecuteGameOver()
    {
        gameOverCanvas.enabled = true;
        pauseBackground.enabled = true;
        pauseButton.enabled = false;
        gameController.PauseSound();
        gameController.ChangeState(new GameOverState(gameController, gameController.stateMachine));
    }
    
    public void Restart()
    {
        gameController.ResetGame();
    }
    #endregion
}
