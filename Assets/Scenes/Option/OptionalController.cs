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
        float bgmVolume = _soundManager.BgmVolume;
        _bGMSlider.value = bgmVolume;

        // ���� ����Ʈ ���� �ε�
        float soundEffectVolume = _soundManager.SoundEffectVolume;
        _soundEffectSlider.value = soundEffectVolume;

        // ���� ���� �ε�
        bool vibrationEnabled = _soundManager.IsVibrationEnabled;
        _vibrationToggle.isOn = vibrationEnabled;
    }
    public void OnBgmVolumeChanged(float value)
    {
        // BGM ���� ���� ����
        _soundManager.BgmVolume = value;
        Debug.Log("BGM Volume: " + value);
    }
    public void OnSoundEffectVolumeChanged(float value)
    {
        // ���� ����Ʈ ���� ���� ����
        _soundManager.SoundEffectVolume = value;
        Debug.Log("Sound Effect Volume: " + value);
    }
    public void OnVibrationToggleChanged(bool value)
    {
        // ���� ���� ����
        _soundManager.IsVibrationEnabled = value;
        Debug.Log("Vibration Enabled: " + value);
    }
}