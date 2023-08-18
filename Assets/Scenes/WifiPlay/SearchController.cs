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
    private bool _searching;         // ���� �˻� ������ ����
    private bool _opponentFound;     // ������ ã�Ҵ��� ����
    private GameObject _currentPanel;    // ���� Ȱ��ȭ�� �г�

    private void Start()
    {
        _searchButtonText = _search.GetComponentInChildren<TMP_Text>();
        _searching = false;
        OnSearchButtonClicked();

        _search.onClick.AddListener(OnSearchButtonClicked);
        _accept.onClick.AddListener(OnAcceptButtonClicked);
        _back.onClick.AddListener(OnBackButtonClicked);
    }
    // �˻� ��ư�� Ŭ������ �� ȣ��Ǵ� �޼���
    private void OnSearchButtonClicked()
    {
        if (!_searching)
        {
            _searchButtonText.text = "Searching...";
            // ������ ã��
            _searching = true;
            OpponentGridData();
        }
    }
    // ������ ���� �����ͷ� �׸��带 ä��� �޼���
    private void OpponentGridData()
    {
        // ���� ���� ���� ��������
        string[] opponentNames = { "Opponent1", "Opponent2", "Opponent3" }; // ToDo: ���� ������ ��������

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
        DestroyCurrentPanel();

        TextMeshProUGUI ����_�̸�_�ؽ�Ʈ = _currentPanel.GetComponentInChildren<TextMeshProUGUI>();
        ����_�̸�_�ؽ�Ʈ.text = "����: " + opponentName;

        // �г��� ���� ȭ���� ������ ó���մϴ�.

        // �̹����� X���� �������� ȸ���մϴ�.
        Image ����_�̹��� = _currentPanel.GetComponentInChildren<Image>();
        ����_�̹���.transform.Rotate(new Vector3(1, 0, 0), 45f);

        // ���� �� ���� ��ư�� Ȱ��ȭ�մϴ�.
        _accept.gameObject.SetActive(true);
    }
    // ���� ��ư�� Ŭ������ �� ȣ��Ǵ� �޼���
    private void OnAcceptButtonClicked()
    {
        // ���� ���� �ε��ϰų� ������ ���� ó���� �����մϴ�.
        // ...
        DestroyCurrentPanel();
    }
    // ���� ��ư�� Ŭ������ �� ȣ��Ǵ� �޼���
    private void OnRefusalButtonClicked()
    {
        // ������ ������ �޽����� ǥ���մϴ�.
        _warningText.text = "������ ������ �����߽��ϴ�.";
        DestroyCurrentPanel();
    }
    // �ڷ� ���� ��ư�� Ŭ������ �� ȣ��Ǵ� �޼���
    private void OnBackButtonClicked()
    {
        DestroyCurrentPanel();
        _searchButtonText.text = "�˻�";
        _searching = false;

        // �׸��� ������ ����ϴ�.
        foreach (Transform child in _grid)
        {
            Destroy(child.gameObject);
        }
    }
    // ���� �г��� �ı��ϰ� ���� UI ���¸� �ʱ�ȭ�ϴ� �޼���
    private void DestroyCurrentPanel()
    {
        if (_currentPanel != null)
        {
            Destroy(_currentPanel);
        }

        // ���� �� ���� ��ư�� ����ϴ�.
        _accept.gameObject.SetActive(false);

        // ��� �޽����� �ʱ�ȭ�մϴ�.
        _warningText.text = "";
    }
}