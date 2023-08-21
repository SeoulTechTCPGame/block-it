using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public static SoundManager Instance { get { return instance; } }
    // 볼륨 값 변수들
    public float BgmVolume { get; set; } = 1.0f; // BGM 볼륨
    public float SoundEffectVolume { get; set; } = 1.0f; // 사운드 이펙트 볼륨
    public bool IsVibrationEnabled { get; set; } = true;
    public AudioClip[] Bgm;
    public AudioSource BgmAudioSource; // BGM을 재생할 오디오 소스
    public AudioSource SoundEffectAudioSource; // 사운드 이펙트를 재생할 오디오 소스

    private string _currentScene; // 현재 씬의 이름을 저장할 변수

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
    #region 저장
    private void LoadSettings()
    {
        // BGM 볼륨 로드
        BgmVolume = PlayerPrefs.GetFloat("SoundManager_BgmVolume", 1.0f);

        // 사운드 이펙트 볼륨 로드
        SoundEffectVolume = PlayerPrefs.GetFloat("SoundManager_SoundEffectVolume", 1.0f);

        // 전체 화면 토글 로드
        IsVibrationEnabled = PlayerPrefs.GetInt("SoundManager_IsVibrationEnabled", 0) == 1;
    }
    public void SaveSettings()
    {
        // BGM 볼륨 저장
        PlayerPrefs.SetFloat("SoundManager_BgmVolume", BgmVolume);

        // 사운드 이펙트 볼륨 저장
        PlayerPrefs.SetFloat("SoundManager_SoundEffectVolume", SoundEffectVolume);

        // 진동 토글 저장
        PlayerPrefs.SetInt("SoundManager_IsVibrationEnabled", IsVibrationEnabled ? 1 : 0);

        // PlayerPrefs 데이터를 디스크에 저장
        PlayerPrefs.Save();
    }
    #endregion
    #region 옵션 기능
    // BGM 재생
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != _currentScene)
        {
            if (scene.name == "InGame")
            {
                // 이전 씬과 다른 BGM인 경우 새로운 BGM 재생
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
                // 이전 씬과 동일한 BGM인 경우 이어서 재생
            }
        }
    }
    private void PlayBgm(string sceneName)
    {
        if (BgmAudioSource == null)
        {
            Debug.LogError("AudioSource 컴포넌트가 없습니다!");
            return;
        }
        // ToDo: Enums BGM 인덱스, 씬 매핑
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
            // 게임 도중
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
    // 사운드 이펙트 재생
    public void PlaySoundEffect(AudioClip soundEffectClip)
    {
        SoundEffectAudioSource.PlayOneShot(soundEffectClip, SoundEffectVolume);
    }
    // 진동 실행
    public void Vibrate()
    {
        if (IsVibrationEnabled)
        {
            Handheld.Vibrate();
        }
    }
    #endregion
}