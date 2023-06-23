using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    [SerializeField] float _TouchTimeout = 1f;
    private float _timer;
    private bool _loggedIn = false;

    private void Start()
    {
        ButtonsVisibility();
    }
    private void Update()
    {
        _timer += Time.deltaTime;
        if (!_loggedIn && ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || _timer >= _TouchTimeout))
        {
            SceneManager.LoadScene("Home");
        }
    }
    private void ButtonsVisibility()
    {
        // TODO: 로그인 여부를 확인하는 코드를 추가
        // 이미 회원가입이 되어 있다면 _loggedIn 변수를 true로 설정

        GameObject signUpButton = GameObject.Find("SignUp");
        GameObject guestButton = GameObject.Find("Guest");

        if (signUpButton != null && guestButton != null)
        {
            signUpButton.SetActive(!_loggedIn);
            guestButton.SetActive(!_loggedIn);
        }
    }
}