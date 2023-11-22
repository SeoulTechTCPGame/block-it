using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// 로그인 여부 판단하여, 시작 씬의 UI를 보여주는 클래스
public class StartGame : MonoBehaviour
{
    //[SerializeField] private float _TouchTimeout = 1f;  // 대기 시간
    [SerializeField] private GameObject Canvas;

    private float _timer;   // 시간 지났는 지 여부
    private bool _loggedIn = false; //로그인 여부
    private GameObject _signUpButton;   //로그인 버튼
    private GameObject _guestButton;    //게스트 모드 버튼

    private void Start()
    {
        _signUpButton = Canvas.transform.Find("Sign In").gameObject;
        _guestButton = Canvas.transform.Find("Guest").gameObject;

        ButtonsVisibility();    // 버튼 보이는 지 여부 판단
    }

    private void ButtonsVisibility()
    {
        _signUpButton.SetActive(!_loggedIn);
        _guestButton.SetActive(!_loggedIn);

        _signUpButton.GetComponent<Button>().onClick.AddListener(MoveSignUpScene);
        _guestButton.GetComponent<Button>().onClick.AddListener(MoveHomeScene);
    }

    private void MoveSignUpScene()
    {
        SceneManager.LoadScene("Auth");
    }

    private void MoveHomeScene()
    {
        CurrentLoginSession.Singleton.StartGuestMode();
        SceneManager.LoadScene("Home");
    }
}