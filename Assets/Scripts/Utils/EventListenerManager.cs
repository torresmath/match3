using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static EventPublisherManager;

public class EventListenerManager
{
    #region Properties
    public GameController gameController;
    protected EventPublisherManager publisher;
    #endregion

    #region Constructor
    public EventListenerManager(GameController gameController, EventPublisherManager publisher)
    {
        this.gameController = gameController;
        this.publisher = publisher;
        Initialize();
    }

    #endregion

    #region Event Mappers

    void Initialize()
    {
        publisher.OnSelectCellEvent += OnSelectCellListener;
        publisher.OnSwapCellEvent += OnSwapCellListener;
        publisher.OnValidateMatchThreeEvent += OnValidateMatchThreeListener;
        publisher.OnMatchThreeFailedEvent += OnMatchThreeFailedListener;
        publisher.OnReorderGemEvent += OnReorderGemListener;
        publisher.OnReorderedGemEvent += OnReorderedGemListener;
        publisher.OnGameOverEvent += OnGameOverListener;
        publisher.OnLevelClearedEvent += OnLevelClearedListener;
    }

    private void OnDisable()
    {
        publisher.OnSelectCellEvent -= OnSelectCellListener;
        publisher.OnSwapCellEvent -= OnSwapCellListener;
        publisher.OnValidateMatchThreeEvent -= OnValidateMatchThreeListener;
        publisher.OnMatchThreeFailedEvent -= OnMatchThreeFailedListener;
        publisher.OnReorderGemEvent -= OnReorderGemListener;
        publisher.OnReorderedGemEvent -= OnReorderedGemListener;
        publisher.OnGameOverEvent -= OnGameOverListener;
        publisher.OnLevelClearedEvent -= OnLevelClearedListener;
    }

    #endregion

    #region Event Listeners
    void OnSelectCellListener(object sender, CellArgs cellArgs)
    {
        gameController.ChangeState(new SelectState(gameController, gameController.stateMachine));
        gameController.SetSelectedCell(cellArgs.cell);
    }

    void OnSwapCellListener(object sender, CellArgs cellArgs)
    {
        gameController.ChangeState(new SwapState(gameController, gameController.stateMachine));
        bool canSwap = gameController.CanSwap(cellArgs.cell);

        if (canSwap)
            gameController.SwapCell(cellArgs.cell);
        else
            SetIdleState();

    }

    void OnValidateMatchThreeListener(object sender, CellListArgs cellListArgs)
    {
        List<Gem> gems = cellListArgs.cells.Select(cell => cell.Content.GetComponent<Gem>()).ToList();
        gameController.AddScore(gems);
        cellListArgs.cells.ForEach(cell => {
            GameObject content = cell.Content;
            cell.Content = null;
            UnityEngine.Object.Destroy(content);
        });
        gameController.ChangeState(new ReorderState(gameController, gameController.stateMachine));
    }

    void OnMatchThreeFailedListener(object sender, CellArgs cell)
    {
        SetIdleState();
    }

    void OnReorderGemListener(object sender, EventArgs e)
    {

    }

    void OnReorderedGemListener(object sender, bool keepReordering)
    {
        if (keepReordering)
            gameController.ReorderGem();
        
        else if(!gameController.Validate())
            SetIdleState();
            
    }

    void OnLevelClearedListener(object sender, EventArgs e)
    {
        gameController.ResetTime();
    }

    void OnGameOverListener(object sender, EventArgs e)
    {
        gameController.ExecuteGameOver();
    }
    #endregion

    #region Private
    void SetIdleState()
    {
        gameController.ChangeState(new IdleState(gameController, gameController.stateMachine));
    }
    #endregion
}
