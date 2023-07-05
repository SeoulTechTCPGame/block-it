using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour
{
    const int ROW = 9;
    const int COL = 9;

    [SerializeField] GameObject cellPrefab;

    private Cell[,] cells = new Cell[COL, ROW];
    private Vector2Int _p1Coordinate = new Vector2Int();
    private Vector2Int _p2Coordinate = new Vector2Int();
    private List<Vector2Int> possiblePawnList = new List<Vector2Int>();
    private Color _p1PawnColor = new Color(0.95f, 0.48f, 0.48f);
    private Color _p2PawnColor = new Color(0.26f, 0.69f, 0.62f);

    public static UnityEvent<EPlayer, Vector2Int> SetPawnCoord = new UnityEvent<EPlayer, Vector2Int>();
    public static UnityEvent<List<Vector2Int>> UpdateMoveablePawns = new UnityEvent<List<Vector2Int>>();

    private void Awake()
    {
        SetPawnCoord = new UnityEvent<EPlayer, Vector2Int>();
        SetPawnCoord.AddListener((player, coordinate) => setPawn(player, coordinate));

        UpdateMoveablePawns = new UnityEvent<List<Vector2Int>>();
        UpdateMoveablePawns.AddListener((moveableCoords) => makePossAble(moveableCoords));
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(InitializeBoard());
    }

    public Cell GetCell(int col, int row)
    {
        if (row >= 0 && row < ROW && col >= 0 && col < COL)
        {
            return cells[col, row];
        }
        else
        {
            Debug.LogError("Invalid cell coordinates!");
            return null;
        }
    }

    IEnumerator InitializeBoard()
    {
        createBoard();

        yield return null;

        setEdge();

    }

    //make only possible route pressable
    private void makePossAble(List<Vector2Int> possibleList) 
    {
        //DeSelect clicked one
        for(int i = 0; i < possiblePawnList.Count; i++)
        {
            Vector2Int coord = possiblePawnList[i];
            Cell targetCell = GetCell(coord.x, coord.y);
            targetCell.SetClickablePawn(false);
        }
        //Select ones
        for(int i = 0; i < possibleList.Count; i++)
        {
            Vector2Int coord = possibleList[i];
            Cell targetCell = GetCell(coord.x, coord.y);
            targetCell.SetClickablePawn(true);
        }
        possiblePawnList = possibleList;
    }

    private void setPawn(EPlayer ePlayer, Vector2Int coordinate) {
        
        if(ePlayer == EPlayer.Player1)
        {
            if(_p1Coordinate != Vector2Int.zero)
            {
                Cell cell = GetCell(_p1Coordinate.x, _p1Coordinate.y);
                cell.RemovePawn();
            }
            
            _p1Coordinate = coordinate;

            Cell targetCell = GetCell(_p1Coordinate.x, _p1Coordinate.y);
            targetCell.SetPawn(true, _p1PawnColor);
        }
        else
        {
            if (_p2Coordinate != Vector2Int.zero)
            {
                Cell cell = GetCell(_p2Coordinate.x, _p2Coordinate.y);
                cell.RemovePawn();
            }

            _p2Coordinate = coordinate;

            Cell targetCell = GetCell(_p2Coordinate.x, _p2Coordinate.y);
            targetCell.SetPawn(true, _p2PawnColor);
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
                cell.SetCoordinate(col, row);
                cells[col, row] = cell;

                cellGO.name = "Cell_( " + col + ", " + row +" )";

            }
        }
    }

    private void setEdge()
    {
        for (int row = 0; row < ROW; row++)
        {
            cells[COL - 1, row].SetRightPlank(false, Color.red);
        }
        for (int col = 0; col < COL; col++)
        {
            cells[col, ROW-1].SetBottomPlank(false, Color.red);
        }
    }
}