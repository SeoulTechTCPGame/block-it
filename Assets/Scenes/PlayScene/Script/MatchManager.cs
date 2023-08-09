using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MatchManager : MonoBehaviour
{
    public GameObject P1Buttons;
    public GameObject P2Buttons;
    public GameObject P1Plank;
    public GameObject P2Plank;
    public GameObject WinState;
    public Vector2Int RequestedPawnCoord;
    public Plank RequestedPlank = new Plank();

    private Enums.EPlayer _turn;
    private GameLogic _gameLogic;
    private bool _bUpdatePawnCoord = false;
    private bool _bUpdatePlank = false;

    public static UnityEvent ToNextTurn;
    public static UnityEvent ResetMove;
    public static UnityEvent<Vector2Int> SetRequestedPawnCoord = new UnityEvent<Vector2Int>();
    public static UnityEvent<Vector2Int> SetRequestedPlank= new UnityEvent<Vector2Int>();

    private List<Vector2Int> placeableVerticalPlanks = new List<Vector2Int>();
    private List<Vector2Int> placeableHorizontalPlanks = new List<Vector2Int>();

    void Awake()
    {
        ToNextTurn = new UnityEvent();
        ToNextTurn.AddListener(nextTurn);

        ResetMove= new UnityEvent();
        ResetMove.AddListener(resetMove);

        SetRequestedPawnCoord = new UnityEvent<Vector2Int>();
        SetRequestedPawnCoord.AddListener(updateRequestedPawnCoord);

        SetRequestedPlank = new UnityEvent<Vector2Int>();
        SetRequestedPlank.AddListener((coord)=>updateRequestedPlank(coord));

        _gameLogic = FindObjectOfType<GameLogic>();

        setButtonsOwner();
    }
    // Start is called before the first frame update
    void Start()
    {
        setTurn(Enums.EPlayer.Player1);
    }

    private void setButtonsOwner()
    {
        P1Buttons.GetComponent<PlayerButtons>().SetOwner(Enums.EPlayer.Player1);
        P2Buttons.GetComponent<PlayerButtons>().SetOwner(Enums.EPlayer.Player2);
    }

    private void nextTurn() 
    {
        if (_turn == Enums.EPlayer.Player1)
        {
            setTurn(Enums.EPlayer.Player2);
        }
        else
        {
            setTurn(Enums.EPlayer.Player1);
        }
        BoardManager.UpdateBoard.Invoke();

    }

    private void setTurn(Enums.EPlayer ePlayer)
    {
        _gameLogic.turn = ePlayer;
        // set target and other player.
        Enums.EPlayer otherPlayer = (ePlayer == Enums.EPlayer.Player1) ? Enums.EPlayer.Player2 : Enums.EPlayer.Player1;

        // get Buttons and distinguish which one is ePlayer one and other player one.
        GameObject theButton = (ePlayer == Enums.EPlayer.Player1) ? P1Buttons : P2Buttons;
        GameObject otherButton = (ePlayer == Enums.EPlayer.Player1) ? P2Buttons : P1Buttons;

        // set Put Button on the board - the target Player's put button will be activated while the other won't be.
        theButton.GetComponent<PlayerButtons>().SetButtons(true);
        otherButton.GetComponent<PlayerButtons>().SetButtons(false);

        // if the last turn has certain changes, apply on GameLogic.
        if (_bUpdatePawnCoord == true)
        {
            _gameLogic.SetPawnPlace(otherPlayer, RequestedPawnCoord);
        }
        if(_bUpdatePlank == true)
        {
            Plank newPlank = new Plank();
            newPlank.SetPlank(RequestedPlank.GetCoordinate(), RequestedPlank.GetDirection());
            _gameLogic.SetPlank(newPlank);
            _gameLogic.GetTargetPawn(otherPlayer).UsePlank();
        }

        // change turn and reset the value
        _turn = ePlayer;
        _bUpdatePawnCoord = false;
        _bUpdatePlank = false;

        // Set Moveable Coord for pawn on the board
        List<Vector2Int> moveableCoord = _gameLogic.GetMoveablePawnCoords(ePlayer);
        BoardManager.UpdateBoard.Invoke();
        BoardManager.ResetState.Invoke();

        checkDisplayWin();
    }

    private void updateRequestedPawnCoord(Vector2Int coord)
    {
        RequestedPawnCoord = coord;
        _bUpdatePawnCoord = true;
        _bUpdatePlank = false;

        BoardManager.RemovePreviewPlank.Invoke();
        BoardManager.UpdateClickedPawn.Invoke(_turn, coord);

        enablePlayerPut(true);
    }

    private void updateRequestedPlank(Vector2Int coord)
    {
        GameObject targetButton = (_turn == Enums.EPlayer.Player1) ? P1Buttons : P2Buttons;

        EPlankImgState plankState = targetButton.GetComponent<PlayerButtons>().GetPlankState();

        if(plankState == EPlankImgState.Normal)
        {
            Debug.LogError("Invalid Plank Direction.");
            return;
        }

        EDirection eDirection = (plankState == EPlankImgState.Horizontal) ? EDirection.Horizontal : EDirection.Vertical;

        RequestedPlank.SetPlank(coord, eDirection);
        BoardManager.PlacePreviewPlank.Invoke(coord, eDirection, _turn);

        _bUpdatePawnCoord = false;
        _bUpdatePlank = true;

        enablePlayerPut(true);
    }


    private void resetMove()
    {
        _bUpdatePawnCoord = false;
        _bUpdatePlank = false;

        // disable player's put button
        enablePlayerPut(false);
    }

    private void checkDisplayWin()
    {
        if( _gameLogic.Wins(Enums.EPlayer.Player1) || _gameLogic.Wins(Enums.EPlayer.Player2))
        {
            P1Buttons.GetComponent<PlayerButtons>().DisableButtons();
            P2Buttons.GetComponent<PlayerButtons>().DisableButtons();

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

    private bool isNextTurnAvaible() 
    { 
        if(_bUpdatePawnCoord || _bUpdatePlank)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void enablePlayerPut(bool bOn)
    {
        GameObject targetButton = getCurrentPlayerButton();
        targetButton.GetComponent<PlayerButtons>().SetPutButtonInteractable(bOn);
    }

    private GameObject getCurrentPlayerButton()
    {
        if(_turn == Enums.EPlayer.Player1)
        {
            return P1Buttons;
        }
        else
        {
            return P2Buttons;
        }
    }
}