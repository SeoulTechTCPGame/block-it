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
    public byte[] profileImage;
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

#region 이미지 업로드 시 송수신할 메세지 정의
[Serializable]
public struct RequestProfileImageUploadMessage : NetworkMessage
{
    public string userId;
    public byte[] image;
}

public struct ResponseProfileImageUploadMessage : NetworkMessage
{
    public bool success;
}
#endregion

#region 닉네임 변경 시 송수신할 메세지 정의
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