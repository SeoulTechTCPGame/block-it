using UnityEngine;

// 유저 정보
public class UserData : MonoBehaviour
{
    public static UserData instance;
    private string _userName;
    private string _userId;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(instance.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }
    /** load login info **/
    public void LoadUserInfo()
    {
        _userName = PlayerPrefs.GetString("User_Display_Name", null);
        _userId = PlayerPrefs.GetString("User_Id", null);
    }
    /** save login info **/
    public void SaveUserInfo()
    {
        PlayerPrefs.SetString("User_Display_Name", _userName);
        PlayerPrefs.SetString("User_Id", _userId);
    }
}