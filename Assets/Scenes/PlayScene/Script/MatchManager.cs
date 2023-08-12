using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/*
 * MatchManager: 
 *   - GameLogic�� UI�� �������̽�. UI�� ��ǲ�� GameLogic�� �����ϰ�, GameLogic�� ��ȭ�� UI�� �����Ѵ�.
 *     - ���������� 1. ��(Move) ���� 2. �� ������ �Ѵ�.
 *   - PlayerButtons, Winstate ����
 */
public class MatchManager : MonoBehaviour
{
    #region GameObjects
    public GameObject P1Buttons;
    public GameObject P2Buttons;
    public GameObject WinState;
    #endregion

    private Enums.EPlayer _turn; // ���� ���� �÷��̾�
    public Vector2Int RequestedPawnCoord; //�̹� ���� ���� ���� pawn�� �̵� ��ǥ
    public Plank RequestedPlank = new Plank(); //�̹� ���� ���� ��ġ�� plank
    private bool _isUpdatePawnCoord = false; // �̹� ���� ���� pawn�� �̵����״°�
    private bool _isUpdatePlank = false; // �̹� ���� ���� plank�� ��ġ�ߴ°�
    private GameLogic _gameLogic;

    #region Events
    public static UnityEvent ToNextTurn; // ���������� �ѱ��
    public static UnityEvent ResetMove; // �̹� ���� ���� �����Ѵ�.
    public static UnityEvent<Vector2Int> SetRequestedPawnCoord = new UnityEvent<Vector2Int>(); // �̹� ���� ���� ���� pawn �̵� ��ġ ������Ʈ
    public static UnityEvent<Vector2Int> SetRequestedPlank= new UnityEvent<Vector2Int>(); // �̹� ���� ���� ���� plank ��ġ ������Ʈ
    #endregion

    void Awake() // �̺�Ʈ �Ҵ�, PlayerButton�� ����� �Ҵ�, _gameLogic �ޱ�
    {
        ToNextTurn = new UnityEvent();
        ToNextTurn.AddListener(NextTurn);

        ResetMove= new UnityEvent();
        ResetMove.AddListener(ResetIsUpdate);

        SetRequestedPawnCoord = new UnityEvent<Vector2Int>();
        SetRequestedPawnCoord.AddListener(UpdateRequestedPawnCoord);

        SetRequestedPlank = new UnityEvent<Vector2Int>();
        SetRequestedPlank.AddListener((coord)=>UpdateRequestedPlank(coord));

        _gameLogic = FindObjectOfType<GameLogic>();

        SetButtonsOwner();
    }
 
    void Start() // ���۽�, Player 1�� ������ �����Ѵ�.
    {
        SetTurn(Enums.EPlayer.Player1);
    }

    private void SetButtonsOwner() // PlayButton���� ����� �Ҵ�
    {
        P1Buttons.GetComponent<PlayerButtons>().SetOwner(Enums.EPlayer.Player1);
        P2Buttons.GetComponent<PlayerButtons>().SetOwner(Enums.EPlayer.Player2);
    }

    private void NextTurn() // ���������� �ѱ�
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

    private void SetTurn(Enums.EPlayer ePlayer) // �� ����. ���� �ٲ� ���� turn, _placeableVerticalPlanks, _placeableHorizontalPlanks �� ������Ʈ, PlayerButton, WinState Ȱ��ȭ/��Ȱ��ȭ ����
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

        // change turn and reset the value
        _turn = ePlayer;
        _isUpdatePawnCoord = false;
        _isUpdatePlank = false;

        // Set Moveable Coord for pawn on the board
        List<Vector2Int> moveableCoord = _gameLogic.GetMoveablePawnCoords(ePlayer);
        BoardManager.UpdateBoard.Invoke();
        BoardManager.ResetState.Invoke();

        CheckWinAndDisplay();
    }

    private void EnablePlayerPut(bool bOn) // PlayerButtons�� put��ư Ȱ��ȭ, ��Ȱ��ȭ
    {
        GameObject targetButton = GetCurrentPlayerButton();
        targetButton.GetComponent<PlayerButtons>().SetPutButtonInteractable(bOn);
    }

    private void UpdateRequestedPawnCoord(Vector2Int coord) // �̹� ���� ���� ���� pawn �̵� ��ġ ������Ʈ
    {
        RequestedPawnCoord = coord;
        _isUpdatePawnCoord = true;
        _isUpdatePlank = false;

        BoardManager.RemovePreviewPlank.Invoke();
        BoardManager.UpdateClickedPawn.Invoke(_turn, coord);

        EnablePlayerPut(true);
    }

    private void UpdateRequestedPlank(Vector2Int coord) // �̹� ���� ���� ���� plank ������Ʈ
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

    private void CheckWinAndDisplay() // �÷��̾� �¸���, �¸�ȭ���� ����. �����ϴ� �÷��̾��� WinStateȮ�� ��, �¸�ȭ�� Ȱ��ȭ/��Ȱ��ȭ
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

    private bool IsNextTurnAvaible() // return ���������� �Ѿ� �� �� �ִ���
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

    private GameObject GetCurrentPlayerButton() // returns ���� �� ������ PlayerButton
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