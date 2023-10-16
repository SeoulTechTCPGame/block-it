using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using Mirror;

public class LANConnctionController : NetworkManager
{
    [Header("클라이언트, 호스트 선택 화면")]
    [SerializeField] GameObject _selectHostClientPanel;
    [SerializeField] Button _client;
    [SerializeField] Button _host;

    [Header("호스트 검색 화면")]
    [SerializeField] private GameObject _searchPanel;   // 상대방 찾는 화면
    [SerializeField] private Button _search;    // 상대방 찾기 버튼
    [SerializeField] private Transform _grid;   // 상대방 나열 그리드
    [SerializeField] private GameObject _playerButtonPrefab;    // 플레이어 프리팹

    [Header("클라이언트 대기 화면")]
    [SerializeField] GameObject _waitingForClientPanel;
    [SerializeField] private GameObject _loading; // 로딩 이미지

    [Header("검색 수락 화면")]
    [SerializeField] private GameObject _matchPanel;    // 대전 수락 화면
    [SerializeField] private GameObject _acceptBtn;            // 대전 수락 버튼
    [SerializeField] private Button _cancelBtn;            // 대전 거부 버튼
    [SerializeField] private Image _image;              // 상대방 이미지
    [SerializeField] TMP_Text _score;                   // 상대방 전적 메세지
    [SerializeField] GameObject _message;           // 호스트 대기 메세지
    private Button _accept;

    [Header("공통")]
    [SerializeField] private Button _back;  // 뒤로가기 버튼
    [SerializeField] private MoveScene _ms; // 이동씬 스크립트
    [SerializeField] GameObject _networkManager; // 네트워크 매니저
    [SerializeField] private UserDiscovery networkDiscovery; // 네트워크 탐색

    private GameObject _currentPanel;    // 현재 활성화된 패널
    private LocalizeScript _searchLocalize;

    private List<DiscoveryResponse> userList = new(); // 유저 리스트

    public override void Start()
    {
        // UI 초기화
        _accept = _acceptBtn.GetComponent<Button>();
        _selectHostClientPanel.SetActive(true);
        _searchPanel.SetActive(false);
        _matchPanel.SetActive(false);
        _currentPanel = _selectHostClientPanel;

        // 번역 스크립트 가져오기
        _searchLocalize = _search.GetComponentInChildren<TMP_Text>().GetComponent<LocalizeScript>();

        // 버튼 Listener 활성화
        _client.onClick.AddListener(OnClientButtonClicked);
        _host.onClick.AddListener(OnHostButtonClicked);
        _search.onClick.AddListener(OnSearchButtonClicked);
        _cancelBtn.onClick.AddListener(OnCancelButtonClicked);
        _back.onClick.AddListener(OnBackButtonClicked);
    }

    public override void Update()
    {
        // 로딩 UI 재생
        if (_currentPanel == _waitingForClientPanel)
        {
            _loading.transform.Rotate(Vector3.back, Time.deltaTime * 100);
        }
    }

    #region 호스트 / 클라이언트 선택 패널 
    // 호스트 버튼을 클릭할 때
    private void OnHostButtonClicked()
    {
        // 상대방 대기 화면 띄우기
        _selectHostClientPanel.SetActive(false);
        _waitingForClientPanel.SetActive(true);
        _loading.SetActive(true);
        _currentPanel = _waitingForClientPanel;
        NetworkManager.singleton.StartHost();
        networkDiscovery.AdvertiseServer();
        NetworkServer.RegisterHandler<OpponentUserDataMessage>(OnReceiveUserId);
    }

    // 클라이언트 버튼을 클릭할 때
    private void OnClientButtonClicked()
    {
        // 상대방 검색 화면 띄우기
        _selectHostClientPanel.SetActive(false);
        _searchPanel.SetActive(true);
        _currentPanel = _searchPanel;
        networkDiscovery.StartDiscovery();
    }
    #endregion

    #region 서치 패널
    // 상대방 검색 버튼을 클릭했을 때
    private void OnSearchButtonClicked()
    {
        ClearOpponentGrid();
        DrawOpponentGrid();
    }

    // 상대방 데이터로 그리드를 채우는 메서드
    private void DrawOpponentGrid()
    {
        foreach (DiscoveryResponse user in userList)
        {
            GameObject opponentButton = Instantiate(_playerButtonPrefab, _grid);
            TMP_Text buttonText = opponentButton.GetComponentInChildren<TMP_Text>();
            buttonText.text = user.userName;

            Button button = opponentButton.GetComponent<Button>();
            button.onClick.AddListener(() => OnOpponentButtonClicked(user));
        }
    }

    // 그리드 비우는 메서드
    private void ClearOpponentGrid()
    {
        foreach (Transform child in _grid)
        {
            Destroy(child.gameObject);
        }
    }

    // 상대방 버튼을 클릭했을 때 호출되는 메서드
    private void OnOpponentButtonClicked(DiscoveryResponse user)
    {
        BlockItUser opponentPlayer = new(user.userId, user.userName);
        if (!user.isGuest)
        {
            // 상대방 정보 가져오기
            BlockItUserDataManager.Singleton.GetUserData(opponentPlayer, () =>
            {
                if (opponentPlayer.PlayCount > 0)
                {
                    _score.text = opponentPlayer.PlayCount + " 게임, 승률: " + opponentPlayer.WinCount * 100 / opponentPlayer.PlayCount + "%";
                }
            });
            // 상대방 프로필 사진 가져오기
            BlockItUserDataManager.Singleton.GetProfileImage(opponentPlayer, () =>
            {
                Texture2D texture = new Texture2D(2, 2);
                texture.LoadImage(opponentPlayer.ProfileImg);
                _image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            });
        }

        // 매칭 화면 띄우기
        _matchPanel.SetActive(true);
        _searchPanel.SetActive(false);
        _waitingForClientPanel.SetActive(false);
        _currentPanel = _matchPanel;

        // 수락 버튼 활성화
        _message.SetActive(false);
        _acceptBtn.SetActive(true);
        _accept.onClick.AddListener(() => OnClientAcceptButtonClicked(user));

        TMP_Text opponentNameText = _currentPanel.GetComponentInChildren<TMP_Text>();
        opponentNameText.text = user.userName;
    }
    #endregion

    #region 매치 패널
    // 클라이언트가 수락 버튼을 클릭했을 때 호출되는 메서드
    private void OnClientAcceptButtonClicked(DiscoveryResponse user)
    {
        // 클라이언트의 경우 호스트와 연결 후 호스트 대기 화면 표시
        // 호스트 대기 메세지 띄우기
        if (!NetworkServer.active)
        {
            NetworkManager.singleton.StartClient(user.serverUri);
            _message.SetActive(true);
            _acceptBtn.SetActive(false);
            NetworkClient.RegisterHandler<ConnectionAcceptanceMessage>(OnAcceptanceMessageReceived);
        }
    }

    // 서버가 수락 버튼을 클릭했을 때 호출되는 메서드
    private void OnServerAcceptButtonClicked()
    {
        Debug.Log("호스트가 해당 연결을 수락.");
        ConnectionAcceptanceMessage msg = new ConnectionAcceptanceMessage { doAccept = true };
        NetworkServer.SendToAll(msg);
        _ms.ToLocalPlayMultiWifi();
    }

    private void OnCancelButtonClicked()
    {
        // 호스트가 취소 버튼을 눌렀을 경우 취소 메세지 클라이언트에게 전달
        if (NetworkServer.active)
        {
            ConnectionAcceptanceMessage msg = new ConnectionAcceptanceMessage { doAccept = false };
            NetworkServer.SendToAll(msg);
        }
        OnBackButtonClicked();
    }
    #endregion

    #region 네트워크
    // 클라이언트가 서버에 연결되었을 때 호출되는 메서드
    public override void OnClientConnect()
    {
        if (!NetworkServer.active)
        {
            base.OnClientConnect();
            NetworkClient.Send<OpponentUserDataMessage>(new OpponentUserDataMessage
            {
                userId = CurrentLoginSession.Singleton.User.Id,
                userName = CurrentLoginSession.Singleton.User.Nickname,
                isGuest = CurrentLoginSession.Singleton.User.IsGuest
            });
        }
    }

    // 클라이언트가 호스트에 연결하고, 유저 ID를 보냈을 때 호스트에서 호출되는 메서드
    private void OnReceiveUserId(NetworkConnectionToClient conn, OpponentUserDataMessage msg)
    {
        Debug.Log("상대방 플레이어(" + msg.userName + ") 수락 요청");
        networkDiscovery.StopDiscovery();
        BlockItUser opponentPlayer = new(msg.userId, msg.userName);
        if (!msg.isGuest)
        {
            // 상대방 정보 가져오기
            BlockItUserDataManager.Singleton.GetUserData(opponentPlayer, () =>
            {
                if (opponentPlayer.PlayCount > 0)
                {
                    _score.text = opponentPlayer.PlayCount + " 게임, 승률: " + opponentPlayer.WinCount * 100 / opponentPlayer.PlayCount + "%";
                }
            });
            // 상대방 프로필 사진 가져오기
            BlockItUserDataManager.Singleton.GetProfileImage(opponentPlayer, () =>
            {
                Texture2D texture = new Texture2D(2, 2);
                texture.LoadImage(opponentPlayer.ProfileImg);
                _image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            });
        }

        // 매칭 화면 띄우기
        _loading.SetActive(false);
        _matchPanel.SetActive(true);
        _searchPanel.SetActive(false);
        _waitingForClientPanel.SetActive(false);
        _currentPanel = _matchPanel;

        // 수락 버튼 활성화
        _message.SetActive(false);
        _acceptBtn.SetActive(true);
        _accept.onClick.AddListener(OnServerAcceptButtonClicked);

        TMP_Text opponentNameText = _currentPanel.GetComponentInChildren<TMP_Text>();
        opponentNameText.text = msg.userName;
    }

    // 클라이언트가 호스트의 수락 여부 메세지를 받을 경우
    private void OnAcceptanceMessageReceived(ConnectionAcceptanceMessage msg)
    {
        // 호스트가 수락했을 때 게임 시작
        if (msg.doAccept)
        {
            _ms.ToLocalPlayMultiWifi();
        }
        // 호스트가 수락하지 않았을 때 거절 메세지 표시
        else
        {
            // 호스트가 연결을 끊었다는 메세지와 함께 클라이언트 연결 중단
            TMP_Text waitingForHostAcceptMessage = _message.GetComponent<TMP_Text>();
            waitingForHostAcceptMessage.text = "The host rejected the challenge.";
            waitingForHostAcceptMessage.color = Color.red;
            NetworkManager.singleton.StopClient();
        }
    }

    // 클라이언트가 연결을 끊었을 때 (= 호스트의 수락을 기다리지 않고 취소했을 때)
    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        base.OnServerDisconnect(conn);

        // 수락 버튼 비활성화와 함께 연결 끊어졌다는 메세지 표시
        _message.SetActive(true);
        _acceptBtn.SetActive(false);
        TMP_Text waitingForHostAcceptMessage = _message.GetComponent<TMP_Text>();
        waitingForHostAcceptMessage.text = "The client rejected the challenge.";
        waitingForHostAcceptMessage.color = Color.red;
    }
    #endregion

    #region 공통
    // 호스트 찾았을 때 호출되는 메서드
    public void OnDiscoveredServer(DiscoveryResponse response)
    {
        // 중복 체크 후 찾은 호스트 리스트에 추가
        // 배포할 땐 한 계정으로 LAN 플레이 제한
        // if (!userList.Contains(response) && CurrentLoginSession.Singleton.User.Nickname != response.userName)
        if (!userList.Contains(response))
        {
            userList.Add(response);
            Debug.Log(response.userId + "(닉네임: " + response.userName + ", URI: " + response.serverUri.ToString() + ") 유저 리스트에 추가");
        }
    }

    // 뒤로 가기 버튼을 클릭했을 때 호출되는 메서드
    private void OnBackButtonClicked()
    {
        // 3단계: 매치 패널일 때
        if (_currentPanel == _matchPanel)
        {
            TMP_Text message = _message.GetComponent<TMP_Text>();
            message.text = "Wating for opponent accept...";
            message.color = Color.blue;

            // 클라이언트의 경우
            if (!NetworkServer.active)
            {
                // 호스트 수락 여부 대기 중에 중단하는 경우
                if (NetworkClient.active)
                {
                    // 연결 중단
                    NetworkManager.singleton.StopClient();
                }

                // 1. 변경 했던 오브젝트 원래대로 돌리기
                // 2. 호스트 검색 다시 시작
                _message.SetActive(false);
                _acceptBtn.SetActive(true);
                networkDiscovery.StartDiscovery();

                // 3. 호스트 검색 씬으로 되돌아가기
                _matchPanel.SetActive(false);
                _searchPanel.SetActive(true);
                _currentPanel = _searchPanel;

                _searchLocalize.TextKey = "Search";

                // 찾은 상대 리스트 비우기
                ClearOpponentGrid();
                userList.Clear();
            }
            // 서버의 경우
            // 1. 클라이언트에게 거절 메세지
            // 2. 클라이언트 대기 씬으로 이동
            else
            {
                _loading.SetActive(true);
                _matchPanel.SetActive(false);
                _waitingForClientPanel.SetActive(true);
                _currentPanel = _waitingForClientPanel;
                networkDiscovery.AdvertiseServer();
            }
        }
        // 2단계: 상대 검색 패널일 떄
        else if (_currentPanel == _searchPanel)
        {
            _searchPanel.SetActive(false);
            _selectHostClientPanel.SetActive(true);
            _currentPanel = _selectHostClientPanel;

            // 찾은 상대 리스트 비우기
            ClearOpponentGrid();
            userList.Clear();

            // 탐색 및 클라이언트 멈추기
            networkDiscovery.StopDiscovery();
            NetworkManager.singleton.StopClient();
            Debug.Log("상대 검색 종료.");
        }
        // 2단계: 상대 대기 패널일 때
        else if (_currentPanel == _waitingForClientPanel)
        {
            _waitingForClientPanel.SetActive(false);
            _selectHostClientPanel.SetActive(true);
            _loading.SetActive(false);
            _currentPanel = _selectHostClientPanel;
            
            // Advertise 및 호스트 멈추기
            networkDiscovery.StopDiscovery();
            NetworkManager.singleton.StopHost();
            Debug.Log("상대 대기 종료.");
        }
        // 1단계: 클라이언트/호스트 선택 패널일 때
        else
        {
            _ms.ToHome();
            Destroy(_networkManager);
        }
    }
    #endregion
}