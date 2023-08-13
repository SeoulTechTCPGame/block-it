using UnityEngine;
using UnityEngine.UI;
using static Singleton;

// 모든 옵션의 효과를 다루는 클래스
public class OptionalController : MonoBehaviour
{
    [SerializeField] Slider _bgmSlider;
    [SerializeField] Image _bgmImage;
    [SerializeField] Slider _soundEffectSlider;
    [SerializeField] Image _soundEffectImage;
    [SerializeField] Sprite[] _soundSprites;    // BGM과 Sound Effect의 바꿀 이미지의 스프라이트 모음
    [SerializeField] Toggle _vibrationToggle;
    [SerializeField] Image _vibBackground;
    [SerializeField] RectTransform _vibCheckmark;
    [SerializeField] Button _englishBtn;
    [SerializeField] Button _koreanBtn;

    private SoundManager _soundManager;
    private float _toggleMinX = -8.5f;
    private float _toggleMaxX = 8.5f;

    private void Start()
    {
        _soundManager = SoundManager.instance;

        // 저장된 설정 값을 슬라이더에 반영
        LoadSettings();

        _bgmSlider.onValueChanged.AddListener(OnBgmVolumeChanged);
        _soundEffectSlider.onValueChanged.AddListener(OnSoundEffectVolumeChanged);
        _vibrationToggle.onValueChanged.AddListener(OnVibrationToggleChanged);

        _englishBtn.onClick.AddListener(() => S.SetLangIndex(((int)Enums.ELanguage.EN)));
        _koreanBtn.onClick.AddListener(() =>S.SetLangIndex(((int)Enums.ELanguage.KR)));
    }
    #region Sound
    // 저장된 값 불러오기
    private void LoadSettings()
    {
        // BGM 볼륨 로드
        float bgmVolume = _soundManager.BgmVolume;
        _bgmSlider.value = bgmVolume;
        UpdateBgmImage(bgmVolume);

        // 사운드 이펙트 볼륨 로드
        float soundEffectVolume = _soundManager.SoundEffectVolume;
        _soundEffectSlider.value = soundEffectVolume;
        UpdateSoundEffectImage(soundEffectVolume);

        // 진동 설정 로드
        bool vibrationEnabled = _soundManager.IsVibrationEnabled;
        _vibrationToggle.isOn = vibrationEnabled;
    }
    public void OnBgmVolumeChanged(float value)
    {
        // BGM 볼륨 값을 변경
        _soundManager.BgmVolume = value;
        _soundManager.BgmAudioSource.volume = value;

        _soundManager.SaveSettings();
        UpdateBgmImage(value);
    }
    public void OnSoundEffectVolumeChanged(float value)
    {
        // 사운드 이펙트 볼륨 값을 변경
        _soundManager.SoundEffectVolume = value;
        _soundManager.SoundEffectAudioSource.volume = value;

        _soundManager.SaveSettings();
        UpdateSoundEffectImage(value);
    }
    public void OnVibrationToggleChanged(bool value)
    {
        // 진동 설정 변경
        _soundManager.IsVibrationEnabled = value;

        // 위치에 따라 배경 색상 변경
        Color backgroundColor = value ? Color.white : Color.gray;
        _vibBackground.color = backgroundColor;

        // 위치에 따라 checkmark 좌우로 움직임
        float targetX = value ? _toggleMinX : _toggleMaxX;
        _vibCheckmark.anchoredPosition = new Vector2(targetX, 0f);

        _soundManager.SaveSettings();
    }
    //  BGM과 Sound Effect의 바꿀 이미지를 지정
    private void UpdateBgmImage(float volume)
    {
        if (volume <= 0.3f)
        {
            _bgmImage.sprite = _soundSprites[0];
        }
        else if (volume >= 0.7f)
        {
            _bgmImage.sprite = _soundSprites[2];
        }
        else
        {
            _bgmImage.sprite = _soundSprites[1];
        }
    }
    private void UpdateSoundEffectImage(float volume)
    {
        if (volume <= 0.3f)
        {
            _soundEffectImage.sprite = _soundSprites[0];
        }
        else if (volume >= 0.7f)
        {
            _soundEffectImage.sprite = _soundSprites[2];
        }
        else
        {
            _soundEffectImage.sprite = _soundSprites[1];
        }
    }
    #endregion
}