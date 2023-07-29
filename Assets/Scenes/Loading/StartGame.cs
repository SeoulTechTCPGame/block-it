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
        // TODO: 로그인 여부를 확인하는 코드를 추가
        // 이미 회원가입이 되어 있다면 _loggedIn 변수를 true로 설정
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