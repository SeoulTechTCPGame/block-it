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
    private AuthManager _auth;

    private void Awake()
    {
        _auth.LoadUserInfo();
        if (PlayerPrefs.GetString("User_Display_Name") != null)
        {
            _loggedIn = true;
        }

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
        if (_loggedIn == false)
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