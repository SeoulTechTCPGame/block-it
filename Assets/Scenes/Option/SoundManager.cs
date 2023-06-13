using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public static SoundManager Instance { get { return instance; } }
    // 볼륨 값 변수들
    public float bgmVolume = 1.0f; // BGM 볼륨
    public float soundEffectVolume = 1.0f; // 사운드 이펙트 볼륨
    public bool isVibrationEnabled = true;

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
        float volume = bgmVolume;
    }
    // 사운드 이펙트 재생
    public void PlaySoundEffect(AudioClip soundEffectClip)
    {
        float volume = soundEffectVolume;
    }
    // 진동 실행
    public void Vibrate()
    {
        if (isVibrationEnabled)
        {
            Handheld.Vibrate();
        }
    }
}