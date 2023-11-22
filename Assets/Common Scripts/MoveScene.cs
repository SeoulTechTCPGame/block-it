using UnityEngine;
using UnityEngine.SceneManagement;
using static Enums;

// 씬 이동을 담당하는 클래스
public class MoveScene : MonoBehaviour
{
    public void ToLoading()   // 로딩으로 이동
    {
        SceneManager.LoadScene("Loading");
    }

    public void ToOption()  // 옵션으로 이동
    {
        SceneManager.LoadScene("Option");
    }

    public void ToProfile() // 프로필으로 이동
    {
        SceneManager.LoadScene("Profile");
    }

    public void ToHome()    // 홈으로 이동
    {
        SceneManager.LoadScene("Home");
    }

    public void ToWifiLoby()    // 로비로 이동
    {
        SceneManager.LoadScene("Search");
    }

    public void ToCredit()  // 크레딧으로 이동
    {
        SceneManager.LoadScene("Credit");
    }

    public void ToLevelSelect() // AI 레벨 선택으로 이동
    {
        SceneManager.LoadScene("LevelSelect");
    }

    public void ToLocalPlayFriend() // 로컬 플레이로 이동
    {
        PlayerPrefs.SetInt("GameMode", ((int)EMode.Local));
        SceneManager.LoadScene("LocalPlay");
    }

    public void ToLocalPlayAIBeginner() // 초심자 난이도 AI 플레이로 이동
    {
        PlayerPrefs.SetInt("GameMode", ((int)EMode.AI));
        PlayerPrefs.SetInt("AILevel", ((int)ELevel.Beginner));
        SceneManager.LoadScene("LocalPlay");
    }

    public void ToLocalPlayAIIntermediate() // 중급자 난이도 AI 플레이로 이동
    {
        PlayerPrefs.SetInt("GameMode", ((int)EMode.AI));
        PlayerPrefs.SetInt("AILevel", ((int)ELevel.Intermediate));
        SceneManager.LoadScene("LocalPlay");
    }

    public void ToLocalPlayAIAdvanced() // 상급자 난이도 AI 플레이로 이동
    {
        PlayerPrefs.SetInt("GameMode", ((int)EMode.AI));
        PlayerPrefs.SetInt("AILevel", ((int)ELevel.Advanced));
        SceneManager.LoadScene("LocalPlay");
    }

    public void ToLocalPlayAIMaster()   // 마스터 난이도 AI 플레이로 이동
    {
        PlayerPrefs.SetInt("GameMode", ((int)EMode.AI));
        PlayerPrefs.SetInt("AILevel", ((int)ELevel.Master));
        SceneManager.LoadScene("LocalPlay");
    }

    public void ToLocalPlayMultiWifi()  // 멀티 모드로 이동
    {
        PlayerPrefs.SetInt("GameMode", ((int)EMode.MultiWifi));
        SceneManager.LoadScene("LocalPlay");
    }
}