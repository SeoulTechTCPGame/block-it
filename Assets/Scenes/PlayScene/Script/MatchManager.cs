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
    public GameObject P1Buttons;
    public GameObject P2Buttons;
    public GameObject WinState;
    #endregion

    private Enums.EPlayer _turn; // 현재 턴인 플레이어
    public Vector2Int RequestedPawnCoord; //이번 턴의 수로 놓을 pawn의 이동 좌표
    public Plank RequestedPlank = new Plank(); //이번 턴의 수로 설치할 plank
    private bool _isUpdatePawnCoord = false; // 이번 턴의 수로 pawn을 이동시켰는가
    private bool _isUpdatePlank = false; // 이번 턴의 수로 plank를 설치했는가
    private GameLogic _gameLogic;

    #region Events
    public static UnityEvent ToNextTurn; // 다음턴으로 넘긴다
    public static UnityEvent ResetMove; // 이번 턴의 수를 리셋한다.
    public static UnityEvent<Vector2Int> SetRequestedPawnCoord = new UnityEvent<Vector2Int>(); // 이번 턴의 수로 놓을 pawn 이동 위치 업데이트
    public static UnityEvent<Vector2Int> SetRequestedPlank= new UnityEvent<Vector2Int>(); // 이번 턴의 수로 놓을 plank 위치 업데이트
    #endregion

    void Awake() // 이벤트 할당, PlayerButton의 사용자 할당, _gameLogic 받기
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
 
    void Start() // 시작시, Player 1의 턴으로 세팅한다.
    {
        SetTurn(Enums.EPlayer.Player1);
    }

    private void SetButtonsOwner() // PlayButton들의 사용자 할당
    {
        P1Buttons.GetComponent<PlayerButtons>().SetOwner(Enums.EPlayer.Player1);
        P2Buttons.GetComponent<PlayerButtons>().SetOwner(Enums.EPlayer.Player2);
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

    private void CheckWinAndDisplay() // 플레이어 승리시, 승리화면을 띄운다. 참가하는 플레이어의 WinState확인 후, 승리화면 활성화/비활성화
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