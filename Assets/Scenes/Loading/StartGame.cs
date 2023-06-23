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
        // TODO: �α��� ���θ� Ȯ���ϴ� �ڵ带 �߰�
        // �̹� ȸ�������� �Ǿ� �ִٸ� _loggedIn ������ true�� ����

        GameObject signUpButton = GameObject.Find("SignUp");
        GameObject guestButton = GameObject.Find("Guest");

        if (signUpButton != null && guestButton != null)
        {
            signUpButton.SetActive(!_loggedIn);
            guestButton.SetActive(!_loggedIn);
        }
    }
}