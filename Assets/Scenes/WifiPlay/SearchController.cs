using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;

public class SearchController : MonoBehaviour
{
    [SerializeField] private GameObject _searchPanel;
    [SerializeField] private GameObject _matchPanel;
    [SerializeField] private Button _back;
    [SerializeField] private Button _search;
    [SerializeField] private Button _accept;
    [SerializeField] private Transform _grid;
    [SerializeField] private GameObject _playerButtonPrefab;
    [SerializeField] private TMP_Text _warningText;

    private TMP_Text _searchButtonText;
    private bool _searching;         // 상대방 검색 중인지 여부
    private bool _opponentFound;     // 상대방을 찾았는지 여부
    private GameObject _currentPanel;    // 현재 활성화된 패널

    private void Start()
    {
        _searchButtonText = _search.GetComponentInChildren<TMP_Text>();
        _searching = false;
        OnSearchButtonClicked();

        _search.onClick.AddListener(OnSearchButtonClicked);
        _accept.onClick.AddListener(OnAcceptButtonClicked);
        _back.onClick.AddListener(OnBackButtonClicked);
    }
    // 검색 버튼을 클릭했을 때 호출되는 메서드
    private void OnSearchButtonClicked()
    {
        if (!_searching)
        {
            _searchButtonText.text = "Searching...";
            // 상대방을 찾기
            _searching = true;
            OpponentGridData();
        }
    }
    // 가상의 상대방 데이터로 그리드를 채우는 메서드
    private void OpponentGridData()
    {
        // 상대방 계정 정보 가져오기
        string[] opponentNames = { "Opponent1", "Opponent2", "Opponent3" }; // ToDo: 상대방 데이터 가져오기

        foreach (string opponentName in opponentNames)
        {
            GameObject opponentButton = Instantiate(_playerButtonPrefab, _grid);
            TMP_Text buttonText = opponentButton.GetComponentInChildren<TMP_Text>();
            buttonText.text = opponentName;

            Button button = opponentButton.GetComponent<Button>();
            button.onClick.AddListener(() => OnOpponentButtonClicked(opponentName));
        }
    }
    // 상대방 버튼을 클릭했을 때 호출되는 메서드
    private void OnOpponentButtonClicked(string opponentName)
    {
        DestroyCurrentPanel();

        TextMeshProUGUI 상대방_이름_텍스트 = _currentPanel.GetComponentInChildren<TextMeshProUGUI>();
        상대방_이름_텍스트.text = "상대방: " + opponentName;

        // 패널이 이전 화면을 덮도록 처리합니다.

        // 이미지를 X축을 기준으로 회전합니다.
        Image 상대방_이미지 = _currentPanel.GetComponentInChildren<Image>();
        상대방_이미지.transform.Rotate(new Vector3(1, 0, 0), 45f);

        // 수락 및 거절 버튼을 활성화합니다.
        _accept.gameObject.SetActive(true);
    }
    // 수락 버튼을 클릭했을 때 호출되는 메서드
    private void OnAcceptButtonClicked()
    {
        // 대전 씬을 로드하거나 수락에 대한 처리를 수행합니다.
        // ...
        DestroyCurrentPanel();
    }
    // 거절 버튼을 클릭했을 때 호출되는 메서드
    private void OnRefusalButtonClicked()
    {
        // 상대방이 거절한 메시지를 표시합니다.
        _warningText.text = "상대방이 대전을 거절했습니다.";
        DestroyCurrentPanel();
    }
    // 뒤로 가기 버튼을 클릭했을 때 호출되는 메서드
    private void OnBackButtonClicked()
    {
        DestroyCurrentPanel();
        _searchButtonText.text = "검색";
        _searching = false;

        // 그리드 내용을 지웁니다.
        foreach (Transform child in _grid)
        {
            Destroy(child.gameObject);
        }
    }
    // 현재 패널을 파괴하고 관련 UI 상태를 초기화하는 메서드
    private void DestroyCurrentPanel()
    {
        if (_currentPanel != null)
        {
            Destroy(_currentPanel);
        }

        // 수락 및 거절 버튼을 숨깁니다.
        _accept.gameObject.SetActive(false);

        // 경고 메시지를 초기화합니다.
        _warningText.text = "";
    }
}