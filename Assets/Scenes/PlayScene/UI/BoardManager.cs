using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour
{
    const int ROW = 9;
    const int COL = 9;

    [SerializeField] GameObject cellPrefab;
    [SerializeField] GameObject p1RemainPlank;
    [SerializeField] GameObject p2RemainPlank;

    private Cell[,] cells = new Cell[COL, ROW];
    private Vector2Int _p1Coordinate = new Vector2Int();
    private Vector2Int _p2Coordinate = new Vector2Int();
    private List<Plank> Planks= new List<Plank>();
      
    private List<Vector2Int> possiblePawnList = new List<Vector2Int>();
    private List<Vector2Int> placeableVerticalPlanks = new List<Vector2Int>();
    private List<Vector2Int> placeableHorizontalPlanks = new List<Vector2Int>();
    private Plank _previewPlank = new Plank();

    bool _bPreviewPlank = false;

    public Color _p1PawnColor = new Color(0.70f, 0.01f, 0.01f);
    public Color _p2PawnColor = new Color(0.11f, 0.36f, 0.60f);

    public Color _p1SelectedPreviewColor = new Color(0.96f, 0.57f, 0.15f);
    public Color _p2SelectedPreviewColor = new Color(0.45f, 0.76f, 0.96f);

    public Color _p1PreviewColor = new Color(0.45f, 0.76f, 0.96f);
    public Color _p2PreviewColor = new Color(1.00f, 0.82f, 0.18f);
    private Color _disabledColor= new Color(0.66f, 0.80f, 0.86f);


    public static UnityEvent<EPlayer, Vector2Int> SetPawnCoord = new UnityEvent<EPlayer, Vector2Int>();
    public static UnityEvent<List<Vector2Int>> UpdateMoveablePawns = new UnityEvent<List<Vector2Int>>();
    public static UnityEvent RemoveMoveablePawns = new UnityEvent();
    public static UnityEvent<EPlayer> ShowMoveablePawns = new UnityEvent<EPlayer>();
    public static UnityEvent<EPlayer> ReduceRemainPlank = new UnityEvent<EPlayer>();
    public static UnityEvent<EPlayer, Vector2Int> UpdateClickedPawn= new UnityEvent<EPlayer, Vector2Int>();

    public static UnityEvent<List<Vector2Int>, List<Vector2Int>> UpdatePlaceablePlanks = new UnityEvent<List<Vector2Int>, List<Vector2Int>>();
    public static UnityEvent<EDirection, EPlayer> ShowPlaceablePlanks= new UnityEvent<EDirection, EPlayer>();
    public static UnityEvent RemovePlaceablePlanks = new UnityEvent();

    public static UnityEvent<Vector2Int, EDirection, EPlayer> PlacePreviewPlank = new UnityEvent<Vector2Int, EDirection, EPlayer>();
    public static UnityEvent RemovePreviewPlank = new UnityEvent();
    public static UnityEvent<Plank> PlacePlank = new UnityEvent<Plank>();
    
    private void Awake()
    {
        setEvents();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(InitializeBoard());
    }


    private void setEvents()
    {
        //pawn
        SetPawnCoord = new UnityEvent<EPlayer, Vector2Int>();
        SetPawnCoord.AddListener((player, coordinate) => setPawn(player, coordinate));

        UpdateMoveablePawns = new UnityEvent<List<Vector2Int>>();
        UpdateMoveablePawns.AddListener((moveableCoords) => updateMoveablePawns(moveableCoords));

        RemoveMoveablePawns = new UnityEvent();
        RemoveMoveablePawns.AddListener(removeMoveablePawn);

        ShowMoveablePawns = new UnityEvent<EPlayer>();
        ShowMoveablePawns.AddListener((ePlayer) => showMoveablePawns(ePlayer));

        UpdateClickedPawn = new UnityEvent<EPlayer, Vector2Int>();
        UpdateClickedPawn.AddListener((turn, coordination) => updateClickedPawn(turn, coordination));

        //plank Dot
        UpdatePlaceablePlanks= new UnityEvent<List<Vector2Int>, List<Vector2Int>>();
        UpdatePlaceablePlanks.AddListener((horizontal, vertical) => updatePlaceablePlanks(horizontal, vertical));

        ShowPlaceablePlanks = new UnityEvent<EDirection, EPlayer>();
        ShowPlaceablePlanks.AddListener((eDirection, ePlayer) => showPlaceablePlankDot(eDirection, ePlayer));

        RemovePlaceablePlanks = new UnityEvent();
        RemovePlaceablePlanks.AddListener(removePlaceablePlankDot);

        // Plank
        PlacePreviewPlank = new UnityEvent<Vector2Int, EDirection, EPlayer>();
        PlacePreviewPlank.AddListener((coord, direction, player) => placePreviewPlank(coord, direction, player));

        RemovePreviewPlank = new UnityEvent();
        RemovePreviewPlank.AddListener(removePreviewPlank);

        PlacePlank = new UnityEvent<Plank>();
        PlacePlank.AddListener((plank) => placePlank(plank));

        // remain plank
        ReduceRemainPlank = new UnityEvent<EPlayer>();
        ReduceRemainPlank.AddListener((ePlayer) => reduceRemainPlank(ePlayer));

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

        setRemainPlank(10);
    }


    private void setRemainPlank(int defaultPlankNum)
    {
        p1RemainPlank.GetComponent<RemainPlank>().CreatePlank(defaultPlankNum);
        p2RemainPlank.GetComponent<RemainPlank>().CreatePlank(defaultPlankNum);
    }

    private void reduceRemainPlank(EPlayer ePlayer)
    {
        GameObject targetRemainPlank;
        if(ePlayer == EPlayer.Player1) 
        {
            targetRemainPlank = p1RemainPlank;
        }
        else
        {
            targetRemainPlank = p2RemainPlank;
        }

        targetRemainPlank.GetComponent<RemainPlank>().ReduceRemainPlank();
    }

    //make only possible route pressable
    private void updateMoveablePawns(List<Vector2Int> possibleList) 
    {
        possiblePawnList = possibleList;
    }

    private void removeMoveablePawn()
    {
        foreach (Vector2Int coord in possiblePawnList)
        {
            Cell targetCell = GetCell(coord.x, coord.y);
            targetCell.SetClickablePawn(false, _disabledColor);
        }
    }
    
    private void showMoveablePawns(EPlayer ePlayer)
    {
        Color previewColor = getPreviewColor(ePlayer);

        foreach (Vector2Int coord in possiblePawnList)
        {
            Cell targetCell = GetCell(coord.x, coord.y);
            targetCell.SetClickablePawn(true, previewColor);
        }
    }

    private void updatePlaceablePlanks(List<Vector2Int> horizontal, List<Vector2Int> vertical)
    {
        placeableHorizontalPlanks = horizontal;
        placeableVerticalPlanks = vertical;
    }

    private void showPlaceablePlankDot(EDirection eDirection, EPlayer ePlayer)
    {
        removePlaceablePlankDot();
        List<Vector2Int> targetCoords = getPlaceablePlankDots(eDirection);
        Color color = getPreviewColor(ePlayer);

        foreach (Vector2Int coord in targetCoords)
        {
            Cell targetCell = GetCell(coord.x, coord.y);
            targetCell.SetPlankDot(true, color);
        }
    }

    private void removePlaceablePlankDot()
    {
        foreach (Vector2Int coord in placeableVerticalPlanks)
        {
            Cell targetCell = GetCell(coord.x, coord.y);
            targetCell.SetPlankDot(false, Color.white);
        }

        foreach (Vector2Int coord in placeableHorizontalPlanks)
        {
            Cell targetCell = GetCell(coord.x, coord.y);
            targetCell.SetPlankDot(false, Color.white);
        }
    }

    private List<Vector2Int> getPlaceablePlankDots(EDirection eDirection)
    {
        return eDirection == EDirection.Horizontal ? placeableHorizontalPlanks : placeableVerticalPlanks;
    }

    private Color getPreviewColor(EPlayer ePlayer)
    {
        return ePlayer == EPlayer.Player1 ? _p1PreviewColor : _p2PreviewColor;
    }

    private Color getClickedColor(EPlayer ePlayer)
    {
        return ePlayer == EPlayer.Player1 ? _p1SelectedPreviewColor : _p2SelectedPreviewColor;
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

    private void setPlank(Vector2Int coordinate, EDirection eDirection, bool visible, Color color )
    {
        Cell cell1 = GetCell(coordinate.x, coordinate.y);
        Cell cell2;
        string targetMiddle;

        if (eDirection == EDirection.Vertical)
        {
            cell2 = GetCell(coordinate.x, coordinate.y + 1);

            cell1.SetRightPlank(visible, color);
            cell2.SetRightPlank(visible, color);
            targetMiddle = "Vertical";
        }
        else if(eDirection == EDirection.Horizontal)
        {
            cell2 = GetCell(coordinate.x + 1, coordinate.y);

            cell1.SetBottomPlank(visible, color);
            cell2.SetBottomPlank(visible, color);
            targetMiddle = "Horizontal";
        }
        else
        {
            Debug.LogError("BoardManager - setPlank: Invalid Plank Direction!");
            return;
        }
        cell1.SetBottomRightPlank(targetMiddle, visible, color);
    }

    private void placePreviewPlank(Vector2Int coordinate, EDirection eDirection, EPlayer ePlayer)
    {
        removePreviewPlank();
        _previewPlank.SetPlank(coordinate, eDirection);

        Color color= getClickedColor(ePlayer);

        setPlank(coordinate, eDirection, true, color);
        _bPreviewPlank = true;
    }

    private void removePreviewPlank()
    {
        if(_bPreviewPlank == false)
        {
            return;
        }
        Vector2Int targetCoord = _previewPlank.GetCoordinate();
        EDirection direction = _previewPlank.GetDirection();

        setPlank(targetCoord, direction, false, Color.white);

        _bPreviewPlank = false;
    }

    private void placePlank(Plank plank)
    {
        Planks.Add(plank);
        foreach(Plank targetPlank in Planks)
        {
            Vector2Int coord = targetPlank.GetCoordinate();
            EDirection eDirection = targetPlank.GetDirection();
            setPlank(coord, eDirection, true, Color.black);
        }
        _bPreviewPlank = false;
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
        Color previewColor = getPreviewColor(ePlayer);
        for (int i = 0; i < possiblePawnList.Count; i++)
        {
            Vector2Int coord = possiblePawnList[i];
            Cell cell = GetCell(coord.x, coord.y);
            cell.SetClickablePawn(true, previewColor);
        }

        
        Cell clickedCell = GetCell(clickedCellCoord.x, clickedCellCoord.y);
        Color clickedColor = getClickedColor(ePlayer);
        clickedCell.SetClickablePawn(true, clickedColor);

    }
}