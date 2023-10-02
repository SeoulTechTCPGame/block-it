using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/*
 * MatchManager: 
 *   - GameLogic과 UI의 인터페이스. UI의 인풋을 GameLogic에 적용하고, GameLogic의 변화를 UI에 적용한다.
 *     - 로직적으로 1. 수(Move) 놓기 2. 턴 세팅을 한다.
 *   - PlayerButtons, Winstate 제어
 */
public class MatchManager : MonoBehaviour
{
    #region GameObjects
    public GameObject Board;
    public GameObject LowerButtons;
    public GameObject UpperButtons;
    public GameObject WinState;
    public GameObject P1Timer;
    public GameObject P2Timer;
    public GameObject MyProfile;
    public GameObject TheirProfile;
    public GameObject MyEmotes;
    public GameObject TheirEmotePanel;
    #endregion

    private float _currentTime = 60f;
    public float maxTime = 60f;
    private bool _isTimerRunning = true;

    private Enums.EMode _gameMode;
    private Enums.EPlayer _turn;// 현재 턴인 플레이어
    public Vector2Int RequestedPawnCoord;  //이번 턴의 수로 놓을 pawn의 이동 좌표
    public Plank RequestedPlank = new Plank(); //이번 턴의 수로 설치할 plank
    private bool _isUpdatePawnCoord = false;// 이번 턴의 수로 pawn을 이동시켰는가
    private bool _isUpdatePlank = false; // 이번 턴의 수로 plank를 설치했는가
    private GameLogic _gameLogic;

    private Enums.EPlayer _user;

    #region Events
    public static UnityEvent ToNextTurn; // 다음턴으로 넘긴다
    public static UnityEvent ResetMove; // 이번 턴의 수를 리셋한다.
    public static UnityEvent<Vector2Int> SetRequestedPawnCoord = new UnityEvent<Vector2Int>(); // 이번 턴의 수로 놓을 pawn 이동 위치 업데이트
    public static UnityEvent<Vector2Int> SetRequestedPlank = new UnityEvent<Vector2Int>(); // 이번 턴의 수로 놓을 plank 위치 업데이트
    #endregion

    void Awake() // 이벤트 할당, PlayerButton의 사용자 할당, _gameLogic 받기
    {
        SetEvents();
        _gameLogic = FindObjectOfType<GameLogic>();
    }

    void Start() // 시작시, Player 1의 턴으로 세팅한다.
    {
        _gameMode = (Enums.EMode)PlayerPrefs.GetInt("GameMode", (int)Enums.EMode.Friend); ;
        InitGameMode(_gameMode);
    }

    void Update()
    {
        if (isTimerRunning)
        {
            _currentTime -= Time.deltaTime;
            UpdateTimer();
            if (_currentTime <= 0)
            {
                // Timer has reached its maximum time
                _isTimerRunning = false;

                NextTurn();
            }
        }
    }

    private void InitGameMode(Enums.EMode gameMode)
    {
        switch(gameMode)
        {
            case Enums.EMode.Friend:
                InitFriendMode();
                break;
            case Enums.EMode.AI:
                InitAiMode();
                break;
            case Enums.EMode.MultiWifi:
                InitWifiMode();
                break;
        }

    }

    private void InitFriendMode()
    {
        _user = Enums.EPlayer.Player1;
        SetButtonsOwner();
        SetTurn(Enums.EPlayer.Player1);

        OrientBoard();

        Destroy(MyProfile);
        Destroy(TheirProfile);
        Destroy(MyEmotes);
        Destroy(TheirEmotePanel);
    }

    private void InitWifiMode()
    {
        // Set _user
        //_user = getUserTurn();
        _user = Enums.EPlayer.Player1;

        SetTurn(Enums.EPlayer.Player1);

        OrientBoard();
        MyProfile.GetComponent<ProfilePlayscene>().SetPlayerProfile(true);
        TheirProfile.GetComponent<ProfilePlayscene>().SetPlayerProfile(true);
    }

    private void InitAiMode()
    {
        // Set _user
        //_user = getUserTurn();
        _user = Enums.EPlayer.Player1;
        SetTurn(Enums.EPlayer.Player1);

        OrientBoard();

        Destroy(MyProfile);
        TheirProfile.GetComponent<ProfilePlayscene>().SetAiProfile();
    }

    private void OrientBoard()
    {
        if(_user == Enums.EPlayer.Player2)
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
    }

    private void SwitchTimer()
    {
        if(_turn == Enums.EPlayer.Player1)
        {
            P1Timer.GetComponent<Timer>().ShowTimer();
            P2Timer.GetComponent<Timer>().HideTimer();
        }
        else
        {
            P2Timer.GetComponent<Timer>().ShowTimer();
            P1Timer.GetComponent<Timer>().HideTimer();
        }
    }
    
    private void UpdateTimer()
    {
        P2Timer.GetComponent<Timer>().SetCurrentTime(_currentTime);
        P1Timer.GetComponent<Timer>().SetCurrentTime(_currentTime);
    }

    private void SetButtonsOwner() // PlayButton들의 사용자 할당
    {
        LowerButtons.GetComponent<PlayerButtons>().SetOwner(_user);

        if(_user == Enums.EPlayer.Player1)
        {
            UpperButtons.GetComponent<PlayerButtons>().SetOwner(Enums.EPlayer.Player2);
        }
        else
        {
            UpperButtons.GetComponent<PlayerButtons>().SetOwner(Enums.EPlayer.Player1);
        }
    }

    private void NextTurn() // 다음턴으로 넘김
    {
        if (_turn == Enums.EPlayer.Player1)
        {
            SetTurn(Enums.EPlayer.Player2);
        }
        else
        {
            SetTurn(Enums.EPlayer.Player1);
        }
        BoardManager.UpdateBoard.Invoke();

    }

    private void SetTurn(Enums.EPlayer ePlayer) // 턴 세팅. 턴이 바뀜에 따라 turn, _placeableVerticalPlanks, _placeableHorizontalPlanks 값 업데이트, PlayerButton, WinState 활성화/비활성화 제어
    {
        _gameLogic.Turn = ePlayer;
        // set target and other player.
        Enums.EPlayer otherPlayer = (ePlayer == Enums.EPlayer.Player1) ? Enums.EPlayer.Player2 : Enums.EPlayer.Player1;

        // get Buttons and distinguish which one is ePlayer one and other player one.
        GameObject theButton = (ePlayer == _user) ? LowerButtons : UpperButtons;
        GameObject otherButton = (ePlayer == _user) ? UpperButtons : LowerButtons;

        // set Put Button on the board - the target Player's put button will be activated while the other won't be.
        theButton.GetComponent<PlayerButtons>().SetButtons(true);
        otherButton.GetComponent<PlayerButtons>().SetButtons(false);

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
        SwitchTimer();
        _currentTime = maxTime;
        _isTimerRunning = true;


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
        if( _gameLogic.Wins(Enums.EPlayer.Player1) || _gameLogic.Wins(Enums.EPlayer.Player2))
        {
            LowerButtons.GetComponent<PlayerButtons>().DisableButtons();
            UpperButtons.GetComponent<PlayerButtons>().DisableButtons();

            if (_gameLogic.Wins(Enums.EPlayer.Player1))
            {
                WinState.GetComponent<WinState>().DisplayWin(Enums.EPlayer.Player1);
            }
            else
            {
                WinState.GetComponent<WinState>().DisplayWin(Enums.EPlayer.Player2);
            }
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
}