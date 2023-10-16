using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Transactions;
using UnityEngine;
using UnityEngine.Networking;

public class BlockItUserDataManager : MonoBehaviour
{
    [Header("서버의 주소")]
    [Tooltip("API 서버 주소. 주소 마지막에 /는 빼주세요.")]
    [SerializeField] string serverUrl = "http://localhost:21527/blockit";

    [Header("위의 주소 입력 필드 사용 여부")]
    [Tooltip("체크하지 않을 경우 ConnectionInfo.json에서 서버 정보를 불러옵니다.")]
    [SerializeField] bool _usePropertyField = false;

    //[Tooltip("회원가입 요청 주소")]
    private string registerUrl;

    // [Tooltip("회원 정보 요청 주소")]
    private string userDataRequestUrl;

    // [Tooltip("회원 정보 수정 요청 주소")]
    private string userDataUpdateRequestUrl;

    // Tooltip("프로필 사진 변경 요청 주소")]
    private string userProfileImageUploadUrl;

    // [Tooltip("프로필 사진 요청 주소")]
    private string userProfileImageRequestUrl;

    // 연결 정보 가져오기
    private string _jsonFilepath = @"Assets/Resources/Json/ConnectionInfo.json";

    private void Start()
    {
        if (!_usePropertyField) {
            string jsonText = System.IO.File.ReadAllText(_jsonFilepath);
            JObject jsonData = JObject.Parse(jsonText);

            serverUrl = "https://" + (string)jsonData["Server"]["Address"] + "/blockit";
        }

        registerUrl = serverUrl + "/register";
        userDataRequestUrl = serverUrl + "/user";
        userDataUpdateRequestUrl = serverUrl + "/user/update/nickname";
        userProfileImageUploadUrl = serverUrl + "/user/update/img";
        userProfileImageRequestUrl = serverUrl + "/user/img";
    }

    #region 싱글톤 패턴
    private static BlockItUserDataManager _singleton;   // 싱글톤 구현

    public static BlockItUserDataManager Singleton
    {
        get
        {
            if (_singleton == null)
            {
                _singleton = FindObjectOfType<BlockItUserDataManager>();
                if (_singleton != null)
                {
                    GameObject singleton = new GameObject("CurrentLoginSession");
                    _singleton = singleton.AddComponent<BlockItUserDataManager>();
                }
            }
            return _singleton;
        }
    }

    public void Awake()
    {
        if (_singleton == null)
        {
            _singleton = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_singleton == this)
        {
            Destroy(gameObject);
        }
    }
    #endregion

    #region 회원가입 요청 POST
    public void Register(BlockItUser user)
    {
        string json = JsonConvert.SerializeObject(user);
        StartCoroutine(RegisterCoroutine(json));
    }

    private IEnumerator RegisterCoroutine(string json)
    {
        using (UnityWebRequest www = UnityWebRequest.PostWwwForm(registerUrl, "POST"))
        {
            // Json을 직렬화
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            Debug.Log(json);

            // 헤더 추가
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            // POST
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Server Response: " + www.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Server Error: " + www.error);
            }
        }
    }
    #endregion

    #region 유저 정보 요청 POST
    public void GetUserData(BlockItUser targetUser, System.Action onComplete = default)
    {
        StartCoroutine(GetUserDataCoroutine(targetUser, onComplete));
    }

    private IEnumerator GetUserDataCoroutine(BlockItUser targetUser, System.Action onComplete)
    {
        using (UnityWebRequest www = new UnityWebRequest(userDataRequestUrl, "POST"))
        {
            string json = JsonConvert.SerializeObject(targetUser);
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                JObject jsonData = JObject.Parse(www.downloadHandler.text);
                Debug.Log(www.downloadHandler.text);
                targetUser.UpdateData(
                    (string)jsonData["nickname"],
                    (int)jsonData["winCount"],
                    (int)jsonData["playCount"]);
                Debug.Log("Firebase GID '" + targetUser.Id + "'(닉네임: " + targetUser.Nickname + ") 정보 수신 성공");
            }
            else
            {
                Debug.LogError("Server Error: " + www.error);
            }
        }

        onComplete?.Invoke();
    }
    #endregion

    #region 닉네임 변경 요청 POST
    public void UpdateUserName(BlockItUser targetUser, string newName, System.Action onComplete)
    {
        StartCoroutine(UpdateUserNameCoroutine(targetUser, newName, onComplete));
    }

    private IEnumerator UpdateUserNameCoroutine(BlockItUser targetUser, string newName, System.Action onComplete)
    {
        using (UnityWebRequest www = new UnityWebRequest(userDataUpdateRequestUrl, "POST"))
        {
            BlockItUser newData = new BlockItUser(targetUser.Id, newName);
            string json = JsonConvert.SerializeObject(newData);
            
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                if (www.downloadHandler.text.Equals("Success"))
                {
                    CurrentLoginSession.Singleton.User.UpdateData(
                        newName,
                        targetUser.PlayCount,
                        targetUser.WinCount);
                    CurrentLoginSession.Singleton.User.IsSuccess = true;
                    Debug.Log("Firebase GID '" + CurrentLoginSession.Singleton.User.Id + "'(닉네임: " + CurrentLoginSession.Singleton.User.Nickname + ") 닉네임 변경 완료");
                }
                else
                {
                    CurrentLoginSession.Singleton.User.IsSuccess = false;
                    Debug.Log("Firebase GID '" + CurrentLoginSession.Singleton.User.Id + "'(닉네임: " + CurrentLoginSession.Singleton.User.Nickname + ") 닉네임 변경 실패, 닉네임 중복");
                }
            }
            else
            {
                Debug.Log("Firebase GID '" + CurrentLoginSession.Singleton.User.Id + "'(닉네임: " + CurrentLoginSession.Singleton.User.Nickname + ")" + www.error);
                Debug.LogError("Server Error: " + www.error);
            }
        }

        onComplete?.Invoke();
    }
    #endregion

    #region 프로필 이미지 업로드 POST
    public void UploadProfileImage(BlockItUser targetUser, byte[] imageData, System.Action onComplete)
    {
        StartCoroutine(UploadProfileImageCoroutine(targetUser, imageData, onComplete));
    }

    private IEnumerator UploadProfileImageCoroutine(BlockItUser targetUser, byte[] imageData, System.Action onComplete)
    {
        WWWForm form = new WWWForm();
        form.AddBinaryData("file", imageData);
        form.AddField("userId", targetUser.Id);

        UnityWebRequest www = UnityWebRequest.Post(userProfileImageUploadUrl, form);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            CurrentLoginSession.Singleton.User.IsSuccess = true;
            Debug.Log("Firebase GID '" + CurrentLoginSession.Singleton.User.Id + "'(닉네임: " + CurrentLoginSession.Singleton.User.Nickname + ") 프로필 사진 변경 완료");
        }
        else
        {
            CurrentLoginSession.Singleton.User.IsSuccess = false;
            Debug.Log("Firebase GID '" + CurrentLoginSession.Singleton.User.Id + "'(닉네임: " + CurrentLoginSession.Singleton.User.Nickname + ") 프로필 사진 변경 실패, 오류 메세지: " + www.error);
        }

        onComplete?.Invoke();
    }
    #endregion

    #region 프로필 사진 요청 POST
    public void GetProfileImage(BlockItUser targetUser, System.Action onComplete = default)
    {
        StartCoroutine(GetProfileImageCoroutine(targetUser, onComplete));
    }

    private IEnumerator GetProfileImageCoroutine(BlockItUser targetUser, System.Action onComplete)
    {
        UnityWebRequest www = UnityWebRequest.Get(userProfileImageRequestUrl + "/" + targetUser.Id);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            targetUser.ProfileImg = www.downloadHandler.data;
            Debug.Log("Firebase GID '" + CurrentLoginSession.Singleton.User.Id + "'(닉네임: " + CurrentLoginSession.Singleton.User.Nickname + ") 프로필 사진 다운로드 완료");
        }
        else
        {
            Debug.LogError("Firebase GID '" + CurrentLoginSession.Singleton.User.Id + "'(닉네임: " + CurrentLoginSession.Singleton.User.Nickname + ") 프로필 사진 다운로드 실패");
        }
        onComplete?.Invoke();
    }
    #endregion
}