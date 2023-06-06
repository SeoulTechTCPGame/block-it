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

        // 저장된 설정 값을 슬라이더에 반영
        LoadSettings();
    }

    private void LoadSettings()
    {
        // BGM 볼륨 로드
        float bgmVolume = soundManager.bgmVolume;
        bGMSlider.value = bgmVolume;

        // 사운드 이펙트 볼륨 로드
        float soundEffectVolume = soundManager.soundEffectVolume;
        soundEffectSlider.value = soundEffectVolume;

        // 진동 설정 로드
        bool vibrationEnabled = soundManager.isVibrationEnabled;
        vibrationToggle.isOn = vibrationEnabled;
    }

    public void OnBgmVolumeChanged(float value)
    {
        // BGM 볼륨 값을 변경
        soundManager.bgmVolume = value;
        Debug.Log("BGM Volume: " + value);
    }

    public void OnSoundEffectVolumeChanged(float value)
    {
        // 사운드 이펙트 볼륨 값을 변경
        soundManager.soundEffectVolume = value;
        Debug.Log("Sound Effect Volume: " + value);
    }

    public void OnVibrationToggleChanged(bool value)
    {
        // 진동 설정 변경
        soundManager.isVibrationEnabled = value;
        Debug.Log("Vibration Enabled: " + value);
    }
}