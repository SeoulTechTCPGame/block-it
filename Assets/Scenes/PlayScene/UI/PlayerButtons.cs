using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerButtons : MonoBehaviour
{
    public Button PawnButton;
    public Button PlankButton;

    private Image _pawnImage;
    private Image _plankImage;

    private Color selectedColor = new Color(0.28f, 0.64f, 0.71f);
    private Color disabledColor = new Color(0.44f, 0.44f, 0.44f);
    private Color normalColor = new Color(0.78f, 0.91f, 0.91f);

    private void Start()
    {
        _pawnImage = PawnButton.GetComponent<Image>();
        _plankImage = PlankButton.GetComponent<Image>();

        onPawnButtonClicked();

        PawnButton.onClick.AddListener(onPawnButtonClicked);
        PlankButton.onClick.AddListener(onPlankButtonClicked);
    }

    private void onPawnButtonClicked()
    {
        //set color of the buttons
        _pawnImage.color = selectedColor;
        if (PlankButton.interactable == true)
        {
            _plankImage.color = normalColor;
        }

        PawnButton.Select();
    }

    private void onPlankButtonClicked()
    {
        //set color of the buttons
        _pawnImage.color = normalColor;
        if(PlankButton.interactable == true)
        {
            _plankImage.color = selectedColor;
        }

        PlankButton.Select();
    }

    private void setPlankButtonDisable()
    {
        PlankButton.interactable = false;
        _plankImage.color = disabledColor;
    }
}