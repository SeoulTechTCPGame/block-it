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
    
    public Color _p1PawnColor = new Color(0.95f, 0.48f, 0.48f);
    public Color _p2PawnColor = new Color(0.26f, 0.69f, 0.62f);

    public Color _p1SelectedPreviewColor = new Color(1.00f, 0.60f, 0.30f);
    public Color _p2SelectedPreviewColor = new Color(0.40f, 0.80f, 0.30f);

    public Color _previewColor = new Color(0.85f, 0.70f, 0.16f);
    private Color _disabledColor= new Color(0.66f, 0.80f, 0.86f);


    public static UnityEvent<EPlayer, Vector2Int> SetPawnCoord = new UnityEvent<EPlayer, Vector2Int>();
    public static UnityEvent<List<Vector2Int>> UpdateMoveablePawns = new UnityEvent<List<Vector2Int>>();
    public static UnityEvent RemoveMoveablePawns = new UnityEvent();
    public static UnityEvent ShowMoveablePawns = new UnityEvent();
    public static UnityEvent<EPlayer, Vector2Int> UpdateClickedPawn= new UnityEvent<EPlayer, Vector2Int>();

    private void Awake()
    {
        SetPawnCoord = new UnityEvent<EPlayer, Vector2Int>();
        SetPawnCoord.AddListener((player, coordinate) => setPawn(player, coordinate));

        UpdateMoveablePawns = new UnityEvent<List<Vector2Int>>();
        UpdateMoveablePawns.AddListener((moveableCoords) => updateMoveablePawns(moveableCoords));

        RemoveMoveablePawns = new UnityEvent();
        RemoveMoveablePawns.AddListener(removeMoveablePawn);

        ShowMoveablePawns = new UnityEvent();
        ShowMoveablePawns.AddListener(showMoveablePawns);

        UpdateClickedPawn = new UnityEvent<EPlayer, Vector2Int>();
        UpdateClickedPawn.AddListener( (turn, coordination) => updateClickedPawn(turn, coordination));
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
    private void updateMoveablePawns(List<Vector2Int> possibleList) 
    {
        possiblePawnList = possibleList;
    }
    private void removeMoveablePawn()
    {
        for (int i = 0; i < possiblePawnList.Count; i++)
        {
            Vector2Int coord = possiblePawnList[i];
            Cell targetCell = GetCell(coord.x, coord.y);
            targetCell.SetClickablePawn(false, _disabledColor);
        }
    }
    private void showMoveablePawns()
    {
        for (int i = 0; i < possiblePawnList.Count; i++)
        {
            Vector2Int coord = possiblePawnList[i];
            Cell targetCell = GetCell(coord.x, coord.y);
            targetCell.SetClickablePawn(true, _previewColor);
        }
    }

    private void setPawn(EPlayer ePlayer, Vector2Int coordinate) {

        Vector2Int previousCoordinate = Vector2Int.zero;
        Color pawnColor = Color.white;

        if (ePlayer == EPlayer.Player1)
        {
            previousCoordinate = _p1Coordinate;
            _p1Coordinate = coordinate;
            pawnColor = _p1PawnColor;
        }
        else
        {
            previousCoordinate = _p2Coordinate;
            _p2Coordinate = coordinate;
            pawnColor = _p2PawnColor;
        }


        if (previousCoordinate != Vector2Int.zero)
        {
            Cell previousCell = GetCell(previousCoordinate.x, previousCoordinate.y);
            previousCell.RemovePawn();
        }

        Cell targetCell = GetCell(coordinate.x, coordinate.y);
        targetCell.SetPawn(true, pawnColor);
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
            cells[COL - 1, row].SetPlankDot(false);
        }
        for (int col = 0; col < COL; col++)
        {
            cells[col, ROW-1].SetBottomPlank(false, Color.red);
            cells[col, ROW - 1].SetPlankDot(false);
        }
    }

    private void updateClickedPawn(EPlayer ePlayer, Vector2Int clickedCellCoord) 
    {
        for (int i = 0; i < possiblePawnList.Count; i++)
        {
            Vector2Int coord = possiblePawnList[i];
            Cell cell = GetCell(coord.x, coord.y);
            cell.SetClickablePawn(true, _previewColor);
        }

        
        Cell clickedCell = GetCell(clickedCellCoord.x, clickedCellCoord.y);
        Color clickedColor;
        if(ePlayer == EPlayer.Player1)
        {
            clickedColor = _p1SelectedPreviewColor;
        }
        else
        {
            clickedColor = _p2SelectedPreviewColor;
        }
        clickedCell.SetClickablePawn(true, clickedColor);

    }
}