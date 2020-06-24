using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    #region Properties
    [Range(0, 6)]    
    [SerializeField] protected int x;
    public int X { get { return x; } }
    [Range(0, 6)]
    [SerializeField] protected int y;
    public int Y { get { return y; } }

    [Range(100, 100000)]
    [SerializeField] protected int baseGoal;
    [Range(0, 120)]
    [SerializeField] protected float maxTime;
    [SerializeField] protected GameObject gridHolder;
    [SerializeField] protected GameObject cellPrefab;
    [SerializeField] protected TextMeshProUGUI warningText;
    [SerializeField] protected Color32 selectedColor;

    private List<GameObject> gemPrefabs;

    public Cell cell { get { return cellPrefab.GetComponent<Cell>(); } private set { } }
    public StateMachine stateMachine;

    [SerializeField] protected EventListenerManager listener;
    [SerializeField] protected EventPublisherManager publisher;

    private GameOverManager gameOverManager;
    private GridManager gridManager;
    private PauseManager pauseManager;
    private ScoreManager scoreManager;
    private SoundManager soundManager;
    private TimerManager timerManager;

    #endregion

    #region MonoBehaviour
    void Awake()
    {
        publisher = new EventPublisherManager();
        listener = new EventListenerManager(this, publisher);

        warningText.enabled = false;

        gameOverManager = GetComponentInChildren<GameOverManager>();
        gridManager = GetComponentInChildren<GridManager>();
        pauseManager = GetComponentInChildren<PauseManager>();
        scoreManager = GetComponentInChildren<ScoreManager>();
        soundManager = GetComponentInChildren<SoundManager>();
        timerManager = GetComponentInChildren<TimerManager>();
        gridManager.SelectedColor = selectedColor;
        timerManager.maxTime = maxTime;
        scoreManager.baseGoal = baseGoal;
        InitializeManagers();
        StartCoroutine(Initialize());
    }
    #endregion
    
    #region Public
    public void ResetGame()
    {
        RespawnAllGem();
        InitializeManagers();
        StartCoroutine(ResetState());
        IEnumerator ResetState()
        {
            yield return new WaitForSeconds(1.0f);
            ChangeState(new ValidateState(this, stateMachine, true));
        }
    }
    public void RespawnAllGem()
    {
        foreach(Cell cellItem in gridManager.cells)
        {
            SpawnGem(cellItem);
        }
        stateMachine.Initialize(new ValidateState(this, stateMachine, true));
    }
    public void SpawnGem(Cell cell)
    {
        if (cell.Content != null)
            Destroy(cell.Content.gameObject);

        int i = UnityEngine.Random.Range(0, gemPrefabs.Count);
        GameObject gem = Instantiate(gemPrefabs[i], cell.gameObject.transform);
        cell.Content = gem;
    }
    #endregion

    #region Private
    void InitializeManagers()
    {
        List<IManager> IManagers = GetComponentsInChildren<IManager>().ToList();
        foreach (IManager manager in IManagers)
            manager.Init();
    }

    IEnumerator Initialize()
    {
        gemPrefabs = Resources.LoadAll<GameObject>("Prefabs/Gems").ToList();

        for (int x = 0; x < this.x; x++)
        {
            for (int y = 0; y < this.y; y++)
            {
                GameObject cellObject = Instantiate(cellPrefab, gridHolder.transform);
                cellObject.name = "Cell " + x + " " + y;

                Cell cell = cellObject.GetComponent<Cell>();
                cell.X = x;
                cell.Y = y;
                cell.GameController = this;
                cell.Neighbours = new List<Cell>();
                SpawnGem(cell);
                gridManager.cells.Add(cell);
            }
        }

        gridManager.cells.ForEach(cell => cell.Neighbours = gridManager.GetNeighbours(cell));
        yield return null;
        stateMachine.Initialize(new ValidateState(this, stateMachine, true));
    }
    #endregion

    #region Event Publishers
    public void SelectCellEvent(Cell cell)
    {
        if (cell != null)
            publisher.OnSelect(cell);
    }

    public void SwapCellEvent(Cell cell)
    {
        if (cell != null)
            publisher.OnSwap(cell);
    }

    void ValidateMatchThreeEvent(List<Cell> cells)
    {
        if (cells != null)
            publisher.OnValidateMatchThree(cells);
    }

    public void MatchThreeFailedEvent(Cell swappedCell)
    {
        if (swappedCell != null)
            publisher.OnMatchThreeFailed(swappedCell);
    }

    public void ReorderGemEvent()
    {
        publisher.OnReorderGem();
    }

    public void LevelClearedEvent()
    {
        publisher.OnLevelCleared();
    }

    void GameOverEvent()
    {
        publisher.OnGameOver();
    }

    void ReorderedGemEvent(bool keepReordering)
    {
        publisher.OnReorderedGem(keepReordering);
    }
    #endregion

    #region State Machine
    public State GetCurrentState()
    {
        return stateMachine.GetCurrentState();
    }

    public void ChangeState(State newState)
    {
        stateMachine.ChangeState(newState);
    }
    #endregion

    #region Game Over Manager

    public void GameOver()
    {
        GameOverEvent();
    }
    public void ExecuteGameOver()
    {
        gameOverManager.ExecuteGameOver();
    }
    #endregion

    #region Grid Manager
    public void SetSelectedCell(Cell cell)
    {
        if (cell != null)
        {
            gridManager.SetSelectedCell(cell);
            soundManager.Select();
        }
            
    }
    public bool CanSwap(Cell cell)
    {
        return gridManager.CanSwap(cell);
    }

    public void SwapCell(Cell cell)
    {
        gridManager.SwapCell(cell);
        ChangeState(new ValidateState(this, stateMachine, false));
        ValidateMatchThree(cell);
    }

    public void ValidateMatchThree(Cell cell = null)
    {
        bool validate = Validate();
        if (!validate && cell != null)
        {
            gridManager.StartCoroutine(gridManager.SwapCellBack(cell));
            soundManager.Swap();
        }
        else if (!validate && cell == null)
            ChangeState(new IdleState(this, stateMachine));
    }

    public bool Validate()
    {
        List<Cell> matchThree = gridManager.GetMatchThree();
        if (matchThree.Any())
        {
            ValidateMatchThreeEvent(gridManager.matchThree);
            soundManager.Clear();
            return true;
        }
        return false;
    }

    public void ResetContent()
    {
        gridManager.ResetContent();
    }

    public void ReorderGem()
    {
        ReorderedGemEvent(gridManager.ReorderGem());
    }

    public void Evaluate()
    {
        if (!gridManager.Evaluate())
            ChangeState(new ShuffleState(this, stateMachine));   
    }

    public void Shuffle()
    {
        StartCoroutine(Shuffle());
        IEnumerator Shuffle()
        {
            RespawnAllGem();
            warningText.enabled = true;
            yield return new WaitForSeconds(2.0f);
            ChangeState(new IdleState(this, stateMachine));
            warningText.enabled = false;
        }
    }
    #endregion

    #region Score Manager
    public void AddScore(List<Gem> gems)
    {
        scoreManager.AddScore(gems);
    }
    #endregion

    #region Timer Manager
    public void ResetTime()
    {
        timerManager.ResetTime();
    }
    #endregion

    #region Sound Manager
    public void PauseSound()
    {
        soundManager.PauseSound();
    }

    public void ResumeSound()
    {
        soundManager.ResumeSound();
    }
    #endregion
}
