using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public static SoundManager Instance { get { return instance; } }
    // ���� �� ������
    public float BgmVolume = 1.0f; // BGM ����
    public float SoundEffectVolume = 1.0f; // ���� ����Ʈ ����
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

    // BGM ���
    public void PlayBGM(AudioClip bgmClip)
    {
        float volume = BgmVolume;
    }
    // ���� ����Ʈ ���
    public void PlaySoundEffect(AudioClip soundEffectClip)
    {
        float volume = SoundEffectVolume;
    }
    // ���� ����
    public void Vibrate()
    {
        if (IsVibrationEnabled)
        {
            Handheld.Vibrate();
        }
    }
}