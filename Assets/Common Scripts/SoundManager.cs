using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public static SoundManager Instance { get { return instance; } }
    // ���� �� ������
    public float BgmVolume { get; set; } = 1.0f; // BGM ����
    public float SoundEffectVolume { get; set; } = 1.0f; // ���� ����Ʈ ����
    public bool IsVibrationEnabled { get; set; } = true;
    public AudioClip[] Bgm;
    public AudioSource BgmAudioSource; // BGM�� ����� ����� �ҽ�
    public AudioSource SoundEffectAudioSource; // ���� ����Ʈ�� ����� ����� �ҽ�

    private string _currentScene; // ���� ���� �̸��� ������ ����

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            BgmAudioSource = gameObject.AddComponent<AudioSource>();
            SoundEffectAudioSource = gameObject.AddComponent<AudioSource>();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }
    private void Start()
    {
        _currentScene = SceneManager.GetActiveScene().name;
        SceneManager.sceneLoaded += OnSceneLoaded;

        LoadSettings();

        PlayBgm(_currentScene);
    }
    #region ����
    private void LoadSettings()
    {
        // BGM ���� �ε�
        BgmVolume = PlayerPrefs.GetFloat("SoundManager_BgmVolume", 1.0f);

        // ���� ����Ʈ ���� �ε�
        SoundEffectVolume = PlayerPrefs.GetFloat("SoundManager_SoundEffectVolume", 1.0f);

        // ��ü ȭ�� ��� �ε�
        IsVibrationEnabled = PlayerPrefs.GetInt("SoundManager_IsVibrationEnabled", 0) == 1;
    }
    public void SaveSettings()
    {
        // BGM ���� ����
        PlayerPrefs.SetFloat("SoundManager_BgmVolume", BgmVolume);

        // ���� ����Ʈ ���� ����
        PlayerPrefs.SetFloat("SoundManager_SoundEffectVolume", SoundEffectVolume);

        // ���� ��� ����
        PlayerPrefs.SetInt("SoundManager_IsVibrationEnabled", IsVibrationEnabled ? 1 : 0);

        // PlayerPrefs �����͸� ��ũ�� ����
        PlayerPrefs.Save();
    }
    #endregion
    #region �ɼ� ���
    // BGM ���
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != _currentScene)
        {
            if (scene.name == "InGame")
            {
                // ���� ���� �ٸ� BGM�� ��� ���ο� BGM ���
                StopBgm();
                PlayBgm(scene.name);
            }
            else if (scene.name == "GameResult")
            {
                StopBgm();
                PlayBgm(scene.name);
            }
            else
            {
                // ���� ���� ������ BGM�� ��� �̾ ���
            }
        }
    }
    private void PlayBgm(string sceneName)
    {
        if (BgmAudioSource == null)
        {
            Debug.LogError("AudioSource ������Ʈ�� �����ϴ�!");
            return;
        }
        // ToDo: Enums BGM �ε���, �� ����
        AudioClip bgmClip;
        if (sceneName == "Loading")
        {
            bgmClip = Bgm[0];
        }
        else if (sceneName == "Home" || sceneName == "Option" || sceneName == "Profile")
        {
            bgmClip = Bgm[1];
        }
        else
        {
            // ���� ����
            bgmClip = Bgm[2];
        }

        BgmAudioSource.clip = bgmClip;
        BgmAudioSource.volume = BgmVolume;
        BgmAudioSource.Play();
    }
    private void StopBgm()
    {
        BgmAudioSource.Stop();
    }
    // ���� ����Ʈ ���
    public void PlaySoundEffect(AudioClip soundEffectClip)
    {
        SoundEffectAudioSource.PlayOneShot(soundEffectClip, SoundEffectVolume);
    }
    // ���� ����
    public void Vibrate()
    {
        if (IsVibrationEnabled)
        {
            Handheld.Vibrate();
        }
    }
    #endregion
}