using UnityEngine;
using UnityEngine.UI;

public class OptionalController : MonoBehaviour
{
    [SerializeField] private Slider bGMSlider;
    [SerializeField] private Slider soundEffectSlider;
    [SerializeField] private Toggle vibrationToggle;
    [SerializeField] private GameObject languageButtons;

    private SoundManager soundManager;

    private void Start()
    {
        soundManager = SoundManager.instance;

        // ����� ���� ���� �����̴��� �ݿ�
        LoadSettings();
    }

    private void LoadSettings()
    {
        // BGM ���� �ε�
        float bgmVolume = soundManager.bgmVolume;
        bGMSlider.value = bgmVolume;

        // ���� ����Ʈ ���� �ε�
        float soundEffectVolume = soundManager.soundEffectVolume;
        soundEffectSlider.value = soundEffectVolume;

        // ���� ���� �ε�
        bool vibrationEnabled = soundManager.isVibrationEnabled;
        vibrationToggle.isOn = vibrationEnabled;
    }

    public void OnBgmVolumeChanged(float value)
    {
        // BGM ���� ���� ����
        soundManager.bgmVolume = value;
        Debug.Log("BGM Volume: " + value);
    }

    public void OnSoundEffectVolumeChanged(float value)
    {
        // ���� ����Ʈ ���� ���� ����
        soundManager.soundEffectVolume = value;
        Debug.Log("Sound Effect Volume: " + value);
    }

    public void OnVibrationToggleChanged(bool value)
    {
        // ���� ���� ����
        soundManager.isVibrationEnabled = value;
        Debug.Log("Vibration Enabled: " + value);
    }
}