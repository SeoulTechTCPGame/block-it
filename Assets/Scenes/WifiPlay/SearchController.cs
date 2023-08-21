using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using Mirror;

public class SearchController : MonoBehaviour
{
    [SerializeField] private GameObject _searchPanel;   // ���� ã�� ȭ��
    [SerializeField] private GameObject _matchPanel;    // ���� ���� ȭ��
    [SerializeField] private Button _back;  // �ڷΰ��� ��ư
    [SerializeField] private Button _search;    // ���� ã�� ��ư
    [SerializeField] private Button _accept;    // ���� ���� ��ư
    [SerializeField] private Button _refuse;    // ���� �ź� ��ư
    [SerializeField] private Image _image;    // ���� �̹���
    [SerializeField] private Transform _grid;   // ���� ���� �׸���
    [SerializeField] private GameObject _playerButtonPrefab;    // �÷��̾� ������
    [SerializeField] private GameObject _loading; // �ε� �̹���
    [SerializeField] private TMP_Text _warningText; // ��� ����
    [SerializeField] private MoveScene _ms;

    private bool _isSearching;  // ���� �˻� ������ ����
    private Coroutine searchCoroutine; // �˻� �ڷ�ƾ ������ �����ϱ� ���� ����
    private bool _doIAccpet; // ���� ���� ����
    private bool _doYouAccpet;  // ��밡 ���� ����
    private GameObject _currentPanel;    // ���� Ȱ��ȭ�� �г�
    private List<NetworkConnection> connectedClients = new List<NetworkConnection>();   // ã�� ����
    private NetworkConnection _opponent;    // ���� ���
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
    #region ��ġ �г�
    // ���� �˻� ��ư�� Ŭ������ ��
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
    // ���� ã��
    private IEnumerator FindConnectedClients()
    {
        _loading.SetActive(true);
        while (searchCoroutine != null)
        {
            foreach (var client in connectedClients)    //ToDo: ���� ã��
            {
                _loading.transform.Rotate(Vector3.back, Time.deltaTime * 100);
                if (client.isReady)
                {
                    //Debug.Log("Connected client: " + client.address);
                }
                yield return null;
            }
        }
        _loading.SetActive(false);
    }
    // ������ ���� �����ͷ� �׸��带 ä��� �޼���
    private void OpponentGridData()
    {
        string[] opponentNames = { "Opponent1", "Opponent2", "Opponent3" }; // ToDo: ã�� ���� ������ ��������

        foreach (string opponentName in opponentNames)
        {
            GameObject opponentButton = Instantiate(_playerButtonPrefab, _grid);
            TMP_Text buttonText = opponentButton.GetComponentInChildren<TMP_Text>();
            buttonText.text = opponentName;

            Button button = opponentButton.GetComponent<Button>();
            button.onClick.AddListener(() => OnOpponentButtonClicked(opponentName));
        }
    }
    // ���� ��ư�� Ŭ������ �� ȣ��Ǵ� �޼���
    private void OnOpponentButtonClicked(string opponentName)
    {
        MovePanel();

        TMP_Text opponentNameText = _currentPanel.GetComponentInChildren<TMP_Text>();
        opponentNameText.text = opponentName;
    }
    #endregion
    #region ��ġ �г�
    // ���� ��ư�� Ŭ������ �� ȣ��Ǵ� �޼���
    private void OnAcceptButtonClicked()
    {
        // ToDo: �� �÷��̾ �����ߴ� �� Ȯ�� �� ���� ���� �ε� _ms.To()
    }
    private void OnRefuseButtonClicked()
    {
        // ToDo: �ź�
        MovePanel();
    }
    #endregion
    #region ����
    // �г� �̵��� ������ ȣ��Ǵ� �޼���
    private void MovePanel()
    {
        if (_currentPanel == _matchPanel)
        {
            // ToDo: ����
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

            //_image.sprite = ã�� ������ �г��Ӱ� �̹���
        }
    }
    // �ڷ� ���� ��ư�� Ŭ������ �� ȣ��Ǵ� �޼���
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