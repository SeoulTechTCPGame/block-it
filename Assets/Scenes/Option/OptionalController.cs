using UnityEngine;
using UnityEngine.UI;

public class OptionalController : MonoBehaviour
{
    [SerializeField] Slider _bGMSlider;
    [SerializeField] Slider _soundEffectSlider;
    [SerializeField] Toggle _vibrationToggle;
    [SerializeField] GameObject _languageButtons;

    private SoundManager _soundManager;

    private void Start()
    {
        _soundManager = SoundManager.instance;

        // ����� ���� ���� �����̴��� �ݿ�
        LoadSettings();
    }
    private void LoadSettings()
    {
        // BGM ���� �ε�
        float bgmVolume = _soundManager.bgmVolume;
        _bGMSlider.value = bgmVolume;

        // ���� ����Ʈ ���� �ε�
        float soundEffectVolume = _soundManager.soundEffectVolume;
        _soundEffectSlider.value = soundEffectVolume;

        // ���� ���� �ε�
        bool vibrationEnabled = _soundManager.isVibrationEnabled;
        _vibrationToggle.isOn = vibrationEnabled;
    }
    public void OnBgmVolumeChanged(float value)
    {
        // BGM ���� ���� ����
        _soundManager.bgmVolume = value;
        Debug.Log("BGM Volume: " + value);
    }
    public void OnSoundEffectVolumeChanged(float value)
    {
        // ���� ����Ʈ ���� ���� ����
        _soundManager.soundEffectVolume = value;
        Debug.Log("Sound Effect Volume: " + value);
    }
    public void OnVibrationToggleChanged(bool value)
    {
        // ���� ���� ����
        _soundManager.isVibrationEnabled = value;
        Debug.Log("Vibration Enabled: " + value);
    }
}