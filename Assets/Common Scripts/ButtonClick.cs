using UnityEngine;
using UnityEngine.UI;

public class ButtonClick : MonoBehaviour
{
    [SerializeField] AudioClip mClip;
    private Button mButton;

    private void Start()
    {
        mButton = GetComponent<Button>();
        mButton.onClick.AddListener(PlayButtonSound);
    }
    private void PlayButtonSound()
    {
        SoundManager.instance.PlaySoundEffect(mClip);
    }
}
