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

    private Color selectedColor = new Color(0.28f, 0.64f, 0.71f);
    private Color disabledColor = new Color(0.44f, 0.44f, 0.44f);
    private Color normalColor = new Color(0.78f, 0.91f, 0.91f);

    private bool bPlankValid = true;

    private void Awake()
    {
        _pawnImage = PawnButton.GetComponent<Image>();
        _plankImage = PlankButton.GetComponent<Image>();

        //onPawnButtonClicked();
        /*
         */
        PawnButton.onClick.AddListener(onPawnButtonClicked);
        PlankButton.onClick.AddListener(onPlankButtonClicked);
        PutButton.onClick.AddListener(onPutButtonClicked);
    }

    private void onPawnButtonClicked()
    {
        //set color of the buttons
        _pawnImage.color = selectedColor;
        if (bPlankValid == true)
        {
            _plankImage.color = normalColor;
        }

        PawnButton.Select();
    }

    private void onPlankButtonClicked()
    {
        //set color of the buttons
        if(bPlankValid == true)
        {
            _pawnImage.color = normalColor;
            _plankImage.color = selectedColor;
            PlankButton.Select();
        }

    }

    public void SetButtons(bool bTurn)
    {
        if (bTurn == true)
        {
            PawnButton.interactable = true;
            PlankButton.interactable = bPlankValid;
            onPawnButtonClicked();
            setPutButton(true);
        }
        else
        {
            //set pawn
            PawnButton.interactable = false;
            _pawnImage.color = disabledColor;
            //set Plank
            PlankButton.interactable = false;
            _plankImage.color = disabledColor;
            //set Put
            setPutButton(false);
        }

    }

    private void setPlankButtonDisable()
    {
        bPlankValid = false;
        PlankButton.interactable = false;
        _plankImage.color = disabledColor;
    }

    private void setPutButton(bool bOn)
    {
        PutButton.gameObject.SetActive(bOn);
    }

    public void onPutButtonClicked()
    {
        MatchManager.toNextTurn.Invoke();
    }
}