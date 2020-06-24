using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{

    #region Properties
    protected int x;
    public int X { get { return x; } set { x = value; } }
    protected int y;
    public int Y { get { return y; } set { y = value; } }
    protected GameController gameController;
    public GameController GameController { set { gameController = value; } }
    protected GameObject content;
    protected List<Cell> neighbours;
    public GameObject Content { get { return content; } set { content = value; } }
    public List<Cell> Neighbours { get { return neighbours; } set { neighbours = value; } }
    #endregion

    #region MonoBehaviour
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RectTransform rt = GetComponent<RectTransform>();
            Rect rect = rt.rect;

            Vector2 lp;
            Vector2 mousePos = Input.mousePosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, mousePos, Camera.main, out lp);
            if (rect.Contains(lp))
                OnClick();
        }
    }
    #endregion

    #region Public
    public void OnClick()
    {
        State currentState = gameController.GetCurrentState();

        if (currentState.GetType().Equals(typeof(IdleState)))
            Select();

        if (currentState.GetType().Equals(typeof(SelectState)))
            Swap();
    }

    public void Select()
    {
        gameController.SelectCellEvent(this);
    }
    public void Swap()
    {
        gameController.SwapCellEvent(this);
    }

    public void Normalize()
    {
        SpriteRenderer sprite = GetComponentInChildren<SpriteRenderer>();
        
        if (sprite != null)
            sprite.color = new Color32(255, 255, 255, 255);

        if (content != null)
        {
            content.transform.parent = transform;
            content.transform.localPosition = new Vector2(0, 0);
        }
    }
    #endregion
}
