using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;

public class ProfileScene : MonoBehaviour
{
    [Header("정보를 표시할 텍스트")]
    [SerializeField] TMP_Text userName;
    [SerializeField] TMP_Text userRecord;

    [SerializeField] Image targetImage; // UI Image의 경우

    // Start is called before the first frame update
    void Start()
    {
        BlockItUser user = CurrentLoginSession.Instance.User;
        Texture2D texture = new Texture2D(2, 2);

        user.getUserData(); // 유저 정보 가져오기
        userName.text = user.Nickname; // 닉네임 표시
        texture.LoadImage(user.ProfileImage);

        // 플레이어 정보 로딩
        if (user.PlayCount == 0)
        {
            user.getUserData();

            userName.text = user.Nickname;

            if (user.PlayCount == 0)
            {
                userRecord.text = "플레이 기록이 없습니다."; ;
            }
            else
            {
                userRecord.text = user.PlayCount + " 게임, 승률: " + user.WinCount * 100 / user.PlayCount + "%";
            }
        }

        // 이미지 로딩
        if (targetImage != null)
        {
            targetImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
    }

    public void OnClickLogoutButton()
    {
        CurrentLoginSession.Instance.Logout();
    }
}
