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
    public Vector2Int RequestedPawnCoord;

    private EPlayer _turn;
    private GameLogic _gameLogic;
    private bool _bUpdatePawnCoord = false;

    public static UnityEvent ToNextTurn;
    public static UnityEvent<Vector2Int> SetRequestedPawnCoord = new UnityEvent<Vector2Int>();


    void Awake()
    {
        ToNextTurn = new UnityEvent();
        ToNextTurn.AddListener(nextTurn);

        SetRequestedPawnCoord = new UnityEvent<Vector2Int>();
        SetRequestedPawnCoord.AddListener(updateRequestedPawnCoord);

        _gameLogic = FindObjectOfType<GameLogic>();
    }
    // Start is called before the first frame update
    void Start()
    {
        setTurn(EPlayer.Player1);
    }

    private void nextTurn() 
    {
        if (_turn == EPlayer.Player1)
        {
            setTurn(EPlayer.Player2);
        }
        else
        {
            setTurn(EPlayer.Player1);
        }

    }

    private void setTurn(EPlayer ePlayer)
    {

        // set target and other player.
        EPlayer otherPlayer = EPlayer.Player1;
        if(ePlayer == EPlayer.Player1)
        {
            otherPlayer = EPlayer.Player2;
        }

        // get Buttons and distinguish which one is ePlayer one and other player one.
        GameObject theButton = P1Buttons;
        GameObject otherButton = P2Buttons;
        if (ePlayer == EPlayer.Player2)
        {
            theButton = P2Buttons;
            otherButton = P1Buttons;
        }
        // set Put Button on the board - the target Player's put button will be activated while the other won't be.
        theButton.GetComponent<PlayerButtons>().SetButtons(true);
        otherButton.GetComponent<PlayerButtons>().SetButtons(false);


        // if the last turn has certain changes, apply on GameLogic.
        if (_bUpdatePawnCoord == true)
        {
            _gameLogic.SetPawnPlace(otherPlayer, RequestedPawnCoord);
        }

        // change turn and reset the value
        _turn = ePlayer;
        _bUpdatePawnCoord = false;

        // Update one Board: MoveablePawn, Pawns' Coord, & MoveableCoord
        BoardManager.SetPawnCoord.Invoke(ePlayer, _gameLogic.GetPawnCoordinate(ePlayer));
        BoardManager.SetPawnCoord.Invoke(otherPlayer, _gameLogic.GetPawnCoordinate(otherPlayer));

        // Set Moveable Coord for pawn on the board.
        List<Vector2Int> moveableCoord = _gameLogic.GetMoveablePawnCoords(ePlayer);
        BoardManager.UpdateMoveablePawns.Invoke(moveableCoord);
    }

    private void updateRequestedPawnCoord(Vector2Int coord)
    {
        RequestedPawnCoord = coord;
        _bUpdatePawnCoord = true;
        _bUpdatePlank = false;

        BoardManager.UpdateClickedPawn.Invoke(_turn, coord);
    }
    }
}
