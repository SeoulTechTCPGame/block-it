using Mirror;
using System;


[Serializable]
public struct RequestUserProfileMessage : NetworkMessage
{
    public string userId;
}

[Serializable]
public struct ResponseUserProfileMessage : NetworkMessage
{
    public string nickname;
    public int totalUserCount;
    public int userRank;
    public int playCount;
    public int winCount;
    public string profilePicturePath;
}
