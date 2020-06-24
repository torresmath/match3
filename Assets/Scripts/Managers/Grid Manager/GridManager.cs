using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class GridManager : MonoBehaviour , IManager
{
    #region Const
    private const string COLUMN = "Column";
    private const string LINE = "Line";
    #endregion

    #region Properties
    [SerializeField] protected Cell selectedCell;
    public Cell SelectedCell { get { return selectedCell; } set { selectedCell = value; } }
    protected Color32 selectedColor;
    public Color32 SelectedColor { set { selectedColor = value; } }

    public List<Cell> cells;
    public List<Cell> matchThree;
   
    private GameController gameController;
    private GridLayoutGroup grouper;
    private int x;
    private int y;

    #endregion

    #region MonoBehaviour
    void Start()
    {
        gameController = GetComponentInParent<GameController>();
        grouper = GetComponentInChildren<GridLayoutGroup>();

        x = gameController.X;
        y = gameController.Y;
        SetGrouper();
        SetLists(x, "Line");
        SetLists(y, "Column", true);
    }
    #endregion

    #region IManager
    public void Init()
    {
        ResetContent();
    }
    #endregion

    #region Public

    public void SetSelectedCell(Cell cell)
    {
        selectedCell = cell;
        SpriteRenderer sprite = selectedCell.GetComponentInChildren<SpriteRenderer>();
        sprite.color = selectedColor;

    }

    public bool CanSwap(Cell cell)
    {
        if (cell == null || selectedCell == null)
            return false;

        return GetSelectCellNeighbours().Contains(cell);
    }

    public void SwapCell(Cell cell)
    {
        if (cell == null || selectedCell == null)
            return;

        if (cell.Content == null || selectedCell.Content == null)
            return;

        SwapCellRoutine(cell);
    }

    public IEnumerator SwapCellBack(Cell cell)
    {
        yield return new WaitForSeconds(.2f);
        SwapCell(cell);
        yield return new WaitForSeconds(.2f);
        gameController.ChangeState(new IdleState(gameController, gameController.stateMachine));
    }

    public List<Cell> GetNeighbours(Cell cell)
    {

        return GetNeighboursInList(cell, cells);
    }

    public void ResetContent()
    {
        cells.ForEach(cell => cell.Normalize());
        if (selectedCell != null)
            selectedCell = null;
        
        matchThree = new List<Cell>();
    }

    public List<Cell> GetMatchThree()
    {
        List<Grid> grids = GetComponentsInChildren<Grid>().ToList();
        grids.ForEach(grid => {
            List<Cell> lines = grid.lines; ;
            for (int i = 0; i < lines.Count - 1; i++) {
                GameObject content = lines[i].Content;
                Gem gem = content.GetComponent<Gem>();
                int gemId = gem.Id;

                List<Cell> neighbours = GetNeighboursInList(lines[i], lines);
                bool isMatchThree = neighbours.TrueForAll(cell => cell.Content.GetComponent<Gem>().Id == gemId) 
                && neighbours.Count == 2;

                if (isMatchThree)
                {
                    matchThree.Add(lines[i]);
                    neighbours.ForEach(c => {
                        if (!matchThree.Contains(c))
                        {
                            matchThree.Add(c);
                        }
                    });
                }
            }
        });

        return matchThree;
    }

    public bool ReorderGem()
    {
        List<Grid> columns = GetComponentsInChildren<Grid>().ToList().Where(grid => grid.name.Contains(COLUMN)).ToList();
        columns.ForEach(column => {
            List<Cell> cellColumn = column.lines;
            for (int i = 0; i < cellColumn.Count; i++) {
                GameObject content = cellColumn[i].Content;
                if (content != null)
                    continue;
                int j = i - 1;
                if (j == -1)
                {
                    gameController.SpawnGem(cellColumn[i]);
                }
                else if (cellColumn[j].Content != null) {                    
                    cellColumn[i].Content = cellColumn[j].Content;
                    cellColumn[j].Content = null;
                }

                cellColumn[i].Normalize();
            }
        });

        bool keepReordering = !columns.TrueForAll(column => column.lines.TrueForAll(line => line.Content != null));
        return keepReordering;
    }

    // Filter cells with only one neighbour with same gem
    // and check if the other neighbour has neighbours with the specific gem
    // If return any true, there are still possible plays
    public bool Evaluate()
    {
        List<Grid> grids = GetComponentsInChildren<Grid>().ToList();
        List<bool> list = grids.Select(grid => grid.lines)
            .Select(line =>
            {
                for (int i = 1; i < line.Count - 1; i++)
                {
                    GameObject content = line[i].Content;
                    Gem gem = content.GetComponent<Gem>();
                    int gemId = gem.Id;

                    List<Cell> neighbours = GetNeighboursInList(line[i], line);
                    Cell equalNeighbour = neighbours.Where(cell => cell.Content.GetComponent<Gem>().Id == gemId).FirstOrDefault();
                    Cell differentNeighbour = neighbours.Where(cell => cell.Content.GetComponent<Gem>().Id != gemId).FirstOrDefault();

                    if (equalNeighbour != null && differentNeighbour != null)
                    {

                        List<Cell> differentCellNeighbours = GetNeighbours(differentNeighbour).Where(cell => cell.Content.GetComponent<Gem>().Id == gemId).ToList();
                        if (differentCellNeighbours.Count > 1)
                        {
                            return true;
                        }

                    }
                }

                return false;
            }).ToList();

        return list.Any(line => line == true);
    }
    #endregion

    #region Private
    void SetGrouper()
    {
        grouper.childAlignment = TextAnchor.MiddleCenter;
        grouper.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grouper.constraintCount = y;
    }


    void SetLists(int index, string listName, bool isColumn = false)
    {
        for (int i = 0; i < index; i++)
        {
            GameObject lineObject = new GameObject();
            lineObject.transform.SetParent(this.transform);
            lineObject.name = listName + " " + i;
            lineObject.AddComponent<Grid>();
            Grid line = lineObject.GetComponent<Grid>();
            line.lines = new List<Cell>(index);
            
            cells.Where(cell => {
                int reference = isColumn ? cell.Y : cell.X;
                return reference == i;
            }).ToList()
            .ForEach(cell => line.lines.Add(cell));
        }
    }

    List<Cell> GetSelectCellNeighbours()
    {
        if (selectedCell == null)
            return new List<Cell>();


        return GetNeighbours(selectedCell);
    }

    List<Cell> GetNeighboursInList(Cell cell, List<Cell> cellList)
    {
        int CellValue = cell.X + cell.Y;
        return cellList.Where(neighbour =>
        {
            return Mathf.Abs(cell.X - neighbour.X) + Mathf.Abs(cell.Y - neighbour.Y) == 1;
        }).ToList();
    }

    void SwapCellRoutine(Cell cell)
    {
        GameObject temp = cell.Content;
        cell.Content = selectedCell.Content;
        cell.Content.transform.parent = cell.transform;
        cell.Content.transform.localPosition = new Vector2(0, 0);

        selectedCell.Content = temp;
        selectedCell.Content.transform.parent = selectedCell.transform;
        selectedCell.Content.transform.localPosition = new Vector2(0, 0);
    }
    #endregion
}
