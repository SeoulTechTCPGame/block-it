using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System;
using UnityEngine;
using TMPro;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using Mirror;

public class GetUserInfo : NetworkBehaviour
{
    [Header("정보를 불러올 유저의 ID")]
    [SerializeField] string userId;

    [Header("정보를 표시할 텍스트")]
    [SerializeField] TMP_Text userName;
    [SerializeField] TMP_Text userRecord;

    /* DB Connetion String Information
    private string dbAddress;
    private string dbPortNumber;
    private string dbName;
    private string dbId;
    private string dbPassword; */

    // 불러온 정보들
    private int gameScore;
    private int gameCount;
    private int winCount;

    // Start is called before the first frame update
    void Start()
    {
        NetworkManager.singleton.StartClient();
        NetworkClient.RegisterHandler<ResponseUserProfileMessage>(OnReceiveDataResponse);
        StartCoroutine(WaitForConnectionAndSend(userId));

        if (gameCount != 0)
        {
            userRecord.text = gameCount + " 게임, 승률: " + winCount * 100 / gameCount + "%";
        }
        else
        {
            userRecord.text = gameCount + " 게임, 승률: - %";
        }
    }

    private IEnumerator WaitForConnectionAndSend(string userId)
    {
        // Wait until the client is connected
        while (!NetworkClient.isConnected)
        {
            yield return null; // Wait for the next frame
        }

        SendDataRequest(userId);
    }

    private void SendDataRequest(string userId)
    {
        RequestUserProfileMessage requestData = new RequestUserProfileMessage { userId = userId };
        NetworkClient.Send(requestData);
    }

    private void OnReceiveDataResponse(ResponseUserProfileMessage msg)
    {
        // Handle the received data, e.g., update the UI
        string nickname = msg.nickname;
        int playCount = msg.playCount;
        int winCount = msg.winCount;
        string profilePicturePath = msg.profilePicturePath;

        // Implement UI updates or other logic here
        userName.text = nickname;
        userRecord.text = playCount + " 게임, 승률: " + winCount * 100 / playCount + "%";
    }
}
