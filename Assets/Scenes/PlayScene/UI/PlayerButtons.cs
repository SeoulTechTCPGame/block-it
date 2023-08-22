using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum EPlankImgState
{
    Normal,
    Horizontal,
    Vertical
}

/*
 * 'PlayerButtons'�� pawn�̵���Ű��, plank ����, Put (�� �����ϱ�) ��ư ���� �� ǥ���ϴ� Ŭ�����̴�.
 */
public class PlayerButtons : MonoBehaviour
{
    #region buttons, GameObjects
    public Button PawnButton;
    public Button PlankButton;
    public Button PutButton;
    public GameObject PlankImage;
    #endregion

    private Enums.EPlayer _owner; // playerButton�� ����
    private bool _isPlankValid = true; // plank�� ���� �� �ִ°�? (���� plank�� ������ false)
    private EPlankImgState _plankImgState = EPlankImgState.Normal; // plank image�� state

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

    void Awake() // �̹��� �ޱ�, ��ư Ŭ�� �̺�Ʈ �Ҵ�
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

    public void SetButtons(bool bTurn) // ��ü ��ư Ȱ��ȭ/��Ȱ��ȭ
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

    public void OnPutButtonClicked() // put ��ư�� ������ ���� ������
    {
        MatchManager.ToNextTurn.Invoke();
    }

    public void SetOwner(Enums.EPlayer own)
    {
        _owner = own;
    }

   public void SetPutButtonInteractable(bool bInteractable) // put ��ư Ȱ��ȭ/��Ȱ��ȭ
    {
        PutButton.interactable = bInteractable;
    }

    public void DisableButtons() // ��ü ��ư ��Ȱ��ȭ
    {
        PawnButton.gameObject.SetActive(false);
        PlankButton.gameObject.SetActive(false);
        PutButton.gameObject.SetActive(false);
    }

    private void OnPawnButtonClicked() // pawn�̵� ��ư Ŭ����, ���忡 �̵������� ��ġ ǥ�� �� ���� ��û
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

    private void OnPlankButtonClicked() // plank ���� ��ư Ŭ����, ���忡 ��ġ ���� ��ġ ǥ�� �� ���� ��û
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

    private void SetPlankButtonDisable() // plank ���� ��ư ��Ȱ��ȭ
    {
        _isPlankValid = false;
        PlankButton.interactable = false;
        _plankPanelImage.color = _disabledColor;
    }

    private void ActivatePutButton(bool bOn) // put��ư Ȱ��ȭ/��Ȱ��ȭ
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