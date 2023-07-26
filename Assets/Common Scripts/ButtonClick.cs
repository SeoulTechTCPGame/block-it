using UnityEngine;
using UnityEngine.UI;

public class ButtonClick : MonoBehaviour
{
    [SerializeField] AudioClip mClip;

    public void PlayButtonSound()
    {
        SoundManager.instance.PlaySoundEffect(mClip);
    }
}
