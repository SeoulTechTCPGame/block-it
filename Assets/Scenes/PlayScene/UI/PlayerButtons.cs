using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerButtons : MonoBehaviour
{
    public Button PawnButton;
    public Button PlankButton;
    public Button PutButton;

    private Image _pawnImage;
    private Image _plankImage;

    private Color _selectedColor = new Color(0.28f, 0.64f, 0.71f);
    private Color _disabledColor = new Color(0.44f, 0.44f, 0.44f);
    private Color _normalColor = new Color(0.78f, 0.91f, 0.91f);

    public void SetButtons(bool bTurn)
    {
        if (bTurn == true)
        {
            PawnButton.interactable = true;
            PlankButton.interactable = _bPlankValid;
            onPawnButtonClicked();
            setPutButton(true);
        }
        else
        {
            //set pawn
            PawnButton.interactable = false;
            _pawnImage.color = _disabledColor;
            //set Plank
            PlankButton.interactable = false;
            _plankImage.color = _disabledColor;
            //set Put
            setPutButton(false);
        }

    }

    public void OnPutButtonClicked()
    {
        MatchManager.ToNextTurn.Invoke();
    }

    private bool _bPlankValid = true;

    private void Awake()
    {
        _pawnImage = PawnButton.GetComponent<Image>();
        _plankImage = PlankButton.GetComponent<Image>();

        PawnButton.onClick.AddListener(onPawnButtonClicked);
        PlankButton.onClick.AddListener(onPlankButtonClicked);
        PutButton.onClick.AddListener(OnPutButtonClicked);
    }

    private void onPawnButtonClicked()
    {
        //set color of the buttons
        _pawnImage.color = _selectedColor;
        if (_bPlankValid == true)
        {
            _plankImage.color = _normalColor;
        }

        PawnButton.Select();
    }

    private void onPlankButtonClicked()
    {
        //set color of the buttons
        if(_bPlankValid == true)
        {
            _pawnImage.color = _normalColor;
            _plankImage.color = _selectedColor;
            PlankButton.Select();
        }

    }

    private void setPlankButtonDisable()
    {
        _bPlankValid = false;
        PlankButton.interactable = false;
        _plankImage.color = _disabledColor;
    }

    private void setPutButton(bool bOn)
    {
        PutButton.gameObject.SetActive(bOn);
    }

}