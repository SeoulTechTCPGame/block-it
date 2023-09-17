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
    public void ToLocalPlayEModeSingle()
    {
        PlayerPrefs.SetInt("GameMode", ((int)Enums.EMode.Single));
        SceneManager.LoadScene("LocalPlay");
    }
    public void ToLocalPlayEModeAI()
    {
        PlayerPrefs.SetInt("GameMode", ((int)Enums.EMode.AI));
        SceneManager.LoadScene("LocalPlay");
    }
    public void ToLocalPlayEModeMultiWifi()
    {
        PlayerPrefs.SetInt("GameMode", ((int)Enums.EMode.MultiWifi));
        SceneManager.LoadScene("LocalPlay");
    }
    public void ToWifiLoby()
    {
        SceneManager.LoadScene("Search");
    }
    public void ToCredit()
    {
        SceneManager.LoadScene("Credit");
    }
}