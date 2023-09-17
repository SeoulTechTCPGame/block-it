using System.Security.Cryptography;
using System;
using UnityEngine;

// ���� �α����� ������ ������ ��� �̱��� Ŭ����
public class CurrentLoginSession : MonoBehaviour
{
    // �̱��� Ŭ���� ����
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

    // ������
    private bool _hasLogined = false;  // �α��� �ߴ��� Ȯ���ϴ� ����
    private BlockItUser _user;         // �Խ�Ʈ�� �ƴ� ��� ���� ���� ����

    public bool HasLogined
    {
        get { return _hasLogined; }
    }

    public BlockItUser User
    {
        get { return _user; }
    }

    // �α��� �� ���� ���� ����
    public void Login(BlockItUser user)
    {
        _user = user;
        _hasLogined = true;
    }

    // �α׾ƿ� �� ���� ���� ����
    public void Logout()
    {
        _user = null;
        _hasLogined = false;
        Debug.Log("���ӿ��� �α׾ƿ� ��");
    }

    // �Խ�Ʈ ��� ����
    public void StartGuestMode()
    {
        _hasLogined = true;
        _user = new BlockItUser();
        Debug.Log("�Խ�Ʈ ��� ����, �г���: " + _user.Nickname);
    }

    // ������ CurrentLoginSession Ŭ������ Component�� ���� GameObject�� ������
    // ��쿡 ���� GameObject ����
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
