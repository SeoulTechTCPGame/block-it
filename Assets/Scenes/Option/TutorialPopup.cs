using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class Step
{
    public Sprite Image;
    public string UpperExplanation;
    public string LowerExplanation;
}

public class TutorialPopup : MonoBehaviour
{
    [Header("Tutorial")]
    [SerializeField] GameObject _tutorialPopup;
    [SerializeField] Button _openBtn;
    [SerializeField] Button _closeBtn;
    [SerializeField] GameObject _optionPanel;
    [SerializeField] TMP_Text _upperExplain;
    [SerializeField] TMP_Text _lowerExplain;
    [SerializeField] Image _image;
    [SerializeField] Step[] _steps;
    [SerializeField] Button _backBtn;
    [SerializeField] Button _nextBtn;
    [SerializeField] TMP_Text _pageText;

    private int _currentStep = 0;

    private void Start()
    {
        _openBtn.onClick.AddListener(OpenTutorialPopup);
        _closeBtn.onClick.AddListener(CloseTutorialPopup);
        _backBtn.onClick.AddListener(ShowPreviousStep);
        _nextBtn.onClick.AddListener(ShowNextStep);

        _tutorialPopup.SetActive(false);
    }
    private void OpenTutorialPopup()
    {
        _tutorialPopup.SetActive(true);
        _optionPanel.SetActive(false);
        ShowStep(_currentStep);
    }
    private void CloseTutorialPopup()
    {
        _tutorialPopup.SetActive(false);
        _optionPanel.SetActive(true);
    }
    private void ShowPreviousStep()
    {
        ShowStep(_currentStep - 1);
    }
    private void ShowNextStep()
    {
        ShowStep(_currentStep + 1);
    }
    private void ShowStep(int stepIndex)
    {
        _currentStep = Mathf.Clamp(stepIndex, 0, _steps.Length - 1);

        _image.sprite = _steps[_currentStep].Image;
        _upperExplain.text = _steps[_currentStep].UpperExplanation;
        _lowerExplain.text = _steps[_currentStep].LowerExplanation;

        _backBtn.interactable = _currentStep > 0;
        _nextBtn.interactable = _currentStep < _steps.Length - 1;
        UpdatePageText();
    }
    private void UpdatePageText()
    {
        int currentPage = _currentStep + 1;
        int totalPages = _steps.Length;

        _pageText.text = string.Format("({0}/{1})", currentPage, totalPages);
    }
}