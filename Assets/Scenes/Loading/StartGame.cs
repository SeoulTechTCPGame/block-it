using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// �α��� ���� �Ǵ��Ͽ�, ���� ���� UI�� �����ִ� Ŭ����
public class StartGame : MonoBehaviour
{
    [SerializeField] float _TouchTimeout = 1f;  // ��� �ð�
    [SerializeField] GameObject Canvas;

    private float _timer;   // �ð� ������ �� ����
    private bool _loggedIn = false; //�α��� ����
    private GameObject _signUpButton;   //�α��� ��ư
    private GameObject _guestButton;    //�Խ�Ʈ ��� ��ư
    private GameObject _developButton;  //������ ��� ��ư���� ToDo: ���� ����

    private void Awake()
    {
        _developButton = Canvas.transform.Find("Develop").gameObject;
        _signUpButton = Canvas.transform.Find("Sign In").gameObject;
        _guestButton = Canvas.transform.Find("Guest").gameObject;
    }
    private void Start()
    {
        ButtonsVisibility();    // ��ư ���̴� �� ���� �Ǵ�
        _developButton.GetComponent<Button>().onClick.AddListener(MoveHomeScene);   // �����ڿ����� ToDo: ���� ����
    }
    private void Update()
    {
        _timer += Time.deltaTime;
        if (_loggedIn && ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || _timer >= _TouchTimeout))
            // �α����� �Ǿ� �����鼭 ȭ���� ��ġ�߰ų� �ð��� ������ �ڵ����� ȭ�� ��ȯ
        {
            MoveHomeScene();
        }
    }
    private void ButtonsVisibility()
    {
        // TODO: �α��� ���θ� Ȯ���ϴ� �ڵ带 �߰�
        // �̹� ȸ�������� �Ǿ� �ִٸ� _loggedIn ������ true�� ����
        if (_signUpButton != null && _guestButton != null)
        {
            Debug.Log("check");
            _signUpButton.SetActive(!_loggedIn);
            _guestButton.SetActive(!_loggedIn);

            _signUpButton.GetComponent<Button>().onClick.AddListener(MoveSignUpScene);
            _guestButton.GetComponent<Button>().onClick.AddListener(MoveHomeScene);
        }
    }
    private void MoveSignUpScene()
    {
        SceneManager.LoadScene("Auth");
    }
    private void MoveHomeScene()
    {
        SceneManager.LoadScene("Home");
    }
}