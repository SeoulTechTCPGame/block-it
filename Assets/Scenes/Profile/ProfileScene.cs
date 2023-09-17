using UnityEngine;
using TMPro;
using Mirror;

public class ProfileScene : MonoBehaviour
{
    [Header("정보를 표시할 텍스트")]
    [SerializeField] TMP_Text userName;
    [SerializeField] TMP_Text userRecord;

    // Start is called before the first frame update
    void Start()
    {
        BlockItUser user = CurrentLoginSession.Instance.User;
        user.getUserData();

        userName.text = user.Nickname;

        if (user.PlayCount == 0)
        {
            userRecord.text = "플레이 기록이 없습니다.";
        }
        else
        {
            userRecord.text = user.PlayCount + " 게임, 승률: " + user.WinCount * 100 / user.PlayCount + "%";
        }
    }

    public void OnClickLogoutButton()
    {
        CurrentLoginSession.Instance.Logout();
    }
}
