using Mirror;
using System;

#region 유저 정보를 요청 시 송신할 메세지 약속
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

#region 회원 가입 시 송신할 메세지 약속
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