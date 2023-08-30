using Mirror;
using System;

#region ���� ������ ��û �� �۽��� �޼��� ���
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
    public string profilePicturePath;
    public bool success;
}
#endregion

#region ȸ�� ���� �� �۽��� �޼��� ���
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