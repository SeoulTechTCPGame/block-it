using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    [SerializeField] float _TouchTimeout = 1f;
    [SerializeField] GameObject Canvas;
    private float _timer;
    private bool _loggedIn = false;
    private GameObject _signUpButton;
    private GameObject _guestButton;
    private GameObject _developButton;

    private void Awake()
    {
        _developButton = Canvas.transform.Find("Develop").gameObject;
        _signUpButton = Canvas.transform.Find("Sign In").gameObject;
        _guestButton = Canvas.transform.Find("Guest").gameObject;
    }
    private void Start()
    {
        ButtonsVisibility();
        _developButton.GetComponent<Button>().onClick.AddListener(MoveHomeScene);
    }
    private void Update()
    {
        _timer += Time.deltaTime;
        if (_loggedIn && ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || _timer >= _TouchTimeout))
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