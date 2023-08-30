using UnityEngine;

// 현재 로그인한 유저의 정보를 담는 싱글톤 클래스
public class CurrentLoginSession : MonoBehaviour
{
    private static CurrentLoginSession _instance;
    public static CurrentLoginSession Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<CurrentLoginSession>();
                if (_instance != null)
                {
                    GameObject singleton = new GameObject("CurrentLoginSession");
                    _instance = singleton.AddComponent<CurrentLoginSession>();
                }
            }
            return _instance;
        }
    }

    private bool _isGuest = false;   // 게스트인지 확인하는 변수
    private BlockItUser _user;       // 게스트가 아닐 경우 유저 정보 저장

    public bool IsGuest
    {
        get { return _isGuest; }
        set 
        { 
            _isGuest = value;
            Debug.Log("게스트 모드 진입.");
        }
    }

    public BlockItUser user
    {
        get { return _user; }
        set
        { 
            _user = value;
            Debug.Log("유저: " + _user.UserId + " 로그인 성공");
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance == this)
        {
            Destroy(gameObject);
        }
    }
}
