using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;

/*
 'BoardManager'는 보드를 생성하고, 보드에 필요한 View&Control를 관리하는 클래스입니다.
   - BoardManager가 관리하는 오브젝트는 다음과 같습니다.
     - _cells: Cell을 배열로 가지고 있습니다.
       - 어떤 Cell에 Pawn을 표시할지 관리합니다.
       - 어떤 Cell의 어떤 Plank를 표시할지 관리합니다.
       - 어떤 Cell의 PlankDot을 표시할지 관리합니다.
       - 어떤 Cell의 ClickablePawn(PawnButton - pawn이 이동가능한 위치 - 버튼으로 클릭가능)을 표시할지 관리합니다.
     - _p1RemainPlank, _p2RemainPlank: 각 플레이어의 남은 플랭크를 표시합니다.

    - 매 턴마다 _gameLogic에서 정보를 가져와, 보드를 지우고 다시 그립니다.(보드 재생성 x, 표시된 것들)
    - 모든 매서드는 private임, Event를 통해 소통
 */
public class BoardManager : MonoBehaviour
{
    const int ROW = 9; // 세로
    const int COL = 9; // 가로

    #region SerializeFeild Prefabs
    [SerializeField] GameObject _cellPrefab;
    [SerializeField] GameObject _p1RemainPlank;
    [SerializeField] GameObject _p2RemainPlank;
    #endregion

    private GameLogic _gameLogic;

    #region info for display
    private Cell[,] _cells = new Cell[COL, ROW]; // cell들을 담는 배열
    private Vector2Int _p1Coordinate = new Vector2Int(); // p1의 좌표
    private Vector2Int _p2Coordinate = new Vector2Int(); // p2의 좌표
    private List<Plank> _planks= new List<Plank>(); // 표시될 plank들의 배열
    private List<Vector2Int> _possiblePawnList = new List<Vector2Int>(); // pawn이 이동가능한 좌표들 (CLickablePawn으로 만들 좌표 배열 
    private List<Vector2Int> _placeableVerticalPlanks = new List<Vector2Int>(); // plank를 세로로 놓을수 있는 plankDot의 좌표 배열
    private List<Vector2Int> _placeableHorizontalPlanks = new List<Vector2Int>(); // plank를 가로로 놓을수 있는 plankDot의 좌표 배열
    private Plank _previewPlank = new Plank(); // 미리보기 plank (유저가 선택한 plank, plankDot을 눌러 결정)
    bool _isPreviewPlank = false; // 미리보기 plank가 있는가 - 유저가 plank를 놓았는지 놓지 않았는지 알기위해 있음
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
    public static UnityEvent<Enums.EPlayer> ShowMoveablePawns = new UnityEvent<Enums.EPlayer>(); // 보드에 pawn을 이동 시킬 수 있는 Cell의 ClickablePawn을 활성화시켜 표시
    public static UnityEvent RemoveMoveablePawns = new UnityEvent(); // 보드에 pawn을 이동 시킬 수 있는 Cell의 ClickablePawn을 비활성화시켜 숨김 (유저가 눌러 결정한 pawn의 위치도 지워짐)
    public static UnityEvent<Enums.EPlayer, Vector2Int> UpdateClickedPawn= new UnityEvent<Enums.EPlayer, Vector2Int>(); // 유저가 눌러 결정한 pawn의 위치를 표시

    public static UnityEvent<EDirection, Enums.EPlayer> ShowPlaceablePlanks= new UnityEvent<EDirection, Enums.EPlayer>(); // 보드에 plank를 놓을 수 있는 Cell의 PlankDot을 활성화시켜 표시
    public static UnityEvent RemovePlaceablePlanks = new UnityEvent();// 보드에 plank를 놓을 수 있는 Cell의 PlankDot을 비활성화시켜 숨김
    public static UnityEvent<Vector2Int, EDirection, Enums.EPlayer> PlacePreviewPlank = new UnityEvent<Vector2Int, EDirection, Enums.EPlayer>();// 유저가 눌러 결정한 plank의 위치를 표시
    public static UnityEvent RemovePreviewPlank = new UnityEvent(); // 유저가 눌러 결정했던 plank를 지움

    public static UnityEvent UpdateBoard = new UnityEvent(); // 보드에 필요한 info를 업데이트하고 보드에 표시
    public static UnityEvent ResetState = new UnityEvent(); // 보드에 필요한 state(_isPreviewPlank) 리셋
    #endregion

    void Awake() // 이벤트 세팅, GameLogic 받기
    {
        SetEvents();
        _gameLogic = FindObjectOfType<GameLogic>();
    }

    // Start is called before the first frame update
    void Start() // 보드 만들기
    {
        StartCoroutine(InitializeBoard());
    }

    private void SetEvents() // 이벤트 세팅
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

    public Cell GetCell(int col, int row)//해당 좌표의 cell 가져오기
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

    private void UnmarkMoveablePawn() // 보드에 pawn을 이동 시킬 수 있는 Cell의 ClickablePawn을 비활성화시켜 숨김 (유저가 눌러 결정한 pawn의 위치도 지워짐)
    {
        foreach (Vector2Int coord in _possiblePawnList)
        {
            Cell targetCell = GetCell(coord.x, coord.y);
            targetCell.SetClickablePawn(false, DISAPBLED_COLOR);
        }
    }
    
    private void MarkMoveablePawn(Enums.EPlayer ePlayer) // 보드에 pawn을 이동 시킬 수 있는 Cell의 ClickablePawn을 활성화시켜 표시
    {
        Color previewColor = GetPreviewColor(ePlayer);

        foreach (Vector2Int coord in _possiblePawnList)
        {
            Cell targetCell = GetCell(coord.x, coord.y);
            targetCell.SetClickablePawn(true, previewColor);
        }
    }

    private void MarkPlaceablePlankDot(EDirection eDirection, Enums.EPlayer ePlayer) // 보드에 plank를 놓을 수 있는 Cell의 PlankDot을 활성화시켜 표시
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

    private void UnmarkPlaceablePlankDot()  // 유저가 눌러 결정했던 plank를 지움
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

    private void SetPawn(Enums.EPlayer ePlayer, Vector2Int coordinate) // pawn 표시
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

    private void SetPlank(Vector2Int coordinate, EDirection eDirection, bool visible, Color color ) // plank 표시
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

    private void MarkPreviewPlank(Vector2Int coordinate, EDirection eDirection, Enums.EPlayer ePlayer) // 유저가 눌러 결정한 plank의 위치를 표시
    {
        UnmarkPreviewPlank();
        _previewPlank.SetPlank(coordinate, eDirection);

        Color color= GetClickedColor(ePlayer);

        SetPlank(coordinate, eDirection, true, color);
        _isPreviewPlank = true;

    }

    private void UnmarkPreviewPlank() // 유저가 눌러 결정했던 plank를 지움
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

    private void MarkClickedPawn(Enums.EPlayer ePlayer, Vector2Int clickedCellCoord) // 유저가 눌러 결정한 pawn의 위치를 표시
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
    public void Refresh() // 보드에 필요한 info를 업데이트하고 보드에 표시
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
    private void ClearBoard() // 보드 표시 다 지우기
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