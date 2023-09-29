using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/*
 * MatchManager: 
 *   - GameLogic?? UI?? ??????????. UI?? ?????? GameLogic?? ????????, GameLogic?? ?????? UI?? ????????.
 *     - ?????????? 1. ??(Move) ???? 2. ?? ?????? ????.
 *   - PlayerButtons, Winstate ????
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

    private float currentTime = 60f;
    public float maxTime = 60f;
    private bool isTimerRunning = true;

    private Enums.EMode _gameMode;
    private Enums.EPlayer _Turn; // ???? ???? ????????
    public Vector2Int RequestedPawnCoord; //???? ???? ???? ???? pawn?? ???? ????
    public Plank RequestedPlank = new Plank(); //???? ???? ???? ?????? plank
    private bool _isUpdatePawnCoord = false; // ???? ???? ???? pawn?? ????????????
    private bool _isUpdatePlank = false; // ???? ???? ???? plank?? ??????????
    private GameLogic _gameLogic;

    private Enums.EPlayer _user;

    #region Events
    public static UnityEvent ToNextTurn; // ?????????? ??????
    public static UnityEvent ResetMove; // ???? ???? ???? ????????.
    public static UnityEvent<Vector2Int> SetRequestedPawnCoord = new UnityEvent<Vector2Int>(); // ???? ???? ???? ???? pawn ???? ???? ????????
    public static UnityEvent<Vector2Int> SetRequestedPlank= new UnityEvent<Vector2Int>(); // ???? ???? ???? ???? plank ???? ????????
    #endregion

    void Awake() // ?????? ????, PlayerButton?? ?????? ????, _gameLogic ????
    {
        SetEvents();
        _gameLogic = FindObjectOfType<GameLogic>();
    }
 
    void Start() // ??????, Player 1?? ?????? ????????.
    {
        InitGameMode(Enums.EMode.Friend);
    }

    void Update()
    {
        if (isTimerRunning)
        {
            currentTime -= Time.deltaTime;
            UpdateTimer();
            if (currentTime <= 0)
            {
                // Timer has reached its maximum time
                isTimerRunning = false;

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
        if(_Turn == Enums.EPlayer.Player1)
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
        P2Timer.GetComponent<Timer>().SetCurrentTime(currentTime);
        P1Timer.GetComponent<Timer>().SetCurrentTime(currentTime);
    }

    private void SetButtonsOwner() // PlayButton???? ?????? ????
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

    private void NextTurn() // ?????????? ????
    {
        if (_Turn == Enums.EPlayer.Player1)
        {
            SetTurn(Enums.EPlayer.Player2);
        }
        else
        {
            SetTurn(Enums.EPlayer.Player1);
        }
        BoardManager.UpdateBoard.Invoke();

    }

    private void SetTurn(Enums.EPlayer ePlayer) // ?? ????. ???? ?????? ???? Turn, _placeableVerticalPlanks, _placeableHorizontalPlanks ?? ????????, PlayerButton, WinState ??????/???????? ????
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
        _Turn = ePlayer;
        _isUpdatePawnCoord = false;
        _isUpdatePlank = false;

        // Set Timer
        SwitchTimer();
        currentTime = maxTime;
        isTimerRunning = true;


        // Set Moveable Coord for pawn on the board
        List<Vector2Int> moveableCoord = _gameLogic.GetMoveablePawnCoords(ePlayer);
        BoardManager.UpdateBoard.Invoke();
        BoardManager.ResetState.Invoke();

        CheckWinAndDisplay();
    }

    private void EnablePlayerPut(bool bOn) // PlayerButtons?? put???? ??????, ????????
    {
        GameObject targetButton = GetCurrentPlayerButton();
        targetButton.GetComponent<PlayerButtons>().SetPutButtonInteractable(bOn);
    }

    private void UpdateRequestedPawnCoord(Vector2Int coord) // ???? ???? ???? ???? pawn ???? ???? ????????
    {
        RequestedPawnCoord = coord;
        _isUpdatePawnCoord = true;
        _isUpdatePlank = false;

        BoardManager.RemovePreviewPlank.Invoke();
        BoardManager.UpdateClickedPawn.Invoke(_Turn, coord);

        EnablePlayerPut(true);
    }

    private void UpdateRequestedPlank(Vector2Int coord) // ???? ???? ???? ???? plank ????????
    {
        GameObject targetButton = (_Turn == _user) ? LowerButtons : UpperButtons;

        EPlankImgState plankState = targetButton.GetComponent<PlayerButtons>().GetPlankState();

        if(plankState == EPlankImgState.Normal)
        {
            Debug.LogError("Invalid Plank Direction.");
            return;
        }

        EDirection eDirection = (plankState == EPlankImgState.Horizontal) ? EDirection.Horizontal : EDirection.Vertical;

        RequestedPlank.SetPlank(coord, eDirection);
        BoardManager.PlacePreviewPlank.Invoke(coord, eDirection, _Turn);

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

    private void CheckWinAndDisplay() // ???????? ??????, ?????????? ??????. ???????? ?????????? WinState???? ??, ???????? ??????/????????
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

    private bool IsNextTurnAvaible() // return ?????????? ???? ?? ?? ??????
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

    private GameObject GetCurrentPlayerButton() // returns ???? ?? ?????? PlayerButton
    {
        if(_Turn == _user)
        {
            return LowerButtons;
        }
        else
        {
            return UpperButtons;
        }
    }
}