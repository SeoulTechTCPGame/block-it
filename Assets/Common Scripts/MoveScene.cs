using UnityEngine;
using UnityEngine.SceneManagement;
using static Enums;

// 씬 이동을 담당하는 클래스
public class MoveScene : MonoBehaviour
{
    public void ToLobby()
    {
        SceneManager.LoadScene("Loading");
    }
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
    public void ToWifiLoby()
    {
        SceneManager.LoadScene("Search");
    }
    public void ToCredit()
    {
        SceneManager.LoadScene("Credit");
    }
    public void ToLevelSelect()
    {
        SceneManager.LoadScene("LevelSelect");
    }
    public void ToLocalPlayFriend()
    {
        PlayerPrefs.SetInt("GameMode", ((int)EMode.Friend));
        SceneManager.LoadScene("LocalPlay");
    }
    public void ToLocalPlayAIBeginner()
    {
        PlayerPrefs.SetInt("GameMode", ((int)EMode.AI));
        PlayerPrefs.SetInt("AILevel", ((int)ELevel.Beginner));
        SceneManager.LoadScene("LocalPlay");
    }
    public void ToLocalPlayAIIntermediate()
    {
        PlayerPrefs.SetInt("GameMode", ((int)EMode.AI));
        PlayerPrefs.SetInt("AILevel", ((int)ELevel.Intermediate));
        SceneManager.LoadScene("LocalPlay");
    }
    public void ToLocalPlayAIAdvanced()
    {
        PlayerPrefs.SetInt("GameMode", ((int)EMode.AI));
        PlayerPrefs.SetInt("AILevel", ((int)ELevel.Advanced));
        SceneManager.LoadScene("LocalPlay");
    }
    public void ToLocalPlayAIMaster()
    {
        PlayerPrefs.SetInt("GameMode", ((int)EMode.AI));
        PlayerPrefs.SetInt("AILevel", ((int)ELevel.Master));
        SceneManager.LoadScene("LocalPlay");
    }
    public void ToLocalPlayMultiWifi()
    {
        PlayerPrefs.SetInt("GameMode", ((int)EMode.MultiWifi));
        SceneManager.LoadScene("LocalPlay");
    }
}