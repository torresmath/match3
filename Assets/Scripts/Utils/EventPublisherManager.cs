using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventPublisherManager
{
    #region Events
    public event EventHandler<CellArgs> OnSelectCellEvent;
    public event EventHandler<CellArgs> OnSwapCellEvent;
    public event EventHandler<CellListArgs> OnValidateMatchThreeEvent;
    public event EventHandler<CellArgs> OnMatchThreeFailedEvent;
    public event EventHandler OnReorderGemEvent;
    public event EventHandler<bool> OnReorderedGemEvent;
    public event EventHandler OnLevelClearedEvent;
    public event EventHandler OnGameOverEvent;
    #endregion

    #region Event Publishers
    public void OnSelect(Cell cell)
    {
        OnSelectCellEvent?.Invoke(this, new CellArgs(cell));
    }

    public void OnSwap(Cell cell)
    {
        OnSwapCellEvent?.Invoke(this, new CellArgs(cell));
    }

    public void OnValidateMatchThree(List<Cell> cells)
    {
        OnValidateMatchThreeEvent?.Invoke(this, new CellListArgs(cells));
    }

    public void OnMatchThreeFailed(Cell cell)
    {
        OnMatchThreeFailedEvent?.Invoke(this, new CellArgs(cell));
    }

    public void OnReorderGem()
    {
        OnReorderGemEvent?.Invoke(this, EventArgs.Empty);
    }

    public void OnReorderedGem(bool keepReordering)
    {
        OnReorderedGemEvent?.Invoke(this, keepReordering);
    }

    public void OnLevelCleared()
    {
        OnLevelClearedEvent?.Invoke(this, EventArgs.Empty);
    }

    public void OnGameOver()
    {
        OnGameOverEvent?.Invoke(this, EventArgs.Empty);
    }

    #endregion

    #region Event Args
    public class CellArgs {
        public Cell cell;

        public CellArgs(Cell cell)
        {
            this.cell = cell;
        }
    }

    public class CellListArgs
    {
        public List<Cell> cells;

        public CellListArgs(List<Cell> cells)
        {
            this.cells = cells;
        }
    }
    #endregion

}
