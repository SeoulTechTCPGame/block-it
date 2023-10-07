using Mirror;
using System;

#region ���� ������ ��û �� �ۼ����� �޼��� ����
[Serializable]
public struct RequestUserDataMessage : NetworkMessage
{
    public string userId;
}

[Serializable]
public struct ResponseUserDataMessage : NetworkMessage
{
    public string nickname;
    public int totalUserCount;
    public int userRank;
    public int playCount;
    public int winCount;
    public byte[] profileImage;
    public bool success;
}
#endregion

#region ȸ�� ���� �� �ۼ����� �޼��� ����
[Serializable]
public struct RequestUserSignUpMessage : NetworkMessage
{
    public string userId;
    public string nickname;
}

[Serializable]
public struct ResponseUserSignUpMessage : NetworkMessage
{
    public bool success;
}
#endregion

#region �̹��� ���ε� �� �ۼ����� �޼��� ����
[Serializable]
public struct RequestProfileImageUploadMessage : NetworkMessage
{
    public string userId;
    public byte[] image;
}

public struct ResponseProfileImageUploadMessage: NetworkMessage
{
    public bool success;
}
#endregion

#region �г��� ���� �� �ۼ����� �޼��� ����
public struct RequestChangeUserNameMessage: NetworkMessage
{
    public string userId;
    public string nickname;
}

public struct ResponseChangeUserNameMessage : NetworkMessage
{
    public bool success;
}
#endregion