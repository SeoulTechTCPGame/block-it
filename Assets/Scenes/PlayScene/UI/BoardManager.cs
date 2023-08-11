using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;

/*
 'BoardManager'�� ���带 �����ϰ�, ���忡 �ʿ��� View&Control�� �����ϴ� Ŭ�����Դϴ�.
   - BoardManager�� �����ϴ� ������Ʈ�� ������ �����ϴ�.
     - _cells: Cell�� �迭�� ������ �ֽ��ϴ�.
       - � Cell�� Pawn�� ǥ������ �����մϴ�.
       - � Cell�� � Plank�� ǥ������ �����մϴ�.
       - � Cell�� PlankDot�� ǥ������ �����մϴ�.
       - � Cell�� ClickablePawn(PawnButton - pawn�� �̵������� ��ġ - ��ư���� Ŭ������)�� ǥ������ �����մϴ�.
     - _p1RemainPlank, _p2RemainPlank: �� �÷��̾��� ���� �÷�ũ�� ǥ���մϴ�.

    - �� �ϸ��� _gameLogic���� ������ ������, ���带 ����� �ٽ� �׸��ϴ�.(���� ����� x, ǥ�õ� �͵�)
    - ��� �ż���� private��, Event�� ���� ����
 */
public class BoardManager : MonoBehaviour
{
    const int ROW = 9; // ����
    const int COL = 9; // ����

    #region SerializeFeild Prefabs
    [SerializeField] GameObject _cellPrefab;
    [SerializeField] GameObject _p1RemainPlank;
    [SerializeField] GameObject _p2RemainPlank;
    #endregion

    private GameLogic _gameLogic;

    #region info for display
    private Cell[,] _cells = new Cell[COL, ROW]; // cell���� ��� �迭
    private Vector2Int _p1Coordinate = new Vector2Int(); // p1�� ��ǥ
    private Vector2Int _p2Coordinate = new Vector2Int(); // p2�� ��ǥ
    private List<Plank> _planks= new List<Plank>(); // ǥ�õ� plank���� �迭
    private List<Vector2Int> _possiblePawnList = new List<Vector2Int>(); // pawn�� �̵������� ��ǥ�� (CLickablePawn���� ���� ��ǥ �迭 
    private List<Vector2Int> _placeableVerticalPlanks = new List<Vector2Int>(); // plank�� ���η� ������ �ִ� plankDot�� ��ǥ �迭
    private List<Vector2Int> _placeableHorizontalPlanks = new List<Vector2Int>(); // plank�� ���η� ������ �ִ� plankDot�� ��ǥ �迭
    private Plank _previewPlank = new Plank(); // �̸����� plank (������ ������ plank, plankDot�� ���� ����)
    bool _isPreviewPlank = false; // �̸����� plank�� �ִ°� - ������ plank�� ���Ҵ��� ���� �ʾҴ��� �˱����� ����
    #endregion

    #region Colors
    // needs to be const when it is deteremined
    public Color P1_PAWN_COLOR = new Color(0.70f, 0.01f, 0.01f);
    public Color P2_PAWN_COLOR = new Color(0.11f, 0.36f, 0.60f);

    public Color P1_SELECTED_PREVIEW_COLOR = new Color(0.96f, 0.57f, 0.15f);
    public Color P2_SELECTED_PREVIEW_COLOR = new Color(0.45f, 0.76f, 0.96f);

    public Color P1_PREVIEW_COLOR = new Color(0.45f, 0.76f, 0.96f);
    public Color P2_PREVIEW_COLOR = new Color(1.00f, 0.82f, 0.18f);
    private Color DISAPBLED_COLOR= new Color(0.66f, 0.80f, 0.86f);
    #endregion

    #region Evenets
    public static UnityEvent<Enums.EPlayer> ShowMoveablePawns = new UnityEvent<Enums.EPlayer>(); // ���忡 pawn�� �̵� ��ų �� �ִ� Cell�� ClickablePawn�� Ȱ��ȭ���� ǥ��
    public static UnityEvent RemoveMoveablePawns = new UnityEvent(); // ���忡 pawn�� �̵� ��ų �� �ִ� Cell�� ClickablePawn�� ��Ȱ��ȭ���� ���� (������ ���� ������ pawn�� ��ġ�� ������)
    public static UnityEvent<Enums.EPlayer, Vector2Int> UpdateClickedPawn= new UnityEvent<Enums.EPlayer, Vector2Int>(); // ������ ���� ������ pawn�� ��ġ�� ǥ��

    public static UnityEvent<EDirection, Enums.EPlayer> ShowPlaceablePlanks= new UnityEvent<EDirection, Enums.EPlayer>(); // ���忡 plank�� ���� �� �ִ� Cell�� PlankDot�� Ȱ��ȭ���� ǥ��
    public static UnityEvent RemovePlaceablePlanks = new UnityEvent();// ���忡 plank�� ���� �� �ִ� Cell�� PlankDot�� ��Ȱ��ȭ���� ����
    public static UnityEvent<Vector2Int, EDirection, Enums.EPlayer> PlacePreviewPlank = new UnityEvent<Vector2Int, EDirection, Enums.EPlayer>();// ������ ���� ������ plank�� ��ġ�� ǥ��
    public static UnityEvent RemovePreviewPlank = new UnityEvent(); // ������ ���� �����ߴ� plank�� ����

    public static UnityEvent UpdateBoard = new UnityEvent(); // ���忡 �ʿ��� info�� ������Ʈ�ϰ� ���忡 ǥ��
    public static UnityEvent ResetState = new UnityEvent(); // ���忡 �ʿ��� state(_isPreviewPlank) ����
    #endregion

    void Awake() // �̺�Ʈ ����, GameLogic �ޱ�
    {
        SetEvents();
        _gameLogic = FindObjectOfType<GameLogic>();
    }

    // Start is called before the first frame update
    void Start() // ���� �����
    {
        StartCoroutine(InitializeBoard());
    }

    private void SetEvents() // �̺�Ʈ ����
    {
        RemoveMoveablePawns = new UnityEvent();
        RemoveMoveablePawns.AddListener(UnmarkMoveablePawn);

        ShowMoveablePawns = new UnityEvent<Enums.EPlayer>();
        ShowMoveablePawns.AddListener((ePlayer) => MarkMoveablePawn(ePlayer));

        UpdateClickedPawn = new UnityEvent<Enums.EPlayer, Vector2Int>();
        UpdateClickedPawn.AddListener((turn, coordination) => MarkClickedPawn(turn, coordination));

        ShowPlaceablePlanks = new UnityEvent<EDirection, Enums.EPlayer>();
        ShowPlaceablePlanks.AddListener((eDirection, ePlayer) => MarkPlaceablePlankDot(eDirection, ePlayer));

        RemovePlaceablePlanks = new UnityEvent();
        RemovePlaceablePlanks.AddListener(UnmarkPlaceablePlankDot);

        // Plank
        PlacePreviewPlank = new UnityEvent<Vector2Int, EDirection, Enums.EPlayer>();
        PlacePreviewPlank.AddListener((coord, direction, player) => MarkPreviewPlank(coord, direction, player));

        RemovePreviewPlank = new UnityEvent();
        RemovePreviewPlank.AddListener(UnmarkPreviewPlank);

        UpdateBoard = new UnityEvent();
        UpdateBoard.AddListener(Refresh);

        ResetState = new UnityEvent();
        ResetState.AddListener(ResetStates);
    }

    public Cell GetCell(int col, int row)//�ش� ��ǥ�� cell ��������
    {
        if (row >= 0 && row < ROW && col >= 0 && col < COL)
        {
            return _cells[col, row];
        }
        else
        {
            Debug.LogError("Invalid cell coordinates!");
            return null;
        }
    }

    private void UnmarkMoveablePawn() // ���忡 pawn�� �̵� ��ų �� �ִ� Cell�� ClickablePawn�� ��Ȱ��ȭ���� ���� (������ ���� ������ pawn�� ��ġ�� ������)
    {
        foreach (Vector2Int coord in _possiblePawnList)
        {
            Cell targetCell = GetCell(coord.x, coord.y);
            targetCell.SetClickablePawn(false, DISAPBLED_COLOR);
        }
    }
    
    private void MarkMoveablePawn(Enums.EPlayer ePlayer) // ���忡 pawn�� �̵� ��ų �� �ִ� Cell�� ClickablePawn�� Ȱ��ȭ���� ǥ��
    {
        Color previewColor = GetPreviewColor(ePlayer);

        foreach (Vector2Int coord in _possiblePawnList)
        {
            Cell targetCell = GetCell(coord.x, coord.y);
            targetCell.SetClickablePawn(true, previewColor);
        }
    }

    private void MarkPlaceablePlankDot(EDirection eDirection, Enums.EPlayer ePlayer) // ���忡 plank�� ���� �� �ִ� Cell�� PlankDot�� Ȱ��ȭ���� ǥ��
    {
        UnmarkPlaceablePlankDot();
        List<Vector2Int> targetCoords = GetPlaceablePlankDots(eDirection);
        Color color = GetPreviewColor(ePlayer);

        foreach (Vector2Int coord in targetCoords)
        {
            Cell targetCell = GetCell(coord.x, coord.y);
            targetCell.SetPlankDot(true, color);
        }
    }

    private void UnmarkPlaceablePlankDot()  // ������ ���� �����ߴ� plank�� ����
    {
        foreach (Vector2Int coord in _placeableVerticalPlanks)
        {
            Cell targetCell = GetCell(coord.x, coord.y);
            targetCell.SetPlankDot(false, Color.white);
        }

        foreach (Vector2Int coord in _placeableHorizontalPlanks)
        {
            Cell targetCell = GetCell(coord.x, coord.y);
            targetCell.SetPlankDot(false, Color.white);
        }
    }

    private List<Vector2Int> GetPlaceablePlankDots(EDirection eDirection)
    {
        return eDirection == EDirection.Horizontal ? _placeableHorizontalPlanks : _placeableVerticalPlanks;
    }

    private Color GetPreviewColor(Enums.EPlayer ePlayer)
    {
        return ePlayer == Enums.EPlayer.Player1 ? P1_PREVIEW_COLOR : P2_PREVIEW_COLOR;
    }

    private Color GetClickedColor(Enums.EPlayer ePlayer)
    {
        return ePlayer == Enums.EPlayer.Player1 ? P1_SELECTED_PREVIEW_COLOR : P2_SELECTED_PREVIEW_COLOR;
    }

    private void SetPawn(Enums.EPlayer ePlayer, Vector2Int coordinate) // pawn ǥ��
    {
        Vector2Int previousCoordinate = Vector2Int.zero;
        Color pawnColor = Color.white;

        if (ePlayer == Enums.EPlayer.Player1)
        {
            previousCoordinate = _p1Coordinate;
            _p1Coordinate = coordinate;
            pawnColor = P1_PAWN_COLOR;
        }
        else
        {
            previousCoordinate = _p2Coordinate;
            _p2Coordinate = coordinate;
            pawnColor = P2_PAWN_COLOR;
        }


        if (previousCoordinate != Vector2Int.zero)
        {
            Cell previousCell = GetCell(previousCoordinate.x, previousCoordinate.y);
            previousCell.RemovePawn();
        }

        Cell targetCell = GetCell(coordinate.x, coordinate.y);
        targetCell.SetPawn(true, pawnColor);
    }

    private void SetPlank(Vector2Int coordinate, EDirection eDirection, bool visible, Color color ) // plank ǥ��
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
            Debug.LogError("BoardManager - SetPlank: Invalid Plank Direction!");
            return;
        }
        cell1.SetBottomRightPlank(targetMiddle, visible, color);
    }

    private void MarkPreviewPlank(Vector2Int coordinate, EDirection eDirection, Enums.EPlayer ePlayer) // ������ ���� ������ plank�� ��ġ�� ǥ��
    {
        UnmarkPreviewPlank();
        _previewPlank.SetPlank(coordinate, eDirection);

        Color color= GetClickedColor(ePlayer);

        SetPlank(coordinate, eDirection, true, color);
        _isPreviewPlank = true;

    }

    private void UnmarkPreviewPlank() // ������ ���� �����ߴ� plank�� ����
    {
        if(_isPreviewPlank == false)
        {
            return;
        }
        Vector2Int targetCoord = _previewPlank.GetCoordinate();
        EDirection direction = _previewPlank.GetDirection();

        SetPlank(targetCoord, direction, false, Color.white);

        _isPreviewPlank = false;
    }

    #region Initializing
    IEnumerator InitializeBoard()
    {
        CeateBoard();
          
        yield return null;

        SetEdge();

        SetRemainPlank(10);
    }

    private void SetRemainPlank(int defaultPlankNum)
    {
        _p1RemainPlank.GetComponent<RemainPlank>().CreatePlank(defaultPlankNum);
        _p2RemainPlank.GetComponent<RemainPlank>().CreatePlank(defaultPlankNum);
    }
    private void CeateBoard()
    {
        for (int row = 0; row < ROW; row++)
        {
            for (int col = 0; col < COL; col++)
            {
                GameObject cellGO = Instantiate(_cellPrefab, transform);
                Cell cell = cellGO.GetComponent<Cell>();
                cell.SetEdge(col == COL - 1, row == ROW - 1);
                cell.SetCoordinate(col, row);
                _cells[col, row] = cell;

                cellGO.name = "Cell_( " + col + ", " + row +" )";

            }
        }
    }
    private void SetEdge()
    {
        for (int row = 0; row < ROW; row++)
        {
            _cells[COL - 1, row].SetRightPlank(false, Color.red);
            _cells[COL - 1, row].SetPlankDot(false);
        }
        for (int col = 0; col < COL; col++)
        {
            _cells[col, ROW-1].SetBottomPlank(false, Color.red);
            _cells[col, ROW - 1].SetPlankDot(false);
        }
    } 
    #endregion

    private void MarkClickedPawn(Enums.EPlayer ePlayer, Vector2Int clickedCellCoord) // ������ ���� ������ pawn�� ��ġ�� ǥ��
    {
        Color previewColor = GetPreviewColor(ePlayer);
        for (int i = 0; i < _possiblePawnList.Count; i++)
        {
            Vector2Int coord = _possiblePawnList[i];
            Cell cell = GetCell(coord.x, coord.y);
            cell.SetClickablePawn(true, previewColor);
        }

        
        Cell clickedCell = GetCell(clickedCellCoord.x, clickedCellCoord.y);
        Color clickedColor = GetClickedColor(ePlayer);
        clickedCell.SetClickablePawn(true, clickedColor);

    }

    #region Updates Infos and Board
    public void Refresh() // ���忡 �ʿ��� info�� ������Ʈ�ϰ� ���忡 ǥ��
    {
        ClearBoard();

        UpdatePawnCoordination();
        UpdatePlanks();
        UpdateMoveablePawn();
        UpdatePlaceablePlanks();
        UpdateRemainPlankNum();
    }
    private void UpdatePawnCoordination()
     {
        _p1Coordinate = _gameLogic.GetPawnCoordinate(Enums.EPlayer.Player1);
        _p2Coordinate = _gameLogic.GetPawnCoordinate(Enums.EPlayer.Player2);

        SetPawn(Enums.EPlayer.Player1, _p1Coordinate);
        SetPawn(Enums.EPlayer.Player2, _p2Coordinate);
     }
    private void UpdatePlanks()
     {
        _planks = _gameLogic.planks;

        foreach(Plank targetPlank in _planks)
        {
            SetPlank(targetPlank.GetCoordinate(), targetPlank.GetDirection(), true, Color.black);
        }
     }
    private void UpdateMoveablePawn()
     {
        _possiblePawnList = _gameLogic.GetMoveablePawnCoords(_gameLogic.turn);
     }
    private void UpdatePlaceablePlanks()
     {
        _placeableHorizontalPlanks = _gameLogic.GetPlaceablePlankCoords(EDirection.Horizontal);
        _placeableVerticalPlanks = _gameLogic.GetPlaceablePlankCoords(EDirection.Vertical);
     }
    private void UpdateRemainPlankNum()
     {
        int p1PlankNum = _gameLogic.GetRemainPlank(Enums.EPlayer.Player1);
        int p2PlankNum = _gameLogic.GetRemainPlank(Enums.EPlayer.Player2);

        _p1RemainPlank.GetComponent<RemainPlank>().DisplayRemainPlank(p1PlankNum);
        _p2RemainPlank.GetComponent<RemainPlank>().DisplayRemainPlank(p2PlankNum);
    }
    private void ClearBoard() // ���� ǥ�� �� �����
     {
        for (int row = 0; row < ROW; row++)
        {
            for (int col = 0; col < COL; col++)
            {
                Cell targetCell = GetCell(col, row);
                targetCell.ClearCell();
            }
        }
    }
    #endregion
    private void ResetStates()
    {
        _isPreviewPlank = false;
    }
}