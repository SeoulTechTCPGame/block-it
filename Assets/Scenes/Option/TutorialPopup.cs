using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Singleton;

// 한 페이지에 들어갈 구조 클래스
[System.Serializable]
public class Step
{
    public string ImagePath;    // 이미지 주소
    public string UpperExplanation; // 윗 설명
    public string LowerExplanation; // 아랫 설명
}
// 튜토리얼 데이터를 한 번에 저장하는 클래스
[System.Serializable]
public class TutorialData
{
    public Step[] Steps;
}
// 튜토리얼 관리 클래스
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
    [SerializeField] Button _backBtn;
    [SerializeField] Button _nextBtn;
    [SerializeField] TMP_Text _pageText;

    private TutorialData _tutorialData;
    private int _currentStep = 0;

    private void Start()
    {
        _openBtn.onClick.AddListener(OpenTutorialPopup);
        _closeBtn.onClick.AddListener(CloseTutorialPopup);
        _backBtn.onClick.AddListener(ShowPreviousStep);
        _nextBtn.onClick.AddListener(ShowNextStep);

        _tutorialPopup.SetActive(false);
    }
    // 튜토리얼 열기
    private void OpenTutorialPopup()
    {
        LoadTutorialData();
        _tutorialPopup.SetActive(true);
        _optionPanel.SetActive(false);
        ShowStep(_currentStep);
    }
    // 튜토리얼 닫기
    private void CloseTutorialPopup()
    {
        _tutorialPopup.SetActive(false);
        _optionPanel.SetActive(true);
    }
    // 이전 페이지
    private void ShowPreviousStep()
    {
        ShowStep(_currentStep - 1);
    }
    // 다음 페이지
    private void ShowNextStep()
    {
        ShowStep(_currentStep + 1);
    }
    // 현재 페이지의 데이터에 설명 부여
    private void ShowStep(int stepIndex)
    {
        _currentStep = Mathf.Clamp(stepIndex, 0, _tutorialData.Steps.Length - 1);

        string imagePath = "Images/" + _tutorialData.Steps[_currentStep].ImagePath;
        Sprite sprite = Resources.Load<Sprite>(imagePath);
        if (sprite != null)
        {
            _image.sprite = sprite;
            _image.preserveAspect = true;
        }
        else
        {
            Debug.LogError("Failed to load image: " + imagePath);
        }

        _upperExplain.text = _tutorialData.Steps[_currentStep].UpperExplanation;
        _lowerExplain.text = _tutorialData.Steps[_currentStep].LowerExplanation;

        _backBtn.interactable = _currentStep > 0;
        _nextBtn.interactable = _currentStep < _tutorialData.Steps.Length - 1;
        UpdatePageText();
    }
    // 현재 페이지 업데이트
    private void UpdatePageText()
    {
        int currentPage = _currentStep + 1;
        int totalPages = _tutorialData.Steps.Length;

        _pageText.text = string.Format("({0}/{1})", currentPage, totalPages);
    }
    // 튜토리얼 Json 파일 로드
    private void LoadTutorialData()
    {
        string jsonPath = "Json";
        TextAsset jsonAsset = null;
        switch (S.curLangIndex)
        {
            case ((int)Enums.ELanguage.EN):
                jsonPath += "/TutorialEN";

                jsonAsset = Resources.Load<TextAsset>(jsonPath);
                if (jsonAsset != null)
                {
                    string jsonData = jsonAsset.text;
                    _tutorialData = JsonUtility.FromJson<TutorialData>(jsonData);
                }
                else
                {
                    Debug.LogError("Tutorial EN JSON file not found!");
                    return;
                }
                break;
            case ((int)Enums.ELanguage.KR):
                jsonPath += "/TutorialKR";

                jsonAsset = Resources.Load<TextAsset>(jsonPath);
                if (jsonAsset != null)
                {
                    string jsonData = jsonAsset.text;
                    _tutorialData = JsonUtility.FromJson<TutorialData>(jsonData);
                }
                else
                {
                    Debug.LogError("Tutorial KR JSON file not found!");
                    return;
                }
                break;
            default:
                Debug.LogError("Singleton dose not operate");
                break;
        }
    }
}