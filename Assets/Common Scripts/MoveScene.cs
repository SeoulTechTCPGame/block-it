using UnityEngine;
using UnityEngine.SceneManagement;

// �� �̵��� ����ϴ� Ŭ����
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
