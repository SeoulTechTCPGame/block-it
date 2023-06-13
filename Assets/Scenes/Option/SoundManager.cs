using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public static SoundManager Instance { get { return instance; } }
    // 볼륨 값 변수들
    public float BgmVolume = 1.0f; // BGM 볼륨
    public float SoundEffectVolume = 1.0f; // 사운드 이펙트 볼륨
    public bool IsVibrationEnabled = true;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    // BGM 재생
    public void PlayBGM(AudioClip bgmClip)
    {
        float volume = BgmVolume;
    }
    // 사운드 이펙트 재생
    public void PlaySoundEffect(AudioClip soundEffectClip)
    {
        float volume = SoundEffectVolume;
    }
    // 진동 실행
    public void Vibrate()
    {
        if (IsVibrationEnabled)
        {
            Handheld.Vibrate();
        }
    }
}