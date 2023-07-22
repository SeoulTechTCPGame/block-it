using UnityEngine;
using UnityEngine.UI;
using static Singleton;

public class OptionalController : MonoBehaviour
{
    [SerializeField] Slider _bgmSlider;
    [SerializeField] Image _bgmImage;
    [SerializeField] Slider _soundEffectSlider;
    [SerializeField] Image _soundEffectImage;
    [SerializeField] Sprite[] _soundSprites;
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

        // ����� ���� ���� �����̴��� �ݿ�
        LoadSettings();

        _bgmSlider.onValueChanged.AddListener(OnBgmVolumeChanged);
        _soundEffectSlider.onValueChanged.AddListener(OnSoundEffectVolumeChanged);
        _vibrationToggle.onValueChanged.AddListener(OnVibrationToggleChanged);

        _englishBtn.onClick.AddListener(() => S.SetLangIndex(0));   //ToDo: 0�� Enum�� English��, 1�� Korean���� ����
        _koreanBtn.onClick.AddListener(() =>S.SetLangIndex(1));
    }
    #region Sound
    private void LoadSettings()
    {
        // BGM ���� �ε�
        float bgmVolume = _soundManager.BgmVolume;
        _bgmSlider.value = bgmVolume;
        UpdateBgmImage(bgmVolume);

        // ���� ����Ʈ ���� �ε�
        float soundEffectVolume = _soundManager.SoundEffectVolume;
        _soundEffectSlider.value = soundEffectVolume;
        UpdateSoundEffectImage(soundEffectVolume);

        // ���� ���� �ε�
        bool vibrationEnabled = _soundManager.IsVibrationEnabled;
        _vibrationToggle.isOn = vibrationEnabled;
    }
    public void OnBgmVolumeChanged(float value)
    {
        // BGM ���� ���� ����
        _soundManager.BgmVolume = value;
        _soundManager.BgmAudioSource.volume = value;

        _soundManager.SaveSettings();
        UpdateBgmImage(value);
    }
    public void OnSoundEffectVolumeChanged(float value)
    {
        // ���� ����Ʈ ���� ���� ����
        _soundManager.SoundEffectVolume = value;
        _soundManager.SoundEffectAudioSource.volume = value;

        _soundManager.SaveSettings();
        UpdateSoundEffectImage(value);
    }
    public void OnVibrationToggleChanged(bool value)
    {
        // ���� ���� ����
        _soundManager.IsVibrationEnabled = value;

        // ��ġ�� ���� ��� ���� ����
        Color backgroundColor = value ? Color.white : Color.gray;
        _vibBackground.color = backgroundColor;

        // ��ġ�� ���� checkmark �¿�� ������
        float targetX = value ? _toggleMinX : _toggleMaxX;
        _vibCheckmark.anchoredPosition = new Vector2(targetX, 0f);

        _soundManager.SaveSettings();
    }
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