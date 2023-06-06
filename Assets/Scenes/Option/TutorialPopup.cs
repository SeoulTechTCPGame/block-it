using UnityEngine;
using UnityEngine.UI;

public class TutorialPopup : MonoBehaviour
{
    [SerializeField] GameObject tutorialPopup;
    [SerializeField] Button openButton;
    [SerializeField] Button closeButton;

    private void Start()
    {
        openButton.onClick.AddListener(OpenTutorialPopup);
        closeButton.onClick.AddListener(CloseTutorialPopup);
    }

    public void OpenTutorialPopup()
    {
        tutorialPopup.SetActive(true);
    }
    public void CloseTutorialPopup()
    {
        tutorialPopup.SetActive(false);
    }
}
