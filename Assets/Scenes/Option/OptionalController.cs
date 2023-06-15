using UnityEngine;
using UnityEngine.UI;

public class OptionalController : MonoBehaviour
{
    [SerializeField] Slider _bGMSlider;
    [SerializeField] Slider _soundEffectSlider;
    [SerializeField] Toggle _vibrationToggle;
    [SerializeField] GameObject _languageButtons;
    [SerializeField] Image _background;
    [SerializeField] RectTransform _checkmark;

    private SoundManager _soundManager;
    private float _toggleMinX = -8.5f;
    private float _toggleMaxX = 8.5f;

    private void Start()
    {
        _soundManager = SoundManager.instance;

        // 저장된 설정 값을 슬라이더에 반영
        LoadSettings();

        _bGMSlider.onValueChanged.AddListener(OnBgmVolumeChanged);
        _soundEffectSlider.onValueChanged.AddListener(OnSoundEffectVolumeChanged);
        _vibrationToggle.onValueChanged.AddListener(OnVibrationToggleChanged);
    }
    private void LoadSettings()
    {
        // BGM 볼륨 로드
        float bgmVolume = _soundManager.BgmVolume;
        _bGMSlider.value = bgmVolume;

        // 사운드 이펙트 볼륨 로드
        float soundEffectVolume = _soundManager.SoundEffectVolume;
        _soundEffectSlider.value = soundEffectVolume;

        // 진동 설정 로드
        bool vibrationEnabled = _soundManager.IsVibrationEnabled;
        _vibrationToggle.isOn = vibrationEnabled;
    }
    public void OnBgmVolumeChanged(float value)
    {
        // BGM 볼륨 값을 변경
        _soundManager.BgmVolume = value;
        Debug.Log("BGM Volume: " + value);
    }
    public void OnSoundEffectVolumeChanged(float value)
    {
        // 사운드 이펙트 볼륨 값을 변경
        _soundManager.SoundEffectVolume = value;
        Debug.Log("Sound Effect Volume: " + value);
    }
    public void OnVibrationToggleChanged(bool value)
    {
        // 진동 설정 변경
        _soundManager.IsVibrationEnabled = value;
        Debug.Log("Vibration Enabled: " + value);

        // 위치에 따라 배경 색상 변경
        Color backgroundColor = value ? Color.white : Color.gray;
        _background.color = backgroundColor;

        // 위치에 따라 checkmark 좌우로 움직임
        float targetX = value ? _toggleMinX : _toggleMaxX;
        _checkmark.anchoredPosition = new Vector2(targetX, 0f);
    }
}