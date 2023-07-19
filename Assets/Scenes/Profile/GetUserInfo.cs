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

public class GetUserInfo : MonoBehaviour
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
        SqlDataReader queryResult = GetInfo();

        while (queryResult.Read())
        {
            userName.text = queryResult[0] as string;
            gameScore = (int)queryResult[1];
            gameCount = (int)queryResult[2];
            winCount = (int)queryResult[3];
        }

        userRecord.text = gameCount + " 게임, 승률: " + winCount * 100 / gameCount + "%";

        queryResult.Close();
    }

    //데이터 조회
    public SqlDataReader GetInfo()
    {
        SqlDataReader reader = null;
        try
        {
            string strConn = GetConnectionString();
            string query = @"SELECT [User].name, [Rank].score, [Rank].totalgamecount, [Rank].wincount 
                             FROM [User] JOIN [Rank] ON [User].id = [Rank].id WHERE [User].id = " + userId + ";";

            SqlConnection conn = new SqlConnection(strConn);
            conn.Open();

            SqlCommand cmd = new SqlCommand(query, conn);
            reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            return reader;
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
            return reader;
        }
    }

    // 연결 정보 가져오기
    private string GetConnectionString()
    {
        string jsonFilepath = @"Assets/Resources/Json/ConnectionInfo.json";
        string strConn = string.Empty;
        try
        {
            string jsonText = System.IO.File.ReadAllText(jsonFilepath);
            JObject jsonData = JObject.Parse(jsonText);

            string dbAddress = (string)jsonData["DB"]["Address"];
            string dbPortNumber = (string)jsonData["DB"]["Port"];
            string dbName = (string)jsonData["DB"]["DBName"];
            string dbId = (string)jsonData["DB"]["ID"];
            string dbPassword = (string)jsonData["DB"]["Password"];

            strConn = "Server=" + dbAddress + "," + dbPortNumber + ";Database=" + dbName + ";Uid=" + dbId + ";Pwd=" + dbPassword + ";";
            Debug.Log(strConn);

            return strConn;
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            return strConn;
        }
    }
}
