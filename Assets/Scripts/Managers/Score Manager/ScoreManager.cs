using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour, IManager
{
    #region Properties
    GameController gameController;
    public int score;
    public int level;
    public int baseGoal;
    private int goal;

    [SerializeField] protected TextMeshProUGUI multiplierText;
    [SerializeField] protected TextMeshProUGUI scoreText;
    [SerializeField] protected TextMeshProUGUI goalText;
    #endregion

    #region MonoBehaviour
    private void Start()
    {
        gameController = GetComponentInParent<GameController>();
        Init();
        UpdateView();
    }
    #endregion

    #region IManager
    public void Init()
    {
        score = 0;
        level = 1;
        SetGoal();
    }
    #endregion

    #region Public
    public void AddScore(List<Gem> gems)
    {
        score += (gems.Select(gem => gem.Points).ToList().Sum() + level) * gems.Count;
        
        if (score >= goal)
        {
            level = level + 1;
            gameController.LevelClearedEvent();
            SetGoal();
        }
        UpdateView();
    }
    #endregion

    #region Private
    void UpdateView()
    {
        multiplierText.text = level.ToString();
        scoreText.text = score.ToString();
        goalText.text = "GOAL: " + goal.ToString();
    }

    void SetGoal()
    {
        var nextGoal = goal == 0 ? baseGoal : goal;
        goal = Mathf.RoundToInt(nextGoal * 1.5f);
    }
    #endregion
}
