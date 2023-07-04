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

    private EPlayer turn;

    public static UnityEvent toNextTurn;
    public static UnityEvent<Vector2Int> setRequestedPawnCoord = new UnityEvent<Vector2Int>();


    private GameLogic gameLogic;

    public Vector2Int RequestedPawnCoord;
    private bool bUpdatePawnCoord = false;

    void Awake()
    {
        toNextTurn = new UnityEvent();
        toNextTurn.AddListener(nextTurn);

        setRequestedPawnCoord = new UnityEvent<Vector2Int>();
        setRequestedPawnCoord.AddListener(updateRequestedPawnCoord);

        gameLogic = FindObjectOfType<GameLogic>();
    }
    // Start is called before the first frame update
    void Start()
    {
        BoardManager.setPawnCoord.Invoke(EPlayer.Player1, gameLogic.GetPawnCoordinate(EPlayer.Player1));
        BoardManager.setPawnCoord.Invoke(EPlayer.Player2, gameLogic.GetPawnCoordinate(EPlayer.Player2));
        setTurn(EPlayer.Player1);
    }

    private void nextTurn() 
    {
        if (turn == EPlayer.Player1)
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
        if(ePlayer == EPlayer.Player1)
        {
            if(bUpdatePawnCoord == true)
            {
                gameLogic.SetPawnPlace(EPlayer.Player2, RequestedPawnCoord);
            }


            P1Buttons.GetComponent<PlayerButtons>().SetButtons(true);
            P2Buttons.GetComponent<PlayerButtons>().SetButtons(false);

            turn = EPlayer.Player1;
            bUpdatePawnCoord = false;

            List<Vector2Int> moveableCoord = gameLogic.GetMoveablePawnCoords(ePlayer);
            BoardManager.updateMoveablePawns.Invoke(moveableCoord);

            BoardManager.setPawnCoord.Invoke(EPlayer.Player2, gameLogic.GetPawnCoordinate(EPlayer.Player2));
            BoardManager.setPawnCoord.Invoke(EPlayer.Player1, gameLogic.GetPawnCoordinate(EPlayer.Player1));
        }
        else
        {
            if (bUpdatePawnCoord == true)
            {
                gameLogic.SetPawnPlace(EPlayer.Player1, RequestedPawnCoord);
            }

            P1Buttons.GetComponent<PlayerButtons>().SetButtons(false);
            P2Buttons.GetComponent<PlayerButtons>().SetButtons(true);

            turn = EPlayer.Player2;
            bUpdatePawnCoord = false;

            List<Vector2Int> moveableCoord = gameLogic.GetMoveablePawnCoords(ePlayer);
            BoardManager.updateMoveablePawns.Invoke(moveableCoord);
            
            BoardManager.setPawnCoord.Invoke(EPlayer.Player1, gameLogic.GetPawnCoordinate(EPlayer.Player1));
            BoardManager.setPawnCoord.Invoke(EPlayer.Player2, gameLogic.GetPawnCoordinate(EPlayer.Player2));

        }
    }

    private void updateRequestedPawnCoord(Vector2Int coord)
    {
        RequestedPawnCoord = coord;
        bUpdatePawnCoord = true;
    }
}
