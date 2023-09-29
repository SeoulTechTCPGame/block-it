using UnityEngine;
using UnityEngine.UI;

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
