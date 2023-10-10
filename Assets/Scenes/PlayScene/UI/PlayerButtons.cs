using UnityEngine;
using UnityEngine.UI;
using static Enums;

public enum EPlankImgState
{
    Normal,
    Horizontal,
    Vertical
}

/*
 * 'PlayerButtons'는 pawn이동시키기, plank 놓기, Put (수 결정하기) 버튼 관리 및 표시하는 클래스이다.
 */
public class PlayerButtons : MonoBehaviour
{
    #region buttons, GameObjects
    public Button PawnButton;
    public Button PlankButton;
    public Button PutButton;
    public GameObject PlankImage;
    #endregion

    private EPlayer _owner; // playerButton의 주인
    private bool _isPlankValid = true; // plank를 놓을 수 있는가? (남은 plank가 없으면 false)
    private EPlankImgState _plankImgState = EPlankImgState.Normal; // plank image의 state

    #region Image, Color, PlankRotations
    private Image _pawnPanelImage;
    private Image _plankPanelImage;
    private RectTransform _plankImgTransform;

    private Color _selectedColor = new Color(0.28f, 0.64f, 0.71f);
    private Color _disabledColor = new Color(0.44f, 0.44f, 0.44f);
    private Color _normalColor = new Color(0.78f, 0.91f, 0.91f);

    private Vector3 _normalRotation = new Vector3(0f, 0f, 0f);
    private Vector3 _verticalRotation = new Vector3(0f, 0f, 45f);
    private Vector3 _horizontalRotation = new Vector3(0f, 0f, -45f);
    #endregion

    void Awake() // 이미지 받기, 버튼 클릭 이벤트 할당
    {
        _pawnPanelImage = PawnButton.GetComponent<Image>();
        _plankPanelImage = PlankButton.GetComponent<Image>();
        _plankImgTransform = PlankImage.GetComponent<RectTransform>();

        PawnButton.onClick.AddListener(OnPawnButtonClicked);
        PlankButton.onClick.AddListener(OnPlankButtonClicked);
        PutButton.onClick.AddListener(OnPutButtonClicked);
    }

    public EPlankImgState GetPlankState()
    {
        return _plankImgState;
    }

    public void SetButtons(bool bTurn) // 전체 버튼 활성화/비활성화
    {
        ResetPlankState();

        if (bTurn == true)
        {
            PawnButton.interactable = true;
            PlankButton.interactable = _isPlankValid;

            _pawnPanelImage.color = _normalColor;
            if (_isPlankValid == true)
            {
                _plankPanelImage.color = _normalColor;
            }

            ActivatePutButton(true);
        }
        else
        {
            //set pawn
            PawnButton.interactable = false;
            _pawnPanelImage.color = _disabledColor;
            //set Plank
            PlankButton.interactable = false;
            _plankPanelImage.color = _disabledColor;
            //set Put
            ActivatePutButton(false);
        }

    }

    public void OnPutButtonClicked() // put 버튼이 눌리면 다음 턴으로
    {
        MatchManager.ToNextTurn.Invoke();
    }

    public void SetOwner(EPlayer own)
    {
        _owner = own;
    }

   public void SetPutButtonInteractable(bool bInteractable) // put 버튼 활성화/비활성화
    {
        PutButton.interactable = bInteractable;
    }

    public void DisableButtons() // 전체 버튼 비활성화
    {
        PawnButton.gameObject.SetActive(false);
        PlankButton.gameObject.SetActive(false);
        PutButton.gameObject.SetActive(false);
    }

    private void OnPawnButtonClicked() // pawn이동 버튼 클릭시, 보드에 이동가능한 위치 표시 등 보드 요청
    {
        MatchManager.ResetMove.Invoke();
        //set color of the buttons
        _pawnPanelImage.color = _selectedColor;
        if (_isPlankValid)
        {
            _plankPanelImage.color = _normalColor;
        }
        BoardManager.RemovePlaceablePlanks.Invoke();
        BoardManager.RemovePreviewPlank.Invoke();
        BoardManager.ShowMoveablePawns.Invoke(_owner);

        PawnButton.Select();
    }

    private void OnPlankButtonClicked() // plank 놓기 버튼 클릭시, 보드에 설치 가능 위치 표시 등 보드 요청
    {
        MatchManager.ResetMove.Invoke();

        //set color of the buttons
        if (_isPlankValid)
        {
            _pawnPanelImage.color = _normalColor;
            _plankPanelImage.color = _selectedColor;
            PlankButton.Select();
        }

        BoardManager.RemovePlaceablePlanks.Invoke();
        BoardManager.RemovePreviewPlank.Invoke();
        //set Rotation
        if (_plankImgState == EPlankImgState.Normal || _plankImgState == EPlankImgState.Vertical)
        {
            _plankImgState = EPlankImgState.Horizontal;
            BoardManager.ShowPlaceablePlanks.Invoke(EDirection.Horizontal, _owner);
        }
        else
        {
            _plankImgState = EPlankImgState.Vertical;
            BoardManager.ShowPlaceablePlanks.Invoke(EDirection.Vertical, _owner);
        }
        RotatePlank();

        // Remove MoveablePawns Locations
        BoardManager.RemoveMoveablePawns.Invoke();
    }

    private void SetPlankButtonDisable() // plank 놓기 버튼 비활성화
    {
        _isPlankValid = false;
        PlankButton.interactable = false;
        _plankPanelImage.color = _disabledColor;
    }

    private void ActivatePutButton(bool bOn) // put버튼 활성화/비활성화
    {
        PutButton.gameObject.SetActive(bOn);
        PutButton.interactable = false;
    }

    private void RotatePlank()
    {
        Vector3 targetRotation = _normalRotation;

        switch (_plankImgState)
        {
            case EPlankImgState.Normal:
                targetRotation = _normalRotation;
                break;
            case EPlankImgState.Horizontal:
                targetRotation = _horizontalRotation;
                break;
            case EPlankImgState.Vertical:
                targetRotation = _verticalRotation;
                break;
        }

        _plankImgTransform.rotation = Quaternion.Euler(targetRotation);
    }

    private void ResetPlankState()
    {
        _plankImgState = EPlankImgState.Normal;
        RotatePlank();
    }
}