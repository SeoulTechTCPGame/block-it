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

    void Awake()
    {
        toNextTurn = new UnityEvent();
        toNextTurn.AddListener(nextTurn);
    }

    // Start is called before the first frame update
    void Start()
    {
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
            P1Buttons.GetComponent<PlayerButtons>().SetButtons(true);
            P2Buttons.GetComponent<PlayerButtons>().SetButtons(false);

            turn = EPlayer.Player1;
        }
        else
        {
            P1Buttons.GetComponent<PlayerButtons>().SetButtons(false);
            P2Buttons.GetComponent<PlayerButtons>().SetButtons(true);

            turn = EPlayer.Player2;
        }
    }


}
