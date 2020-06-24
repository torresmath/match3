using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerManager : MonoBehaviour , IManager
{
    #region Properties
    private GameController gameController;
    private bool timeOver;

    [SerializeField] protected TextMeshProUGUI timerText;
    public float maxTime;
    private float timer;
    #endregion

    #region MonoBehaviour
    void Start()
    {
        gameController = GetComponentInParent<GameController>();
        Init();
    }
    void FixedUpdate()
    {
        if (timer > 0f)
            timer -= Time.deltaTime;
        timerText.text = Mathf.Round(timer).ToString();

        if (Mathf.Round(timer) == 0f && !timeOver)
        {
            timeOver = true;
            gameController.GameOver();
        }
    }
    #endregion

    #region IManager
    public void Init()
    {
        ResetTime();
        timeOver = false;
    }
    #endregion

    #region Public
    public void ResetTime()
    {
        timer = maxTime;
    }
    #endregion

}
