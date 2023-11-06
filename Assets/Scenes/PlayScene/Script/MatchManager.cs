using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using static Enums;

/*
 * MatchManager: 
 *   - GameLogic과 UI의 인터페이스. UI의 인풋을 GameLogic에 적용하고, GameLogic의 변화를 UI에 적용한다.
 *     - 로직적으로 1. 수(Move) 놓기 2. 턴 세팅을 한다.
 *   - PlayerButtons, Winstate 제어
 */
public class MatchManager : MonoBehaviour
{
    #region GameObjects
    public GameObject Board; // 보드판
    public GameObject LowerButtons; // 보드판 밑의 pawn, Plank, Put 버튼들
    public GameObject UpperButtons; // 보드판 위의 pawn, Plank, Put 버튼들
    public GameObject WinState; // 승리 혹은 패배시 뜨는 화면
    public GameObject LowerTimer; // 보드판 밑의 Timer
    public GameObject UpperTimer; // 보드판 아래의 Timer
    public GameObject MyProfile; // 플레이어(유저)의 Profile
    public GameObject TheirProfile; // 상대 플레이어의 Profile
    public GameObject MyEmotes; // 플레이어의 감정표현 버튼 밑 패널
    public GameObject TheirEmotePanel; // 상대 플레이어의 감정표현 버튼 밑 패널
    public GameObject ReplayButton; // 복기시, 필요한 버튼
    public GameObject ReStartButton; // 다시 시작
    public GameObject ToHomeButton; // 홈으로 버튼 (게임 끝난 후 나타나는 버튼)
    public GameObject ExpelButton; // 홈으로 버튼 (게임 끝난 후 나타나는 버튼)
    #endregion


    private float _currentTime = Constants.TURN_TIME;
    public float maxTime = Constants.TURN_TIME;
    private bool _isTimerRunning = false;
    private EMode _gameMode;
    private EPlayer _turn;// 현재 턴인 플레이어

    public Vector2Int RequestedPawnCoord;  //이번 턴의 수로 놓을 pawn의 이동 좌표
    public Plank RequestedPlank = new Plank(); //이번 턴의 수로 설치할 plank
    private bool _isUpdatePawnCoord = false;// 이번 턴의 수로 pawn을 이동시켰는가
    private bool _isUpdatePlank = false; // 이번 턴의 수로 plank를 설치했는가
    private GameLogic _gameLogic;

    private EPlayer _user;

    #region Events
    public static UnityEvent ToNextTurn; // 다음턴으로 넘긴다
    public static UnityEvent ResetMove; // 이번 턴의 수를 리셋한다.
    public static UnityEvent<Vector2Int> SetRequestedPawnCoord = new UnityEvent<Vector2Int>(); // 이번 턴의 수로 놓을 pawn 이동 위치 업데이트
    public static UnityEvent<Vector2Int> SetRequestedPlank = new UnityEvent<Vector2Int>(); // 이번 턴의 수로 놓을 plank 위치 업데이트
    public static UnityEvent<int> ShowRecord = new UnityEvent<int>(); // 이번 턴의 수로 놓을 plank 위치 업데이트
    #endregion

   private void Awake() // 이벤트 할당, PlayerButton의 사용자 할당, _gameLogic 받기
    {
        Debug.Log("MatchManager Awake");

        // check if public GameObjects are null
        if (IsGameObjectsNull() == false)
        {
            Debug.LogError("MatchManager - Awake: One of the public GameObjects is null.");
            SceneManager.LoadScene("Home");
            return;
        }
        SetEvents();
        
        // Get GameLogic
        _gameLogic = FindObjectOfType<GameLogic>();
        if(_gameLogic== null)
        {
            _gameLogic = new GameLogic();
            Debug.LogError("MatchManager - Awake: GameLogic is null. - created new one");
        }
    }

   private void Start() // 시작시, Player 1의 턴으로 세팅한다.
    {
        Debug.Log("MatchManager Start");

        _gameMode = (EMode)PlayerPrefs.GetInt("GameMode", (int)EMode.Local); ;
        _gameLogic.AddMoveRecord();

        InitGameMode(_gameMode);
        
        ReplayButton.SetActive(false);
        ReStartButton.SetActive(false);
        ToHomeButton.SetActive(false);
        ExpelButton.SetActive(false);
    }

   private void Update()
    {
        if (_isTimerRunning)
        {
            _currentTime -= Time.deltaTime;
            UpdateTimer();
            if (_currentTime <= 0)
            {
                // Timer has reached its maximum time
                _isTimerRunning = false;
                if(_turn != _user)
                {
                    ExpelButton.SetActive(true);
                }
            }
        }
        else
        { 
            LowerTimer.SetActive(false);
            UpperTimer.SetActive(false);
        }
    }

    // 게임 모드에 따라 Init
    #region InitGameModes 
    private void InitGameMode(EMode gameMode)
    {
        switch(gameMode)
        {
            case EMode.Local:
                InitFriendMode();
                break;
            case EMode.AI:
                InitAiMode();
                break;
            case EMode.MultiWifi:
                InitWifiMode();
                break;
        }

    }

    private void InitFriendMode()
    {
        _user = EPlayer.Player1;
        SetButtonsOwner();
        SetTurn(EPlayer.Player1);

        OrientBoard();

        _isTimerRunning = false;

        // Hide Objects
        UpperTimer.SetActive(false);
        LowerTimer.SetActive(false);

        // Hide Objects
        MyProfile.SetActive(false);
        TheirProfile.SetActive(false);
        MyEmotes.SetActive(false);
        TheirEmotePanel.SetActive(false);
    }

    private void InitWifiMode() // 유저가 Player1인지 Player2인지 설정 필요
    {
        // Set _user
        //_user = getUserTurn();
        _user = EPlayer.Player1;
        SetTurn(EPlayer.Player1);

        _isTimerRunning = true;

        OrientBoard();

        MyProfile.GetComponent<ProfilePlayscene>().SetPlayerProfile(true);
        TheirProfile.GetComponent<ProfilePlayscene>().SetPlayerProfile(true);

        UpperTimer.GetComponent<Timer>().RotateTimer();
    }

    private void InitAiMode() // 유저가 Player1인지 Player2인지 설정 필요
    {
        // Set _user
        //_user = getUserTurn();
        _user = EPlayer.Player1;
        SetTurn(EPlayer.Player1);

        _isTimerRunning = false;

        UpperTimer.SetActive(false);
        LowerTimer.SetActive(false);

        OrientBoard();

        // Hide Objects
        MyProfile.SetActive(false);
        TheirProfile.SetActive(false);
        MyEmotes.SetActive(false);
        TheirEmotePanel.SetActive(false);

        TheirProfile.GetComponent<ProfilePlayscene>().SetAiProfile();

        UpperTimer.GetComponent<Timer>().RotateTimer();

    }
    #endregion

    private bool IsGameObjectsNull()
    {
        // Check if public GameObjects are null
        if (Board == null || LowerButtons == null || UpperButtons == null || WinState == null || LowerTimer == null || UpperTimer == null || MyProfile == null || TheirProfile == null || MyEmotes == null || TheirEmotePanel == null || ReplayButton == null || ReStartButton == null || ToHomeButton == null || ExpelButton == null)
        {
            Debug.LogError("MatchManager - IsGameObjectsNull: One of the public GameObjects is null.");
            return false;
        }
        return true;
    }
    private void OrientBoard() // 유저가 Player2인경우 보드판을 뒤집는다.
    {
        if(_user == EPlayer.Player2)
        {
            Board.transform.Rotate(0,0,180);
        }
    }
    private void SetEvents()
    {
        ToNextTurn = new UnityEvent();
        ToNextTurn.AddListener(NextTurn);

        ResetMove = new UnityEvent();
        ResetMove.AddListener(ResetIsUpdate);

        SetRequestedPawnCoord = new UnityEvent<Vector2Int>();
        SetRequestedPawnCoord.AddListener(UpdateRequestedPawnCoord);

        SetRequestedPlank = new UnityEvent<Vector2Int>();
        SetRequestedPlank.AddListener((coord) => UpdateRequestedPlank(coord));

        ShowRecord = new UnityEvent<int>();
        ShowRecord.AddListener((nthMove) => ShowReplay(nthMove) );
    }

    private void SwitchTimer() // 현재 턴 플레이어의 타이머가 켜진다. (현재 턴이 아닌 플레이어의 타이머가 꺼진다)
    {
        if(_turn == EPlayer.Player1)
        {
            LowerTimer.GetComponent<Timer>().ShowTimer();
            UpperTimer.GetComponent<Timer>().HideTimer();
        }
        else
        {
            UpperTimer.GetComponent<Timer>().ShowTimer();
            LowerTimer.GetComponent<Timer>().HideTimer();
        }
    }
    
    private void UpdateTimer() // 타이머를 업데이트한다.
    {
        UpperTimer.GetComponent<Timer>().SetCurrentTime(_currentTime);
        LowerTimer.GetComponent<Timer>().SetCurrentTime(_currentTime);
    }

    private void SetButtonsOwner() // PlayButton들의 사용자 할당
    {
        LowerButtons.GetComponent<PlayerButtons>().SetOwner(_user);

        if(_user == EPlayer.Player1)
        {
            UpperButtons.GetComponent<PlayerButtons>().SetOwner(EPlayer.Player2);
        }
        else
        {
            UpperButtons.GetComponent<PlayerButtons>().SetOwner(EPlayer.Player1);
        }
    }

    private void NextTurn() // 다음턴으로 넘김
    {
        if (_turn == EPlayer.Player1)
        {
            SetTurn(EPlayer.Player2);
        }
        else
        {
            SetTurn(EPlayer.Player1);
        }
        _gameLogic.AddMoveRecord();
        BoardManager.UpdateBoard.Invoke();

    }

    private void SetTurn(EPlayer ePlayer) // 턴 세팅. 턴이 바뀜에 따라 turn, _placeableVerticalPlanks, _placeableHorizontalPlanks 값 업데이트, PlayerButton, WinState 활성화/비활성화 제어
    {
        ExpelButton.SetActive(false);

        _gameLogic.Turn = ePlayer;
        // set target and other player.
        EPlayer otherPlayer = (ePlayer == EPlayer.Player1) ? EPlayer.Player2 : EPlayer.Player1;

        // get Buttons and distinguish which one is ePlayer one and other player one.
        GameObject theButton = (ePlayer == _user) ? LowerButtons : UpperButtons;
        GameObject otherButton = (ePlayer == _user) ? UpperButtons : LowerButtons;

        // set Put Button on the board - the target Player's put button will be activated while the other won't be.
        if(theButton != null) theButton.GetComponent<PlayerButtons>().SetButtons(true);
        if (otherButton != null) otherButton.GetComponent<PlayerButtons>().SetButtons(false);

        // if the last Turn has certain changes, apply on GameLogic.
        if (_isUpdatePawnCoord == true)
        {
            _gameLogic.SetPawnPlace(otherPlayer, RequestedPawnCoord);
        }
        if(_isUpdatePlank == true)
        {
            Plank newPlank = new Plank();
            newPlank.SetPlank(RequestedPlank.GetCoordinate(), RequestedPlank.GetDirection());
            _gameLogic.SetPlank(newPlank);
            _gameLogic.GetTargetPawn(otherPlayer).UsePlank();
        }

        // change Turn and reset the value
        _turn = ePlayer;
        _isUpdatePawnCoord = false;
        _isUpdatePlank = false;

        // Set Timer
        if(_gameMode == EMode.MultiWifi)
        {
            SwitchTimer();
            _currentTime = Constants.TURN_TIME;
            _isTimerRunning = true;
        }


        // Set Moveable Coord for pawn on the board
        List<Vector2Int> moveableCoord = _gameLogic.GetMoveablePawnCoords(ePlayer);
        BoardManager.UpdateBoard.Invoke();
        BoardManager.ResetState.Invoke();

        CheckWinAndDisplay();
    }

    private void EnablePlayerPut(bool bOn) // PlayerButtons의 put버튼 활성화, 비활성화
    {
        GameObject targetButton = GetCurrentPlayerButton();
        targetButton.GetComponent<PlayerButtons>().SetPutButtonInteractable(bOn);
    }

    private void UpdateRequestedPawnCoord(Vector2Int coord) // 이번 턴의 수로 놓을 pawn 이동 위치 업데이트
    {
        RequestedPawnCoord = coord;
        _isUpdatePawnCoord = true;
        _isUpdatePlank = false;

        BoardManager.RemovePreviewPlank.Invoke();
        BoardManager.UpdateClickedPawn.Invoke(_turn, coord);

        EnablePlayerPut(true);
    }

    private void UpdateRequestedPlank(Vector2Int coord) // 이번 턴의 수로 놓을 plank 업데이트
    {
        GameObject targetButton = (_turn == _user) ? LowerButtons : UpperButtons;

        EPlankImgState plankState = targetButton.GetComponent<PlayerButtons>().GetPlankState();

        if(plankState == EPlankImgState.Normal)
        {
            Debug.LogError("Invalid Plank Direction.");
            return;
        }

        EDirection eDirection = (plankState == EPlankImgState.Horizontal) ? EDirection.Horizontal : EDirection.Vertical;

        RequestedPlank.SetPlank(coord, eDirection);
        BoardManager.PlacePreviewPlank.Invoke(coord, eDirection, _turn);

        _isUpdatePawnCoord = false;
        _isUpdatePlank = true;

        EnablePlayerPut(true);
    }

    private void ResetIsUpdate()
    {
        _isUpdatePawnCoord = false;
        _isUpdatePlank = false;

        // disable player's put button
        EnablePlayerPut(false);
    }

    private void CheckWinAndDisplay() // 플레이어 승리시, 승리화면을 띄운다. 참가하는 플레이어의 WinState확인 후, 승리화면 활성화/비활성화
    {
        if( _gameLogic.Wins(EPlayer.Player1) || _gameLogic.Wins(EPlayer.Player2))
        {
            LowerButtons.GetComponent<PlayerButtons>().DisableButtons();
            UpperButtons.GetComponent<PlayerButtons>().DisableButtons();

            _isTimerRunning = false;

            // Determine the winning player
            EPlayer winningPlayer = _gameLogic.Wins(EPlayer.Player1) ? EPlayer.Player1 : EPlayer.Player2;
            bool userWins = _gameLogic.Wins(_user);

            switch (_gameMode)
            {
                case EMode.Local:
                    //Print Win Lose Message
                    WinState.GetComponent<WinState>().DisplayWin(winningPlayer);
                    // Pops Up ReStart Button
                    ReStartButton.gameObject.SetActive(true);
                    break;
                case EMode.AI:
                    // Display Win/Lose message based on the winning player
                    WinState.GetComponent<WinState>().DisplayWinLose(userWins);
                    break;
                case EMode.MultiWifi:
                    // Display Win/Lose message based on the winning player
                    WinState.GetComponent<WinState>().DisplayWinLose(userWins);
                    // Pops Up RePlay Button
                    ReplayButton.gameObject.SetActive(false);
                    ReplayButton.GetComponent<ReplayButton>().SetMaxIndex(_gameLogic.Moves.Count);
                    ReplayButton.GetComponent<ReplayButton>().SetButton(false, 0);
                    break;
            }
            ToHomeButton.gameObject.SetActive(true);
        }
    }

    private bool IsNextTurnAvaible() // return 다음턴으로 넘어 갈 수 있는지
    { 
        if(_isUpdatePawnCoord || _isUpdatePlank)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private GameObject GetCurrentPlayerButton() // returns 현재 턴 유저의 PlayerButton
    {
        if(_turn == _user)
        {
            return LowerButtons;
        }
        else
        {
            return UpperButtons;
        }
    }

    private void ShowReplay(int nthMove)
    {
        if(nthMove < 0 || _gameLogic.Moves.Count <= nthMove)
        {
            Debug.LogError("MatchManager - ShowReplay: Invalid nthMove = "+ nthMove);
            return;
        }

        BoardManager.ShowReplay.Invoke(nthMove);
    }
    public void Restart()
    {
        _gameLogic.Reset();
        _isTimerRunning = true;
        _gameMode = (EMode)PlayerPrefs.GetInt("GameMode", (int)EMode.Local); ;
        _gameLogic.AddMoveRecord();
        InitGameMode(_gameMode);
        ReplayButton.gameObject.SetActive(false);
        ReStartButton.gameObject.SetActive(false);
        ToHomeButton.gameObject.SetActive(false);
        ExpelButton.gameObject.SetActive(false);
    }
    public void ExpelOtherPlayer()
    {
        _gameLogic.IsExpeled = true;
        _gameLogic.IsGameOver = true;
        _gameLogic.Winner = (_user == EPlayer.Player1) ? EPlayer.Player1 : EPlayer.Player2;

        _isTimerRunning = false;

        UpperTimer.gameObject.SetActive(false);
        LowerTimer.gameObject.SetActive(false);

        LowerButtons.GetComponent<PlayerButtons>().DisableButtons();
        UpperButtons.GetComponent<PlayerButtons>().DisableButtons();

        // Display Win/Lose message based on the winning player
        WinState.GetComponent<WinState>().DisplayWinLose(true);

        ToHomeButton.gameObject.SetActive(true);
    }
    public void GetExpeled()
    {
        _gameLogic.IsExpeled = true;
        _gameLogic.IsGameOver = true;
        _gameLogic.Winner = (_user == EPlayer.Player1)? EPlayer.Player2:EPlayer.Player1;
        
        _isTimerRunning = false;

        UpperTimer.gameObject.SetActive(false);
        LowerTimer.gameObject.SetActive(false);

        LowerButtons.GetComponent<PlayerButtons>().DisableButtons();
        UpperButtons.GetComponent<PlayerButtons>().DisableButtons();

        // Display Win/Lose message based on the winning player
        WinState.GetComponent<WinState>().DisplayWinLose(false);

        ToHomeButton.gameObject.SetActive(true);
    }
}
