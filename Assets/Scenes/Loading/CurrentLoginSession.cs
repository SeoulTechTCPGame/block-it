using UnityEngine;

// ���� �α����� ������ ������ ��� �̱��� Ŭ����
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

    private bool _isGuest = false;   // �Խ�Ʈ���� Ȯ���ϴ� ����
    private BlockItUser _user;       // �Խ�Ʈ�� �ƴ� ��� ���� ���� ����

    public bool IsGuest
    {
        get { return _isGuest; }
        set 
        { 
            _isGuest = value;
            Debug.Log("�Խ�Ʈ ��� ����.");
        }
    }

    public BlockItUser user
    {
        get { return _user; }
        set
        { 
            _user = value;
            Debug.Log("����: " + _user.UserId + " �α��� ����");
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
