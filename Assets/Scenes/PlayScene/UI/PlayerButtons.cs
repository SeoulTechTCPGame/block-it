using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum EPlankImgState
{
    Normal,
    Horizontal,
    Vertical
}

public class PlayerButtons : MonoBehaviour
{
    public Button PawnButton;
    public Button PlankButton;
    public Button PutButton;
    public GameObject PlankImage;

    private EPlayer owner;

    private Image _pawnPanelImage;
    private Image _plankPanelImage;
    private RectTransform _plankImgTransform;

    private Color _selectedColor = new Color(0.28f, 0.64f, 0.71f);
    private Color _disabledColor = new Color(0.44f, 0.44f, 0.44f);
    private Color _normalColor = new Color(0.78f, 0.91f, 0.91f);

    private EPlankImgState _plankImgState = EPlankImgState.Normal;

    private Vector3 _normalRotation = new Vector3(0f, 0f, 0f);
    private Vector3 _verticalRotation = new Vector3(0f, 0f, 45f);
    private Vector3 _horizontalRotation = new Vector3(0f, 0f, -45f);

    private void Awake()
    {
        _pawnPanelImage = PawnButton.GetComponent<Image>();
        _plankPanelImage = PlankButton.GetComponent<Image>();
        _plankImgTransform = PlankImage.GetComponent<RectTransform>();

        PawnButton.onClick.AddListener(onPawnButtonClicked);
        PlankButton.onClick.AddListener(onPlankButtonClicked);
        PutButton.onClick.AddListener(OnPutButtonClicked);
    }
    public EPlankImgState GetPlankState()
    {
        return _plankImgState;
    }
    public void SetButtons(bool bTurn)
    {
        _plankImgState = EPlankImgState.Normal;
        rotatePlank();

        if (bTurn == true)
        {
            PawnButton.interactable = true;
            PlankButton.interactable = _bPlankValid;

            _pawnPanelImage.color = _normalColor;
            if (_bPlankValid == true)
            {
                _plankPanelImage.color = _normalColor;
            }

            setPutButton(true);
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
            setPutButton(false);
        }

    }

    public void OnPutButtonClicked()
    {
        BoardManager.RemoveMoveablePawns.Invoke();
        BoardManager.RemovePlaceablePlanks.Invoke();
        BoardManager.RemovePreviewPlank.Invoke();

        MatchManager.ToNextTurn.Invoke();

    }

    public void SetOwner(EPlayer own)
    {
        owner = own;
    }

    private bool _bPlankValid = true;


    private void onPawnButtonClicked()
    {
        //set color of the buttons
        _pawnPanelImage.color = _selectedColor;
        if (_bPlankValid == true)
        {
            _plankPanelImage.color = _normalColor;
        }
        BoardManager.RemovePlaceablePlanks.Invoke();
        BoardManager.ShowMoveablePawns.Invoke(owner);

        PawnButton.Select();
    }

    private void onPlankButtonClicked()
    {
        //set color of the buttons
        if(_bPlankValid == true)
        {
            _pawnPanelImage.color = _normalColor;
            _plankPanelImage.color = _selectedColor;
            PlankButton.Select();
        }

        BoardManager.RemovePlaceablePlanks.Invoke();

        //set Rotation
        if (_plankImgState == EPlankImgState.Normal || _plankImgState == EPlankImgState.Vertical)
        {
            _plankImgState = EPlankImgState.Horizontal;
            BoardManager.ShowPlaceablePlanks.Invoke(EDirection.Horizontal, owner);
        }
        else
        {
            _plankImgState = EPlankImgState.Vertical;
            BoardManager.ShowPlaceablePlanks.Invoke(EDirection.Vertical, owner);
        }
        rotatePlank();

        // Remove MoveablePawns Locations
        BoardManager.RemoveMoveablePawns.Invoke();
    }

    private void setPlankButtonDisable()
    {
        _bPlankValid = false;
        PlankButton.interactable = false;
        _plankPanelImage.color = _disabledColor;
    }

    private void setPutButton(bool bOn)
    {
        PutButton.gameObject.SetActive(bOn);
    }

    private void rotatePlank()
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
}