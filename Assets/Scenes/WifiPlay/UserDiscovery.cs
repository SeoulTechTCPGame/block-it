using System;
using System.Net;
using Mirror;
using Mirror.Discovery;
using UnityEngine;

/*
    Documentation: https://mirror-networking.gitbook.io/docs/components/network-discovery
    API Reference: https://mirror-networking.com/docs/api/Mirror.Discovery.NetworkDiscovery.html
*/

public struct DiscoveryRequest : NetworkMessage
{
    // Add public fields (not properties) for whatever information you want
    // sent by clients in their broadcast messages that servers will use.
}

public struct DiscoveryResponse : NetworkMessage
{
    // Add public fields (not properties) for whatever information you want the server
    // to return to clients for them to display or use for establishing a connection.

    // 상대방이 브로드캐스트 메세지를 받으면 유저 정보로 응답
    public string userId;
    public string userName;
    public bool isGuest;

    // 서버 연결에 필요한 정보
    public IPEndPoint EndPoint { get; set; }
    public Uri serverUri;    
    public long serverId;
}

public class UserDiscovery : NetworkDiscoveryBase<DiscoveryRequest, DiscoveryResponse>
{
    #region Unity Callbacks

#if UNITY_EDITOR
    public override void OnValidate()
    {
        base.OnValidate();
    }
#endif

    public override void Start()
    {
        base.Start();
    }

    #endregion

    #region Server

    /// <summary>
    /// Reply to the client to inform it of this server
    /// </summary>
    /// <remarks>
    /// Override if you wish to ignore server requests based on
    /// custom criteria such as language, full server game mode or difficulty
    /// </remarks>
    /// <param name="request">Request coming from client</param>
    /// <param name="endpoint">Address of the client that sent the request</param>
    protected override void ProcessClientRequest(DiscoveryRequest request, IPEndPoint endpoint)
    {
        Debug.Log("Server: 상대방에게 요청");
        base.ProcessClientRequest(request, endpoint);
    }

    /// <summary>
    /// Process the request from a client
    /// </summary>
    /// <remarks>
    /// Override if you wish to provide more information to the clients
    /// such as the name of the host player
    /// </remarks>
    /// <param name="request">Request coming from client</param>
    /// <param name="endpoint">Address of the client that sent the request</param>
    /// <returns>A message containing information about this server</returns>
    protected override DiscoveryResponse ProcessRequest(DiscoveryRequest request, IPEndPoint endpoint) 
    {
        Debug.Log("Server: 상대방의 요청에 응답");
        return new DiscoveryResponse {
            userId = CurrentLoginSession.Singleton.User.Id,
            userName = CurrentLoginSession.Singleton.User.Nickname,
            isGuest = CurrentLoginSession.Singleton.User.IsGuest,
            serverUri = transport.ServerUri(),
            serverId = ServerId
        };
    }

    #endregion

    #region Client

    /// <summary>
    /// Create a message that will be broadcasted on the network to discover servers
    /// </summary>
    /// <remarks>
    /// Override if you wish to include additional data in the discovery message
    /// such as desired game mode, language, difficulty, etc... </remarks>
    /// <returns>An instance of ServerRequest with data to be broadcasted</returns>
    protected override DiscoveryRequest GetRequest()
    {
        Debug.Log("Client: 상대방에게 요청");
        return new DiscoveryRequest();
    }

    /// <summary>
    /// Process the answer from a server
    /// </summary>
    /// <remarks>
    /// A client receives a reply from a server, this method processes the
    /// reply and raises an event
    /// </remarks>
    /// <param name="response">Response that came from the server</param>
    /// <param name="endpoint">Address of the server that replied</param>
    protected override void ProcessResponse(DiscoveryResponse response, IPEndPoint endpoint) {
        response.EndPoint = endpoint;
        UriBuilder realUri = new UriBuilder(response.serverUri)
        {
            Host = response.EndPoint.Address.ToString()
        };
        response.serverUri = realUri.Uri;

        Debug.Log(response.userId + "(닉네임: " + response.userName + ") 유저 찾기 성공.");
        OnServerFound.Invoke(response);
    }
    #endregion
}
