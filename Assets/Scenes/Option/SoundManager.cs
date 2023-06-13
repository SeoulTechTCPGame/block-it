using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public static SoundManager Instance { get { return instance; } }
    // ���� �� ������
    public float bgmVolume = 1.0f; // BGM ����
    public float soundEffectVolume = 1.0f; // ���� ����Ʈ ����
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

    // BGM ���
    public void PlayBGM(AudioClip bgmClip)
    {
        float volume = bgmVolume;
    }
    // ���� ����Ʈ ���
    public void PlaySoundEffect(AudioClip soundEffectClip)
    {
        float volume = soundEffectVolume;
    }
    // ���� ����
    public void Vibrate()
    {
        if (isVibrationEnabled)
        {
            Handheld.Vibrate();
        }
    }
}