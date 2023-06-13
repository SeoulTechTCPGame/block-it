using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class Step
{
    public Sprite image;
    public string explanation;
}

public class TutorialPopup : MonoBehaviour
{
    [SerializeField] GameObject _tutorialPopup;
    [SerializeField] Button _openBtn;
    [SerializeField] Button _closeBtn;
    [SerializeField] GameObject _scene;
    [SerializeField] TMP_Text _explain;
    [SerializeField] Image _image;
    [SerializeField] Step[] _steps;
    [SerializeField] Button _backBtn;
    [SerializeField] Button _nextBtn;

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
        _scene.SetActive(false);
        ShowStep(_currentStep);
    }
    private void CloseTutorialPopup()
    {
        _tutorialPopup.SetActive(false);
        _scene.SetActive(true);
    }
    public void ShowPreviousStep()
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

        _image.sprite = _steps[_currentStep].image;
        _explain.text = _steps[_currentStep].explanation;

        _backBtn.interactable = _currentStep > 0;
        _nextBtn.interactable = _currentStep < _steps.Length - 1;
    }
}