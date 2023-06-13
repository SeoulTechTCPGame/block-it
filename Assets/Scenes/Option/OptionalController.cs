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

        // 저장된 설정 값을 슬라이더에 반영
        LoadSettings();
    }
    private void LoadSettings()
    {
        // BGM 볼륨 로드
        float bgmVolume = _soundManager.bgmVolume;
        _bGMSlider.value = bgmVolume;

        // 사운드 이펙트 볼륨 로드
        float soundEffectVolume = _soundManager.soundEffectVolume;
        _soundEffectSlider.value = soundEffectVolume;

        // 진동 설정 로드
        bool vibrationEnabled = _soundManager.isVibrationEnabled;
        _vibrationToggle.isOn = vibrationEnabled;
    }
    public void OnBgmVolumeChanged(float value)
    {
        // BGM 볼륨 값을 변경
        _soundManager.bgmVolume = value;
        Debug.Log("BGM Volume: " + value);
    }
    public void OnSoundEffectVolumeChanged(float value)
    {
        // 사운드 이펙트 볼륨 값을 변경
        _soundManager.soundEffectVolume = value;
        Debug.Log("Sound Effect Volume: " + value);
    }
    public void OnVibrationToggleChanged(bool value)
    {
        // 진동 설정 변경
        _soundManager.isVibrationEnabled = value;
        Debug.Log("Vibration Enabled: " + value);
    }
}