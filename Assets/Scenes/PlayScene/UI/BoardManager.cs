using UnityEngine;
using System.Collections;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private GameObject cellPrefab;

    const int ROW = 9;
    const int COL = 9;
    private Cell[,] cells = new Cell[ROW, COL];

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(InitializeBoard());
    }
    IEnumerator InitializeBoard()
    {
        createBoard();

        yield return null;

        setEdge();

        Cell cell = GetCell(1, 1);
        cell.SetPawn(true, Color.red);
    }

    public Cell GetCell(int row, int col)
    {
        if (row >= 0 && row < ROW && col >= 0 && col < COL)
        {
            return cells[row, col];
        }
        else
        {
            Debug.LogError("Invalid cell coordinates!");
            return null;
        }
    }

    private void createBoard()
    {
        for (int row = 0; row < ROW; row++)
        {
            for (int col = 0; col < COL; col++)
            {
                GameObject cellGO = Instantiate(cellPrefab, transform);
                Cell cell = cellGO.GetComponent<Cell>();
                cell.SetEdge(col == COL - 1, row == ROW - 1);
                cell.SetCoordinate(row, col);
                cells[row, col] = cell;

                cellGO.name = "Cell_( " + row + ", " + col +" )";
            }
        }
    }
    private void setEdge()
    {
        for (int row = 0; row < ROW; row++)
        {
            cells[row, COL-1].SetRightPlank(false, Color.red);
        }
        for (int col = 0; col < COL; col++)
        {
            cells[ROW-1, col].SetBottomPlank(false, Color.red);
        }
    }
}