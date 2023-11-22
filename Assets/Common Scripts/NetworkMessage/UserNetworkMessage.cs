using Mirror;
using System;

#region 클라이언트에서 유저 정보 전송 시 메세지
[Serializable]
public struct OpponentUserDataMessage : NetworkMessage
{
    public string userId;
    public string userName;
    public bool isGuest;
}
#endregion

#region 해당 도전 수락 메세지
[Serializable]
public struct ConnectionAcceptanceMessage : NetworkMessage
{
    public bool doAccept;
}
#endregion