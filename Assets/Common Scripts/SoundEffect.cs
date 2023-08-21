using UnityEngine;

public class SoundEffect: MonoBehaviour
{
    [SerializeField] AudioClip mClip;

    public void PlayButtonSound()
    {
        SoundManager.instance.PlaySoundEffect(mClip);
    }
}