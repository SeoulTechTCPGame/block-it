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

        // ��ġ�� ���� ��� ���� ����
        Color backgroundColor = value ? Color.white : Color.gray;
        _background.color = backgroundColor;

        // ��ġ�� ���� checkmark �¿�� ������
        float targetX = value ? _toggleMinX : _toggleMaxX;
        _checkmark.anchoredPosition = new Vector2(targetX, 0f);
    }
}