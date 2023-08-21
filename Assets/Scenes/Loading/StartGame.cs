using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// 로그인 여부 판단하여, 시작 씬의 UI를 보여주는 클래스
public class StartGame : MonoBehaviour
{
    [SerializeField] float _TouchTimeout = 1f;  // 대기 시간
    [SerializeField] GameObject Canvas;

    private float _timer;   // 시간 지났는 지 여부
    private bool _loggedIn = false; //로그인 여부
    private GameObject _signUpButton;   //로그인 버튼
    private GameObject _guestButton;    //게스트 모드 버튼
    private GameObject _developButton;  //개발자 모드 버튼으로 ToDo: 삭제 예정
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
        ButtonsVisibility();    // 버튼 보이는 지 여부 판단
        _developButton.GetComponent<Button>().onClick.AddListener(MoveHomeScene);   // 개발자용으로 ToDo: 삭제 예정
    }
    private void Update()
    {
        _timer += Time.deltaTime;
        if (_loggedIn && ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || _timer >= _TouchTimeout))
            // 로그인이 되어 있으면서 화면을 터치했거나 시간이 지나면 자동으로 화면 전환
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