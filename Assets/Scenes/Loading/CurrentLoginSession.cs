using UnityEngine;

// ???? ???????? ?????? ?????? ???? ?????? ??????
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

    private bool _isGuest = false;   // ?????????? ???????? ????
    private BlockItUser _user;       // ???????? ???? ???? ???? ???? ????

    public bool IsGuest
    {
        get { return _isGuest; }
        set 
        { 
            _isGuest = value;
            Debug.Log("?????? ???? ????.");
        }
    }

    public BlockItUser user
    {
        get { return _user; }
        set
        { 
            _user = value;
            Debug.Log("????: " + _user.UserId + " ?????? ????");
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
