using UnityEngine;
using UnityEngine.SceneManagement;

// 씬 이동을 담당하는 클래스
public class MoveScene : MonoBehaviour
{
    public void ToOption()
    {
        SceneManager.LoadScene("Option");
    }
    public void ToProfile() 
    {
        SceneManager.LoadScene("Profile");
    }
    public void ToHome()
    {
        SceneManager.LoadScene("Home");
    }
    public void ToLocalPlay()
    {
        SceneManager.LoadScene("LocalPlay");
    }
    public void ToWifiLoby()
    {
        SceneManager.LoadScene("Search");
    }
}
