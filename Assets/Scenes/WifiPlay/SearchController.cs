using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using Mirror;
using Mirror.Discovery;

public class SearchController : MonoBehaviour
{
    [SerializeField] private GameObject _searchPanel;   // 상대방 찾는 화면
    [SerializeField] private GameObject _matchPanel;    // 대전 수락 화면
    [SerializeField] private Button _back;  // 뒤로가기 버튼
    [SerializeField] private Button _search;    // 상대방 찾기 버튼
    [SerializeField] private Button _accept;    // 대전 수락 버튼
    [SerializeField] private Button _refuse;    // 대전 거부 버튼
    [SerializeField] private Image _image;    // 상대방 이미지
    [SerializeField] private Transform _grid;   // 상대방 나열 그리드
    [SerializeField] private GameObject _playerButtonPrefab;    // 플레이어 프리팹
    [SerializeField] private GameObject _loading; // 로딩 이미지
    [SerializeField] private TMP_Text _warningText; // 경고 문구
    [SerializeField] private MoveScene _ms;

    private bool _isSearching;  // 상대방 검색 중인지 여부
    private Coroutine searchCoroutine; // 검색 코루틴 참조를 저장하기 위한 변수
    private bool _doIAccpet; // 내가 수락 여부
    private bool _doYouAccpet;  // 상대가 수락 여부
    private GameObject _currentPanel;    // 현재 활성화된 패널
    private List<NetworkConnection> connectedClients = new List<NetworkConnection>();   // 찾은 상대들
    private NetworkConnection _opponent;    // 대전 상대
    private LocalizeScript _searchLocalize;

    private void Start()
    {
        _searchPanel.SetActive(true);
        _matchPanel.SetActive(false);
        _currentPanel = _searchPanel;

        _searchLocalize = _search.GetComponentInChildren<TMP_Text>().GetComponent<LocalizeScript>();
        _isSearching = false;
        OnSearchButtonClicked();

        _search.onClick.AddListener(OnSearchButtonClicked);
        _accept.onClick.AddListener(OnAcceptButtonClicked);
        _refuse.onClick.AddListener(OnRefuseButtonClicked);
        _back.onClick.AddListener(OnBackButtonClicked);
    }

    #region 서치 패널
    // 상대방 검색 버튼을 클릭했을 때
    private void OnSearchButtonClicked()
    {
        if (!_isSearching)
        {
            _searchLocalize.TextKey = "Searching";
            searchCoroutine = StartCoroutine(FindConnectedClients());
            _isSearching = true;
            OpponentGridData();
        }
    }

    // 상대방 찾기
    private IEnumerator FindConnectedClients()
    {
        _loading.SetActive(true);
        while (searchCoroutine != null)
        {
            foreach (var client in connectedClients)    // ToDo: 상대방 찾기
            {
                // 검색하는 동안 로딩바 보여줌
                _loading.transform.Rotate(Vector3.back, Time.deltaTime * 100);

                // 상대방이 확인된다면 로그 출력
                if (client.isReady)
                {
                    // Debug.Log("Connected client: " + client.address);
                }
                yield return null;
            }
        }
        _loading.SetActive(false);
    }

    // 가상의 상대방 데이터로 그리드를 채우는 메서드
    private void OpponentGridData()
    {
        string[] opponentNames = { "Opponent1", "Opponent2", "Opponent3" }; // ToDo: 찾은 상대방 데이터 가져오기

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
        MovePanel();

        TMP_Text opponentNameText = _currentPanel.GetComponentInChildren<TMP_Text>();
        opponentNameText.text = opponentName;
    }
    #endregion

    #region 매치 패널
    // 수락 버튼을 클릭했을 때 호출되는 메서드
    private void OnAcceptButtonClicked()
    {
        // ToDo: 두 플레이어가 수락했는 지 확인 후 대전 씬을 로드 _ms.To()
    }
    private void OnRefuseButtonClicked()
    {
        // ToDo: 거부
        MovePanel();
    }
    #endregion

    #region 공통
    // 패널 이동할 때마다 호출되는 메서드
    private void MovePanel()
    {
        if (_currentPanel == _matchPanel)
        {
            // ToDo: 거절
            _matchPanel.SetActive(false);
            _searchPanel.SetActive(true);
            _currentPanel = _searchPanel;

            _searchLocalize.TextKey = "Search";
            _isSearching = false;

            foreach (Transform child in _grid)
            {
                Destroy(child.gameObject);
            }
        }
        else
        {
            _matchPanel.SetActive(true);
            _searchPanel.SetActive(false);
            _currentPanel = _matchPanel;

            //_image.sprite = 찾은 상대방의 닉네임과 이미지
        }
    }

    // 뒤로 가기 버튼을 클릭했을 때 호출되는 메서드
    private void OnBackButtonClicked()
    {
        if (_currentPanel == _matchPanel)
        {
            MovePanel();
        }
        else
        {
            _ms.ToHome();
        }
    }
    #endregion
}