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
    [SerializeField] Image _bgmImage;
    [SerializeField] Image _soundEffectImage;
    [SerializeField] Sprite[] sprites;

    private SoundManager _soundManager;
    private float _toggleMinX = -8.5f;
    private float _toggleMaxX = 8.5f;

    private void Start()
    {
        _soundManager = SoundManager.instance;

        // ����� ���� ���� �����̴��� �ݿ�
        LoadSettings();

        _bGMSlider.onValueChanged.AddListener(OnBgmVolumeChanged);
        _soundEffectSlider.onValueChanged.AddListener(OnSoundEffectVolumeChanged);
        _vibrationToggle.onValueChanged.AddListener(OnVibrationToggleChanged);
    }
    private void LoadSettings()
    {
        // BGM ���� �ε�
        float bgmVolume = _soundManager.BgmVolume;
        _bGMSlider.value = bgmVolume;
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
        Debug.Log("BGM Volume: " + value);
        UpdateBgmImage(value);
    }
    public void OnSoundEffectVolumeChanged(float value)
    {
        // ���� ����Ʈ ���� ���� ����
        _soundManager.SoundEffectVolume = value;
        Debug.Log("Sound Effect Volume: " + value);
        UpdateSoundEffectImage(value);

    }
    public void OnVibrationToggleChanged(bool value)
    {
        // ���� ���� ����
        _soundManager.IsVibrationEnabled = value;
        Debug.Log("Vibration Enabled: " + value);

        // ��ġ�� ���� ��� ���� ����
        Color backgroundColor = value ? Color.white : Color.gray;
        _background.color = backgroundColor;

        // ��ġ�� ���� checkmark �¿�� ������
        float targetX = value ? _toggleMinX : _toggleMaxX;
        _checkmark.anchoredPosition = new Vector2(targetX, 0f);
    }
    private void UpdateBgmImage(float volume)
    {
        if (volume <= 0.3f)
        {
            _bgmImage.sprite = sprites[0];
        }
        else if (volume >= 0.7f)
        {
            _bgmImage.sprite = sprites[2];
        }
        else
        {
            _bgmImage.sprite = sprites[1];
        }
    }

    private void UpdateSoundEffectImage(float volume)
    {
        if (volume <= 0.3f)
        {
            _soundEffectImage.sprite = sprites[0];
        }
        else if (volume >= 0.7f)
        {
            _soundEffectImage.sprite = sprites[2];
        }
        else
        {
            _soundEffectImage.sprite = sprites[1];
        }
    }
}