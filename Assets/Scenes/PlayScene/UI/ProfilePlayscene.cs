using UnityEngine;
using UnityEngine.UI;

// 인게임에 뜨는 프로필 사진을 설정한다.
public class ProfilePlayscene : MonoBehaviour
{
    private Sprite _profilePicture;
    [SerializeField] Sprite _defaultPicture;
    [SerializeField] Sprite _aiPicture;

    public void SetProfilePicture(Sprite profilePicture)
    {
        _profilePicture = profilePicture ?? _defaultPicture;
        gameObject.GetComponent<Image>().sprite = _profilePicture;
    }

    public void SetPlayerProfile(bool bActive, Sprite profilePicture = null)
    {
        gameObject.SetActive(bActive);
        if (bActive) 
        {
            SetProfilePicture(profilePicture);
        }
    }

    public void SetAiProfile()
    {
        SetPlayerProfile(true, _aiPicture);
    }
}
