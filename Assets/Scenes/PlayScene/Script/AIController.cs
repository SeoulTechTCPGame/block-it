using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using static Enums;
using System;

public class AIController : MonoBehaviour
{
    private MatchManager matchManager;
    private int _level;
    private GameLogic _gameLogic;
    private BoardManager boardManager;

    public static int num = 0;

    public void Initialize(MatchManager manager)
    {
        matchManager = manager;
        boardManager = FindObjectOfType<BoardManager>();
        _gameLogic = FindAnyObjectByType<GameLogic>();
        _level = PlayerPrefs.GetInt("AILevel", 1);
        StartCoroutine(AITurn());
    }


    IEnumerator AITurn()
    {
        while (true)
        {
            // Check if it's the AI's turn
            if (!matchManager.getIsAITurn())
            {
                yield return null; // Wait for the next frame
                continue; // Skip the rest of the loop
            }

            Debug.Log("AITurn coroutine started.");

            // Perform AI actions here...
            string serverUrl = "http://localhost:8080/choose/next/move";
            GameData gameData = new GameData();
            // Set up your GameData here...
            gameData.level = _level + 1;
            gameData.game = new Game(_gameLogic, matchManager);
            num += 1;

            // Convert GameData to JSON string
            string jsonData = JsonUtility.ToJson(new RootObject { data = gameData });
            Debug.Log(jsonData);

            using (UnityWebRequest www = UnityWebRequest.Post(serverUrl, jsonData, "application/json"))
            {
                www.SetRequestHeader("Content-Type", "application/json");

                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Error sending data to server: " + www.error);
                }
                else
                {
                    Debug.Log("Data sent successfully!");
                    if (matchManager != null)
                    {
                        matchManager.HandleAIResponse(www.downloadHandler.text);
                    }
                }
            }

            // End the AI turn after completing actions
            matchManager.EndAITurn();
            Debug.Log("AITurn coroutine ended.");

            yield return null; // Wait for the next frame before checking again
        }
    }

    public static bool[] Flatten(bool[][] input)
    {
        int height = input.Length;
        int width = input[0].Length;
        bool[] flattened = new bool[width * height];

        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < width; i++)
            {
                flattened[j * width + i] = input[j][i];
            }
        }

        return flattened;
    }
}

[System.Serializable]
public class Board
{
    public Pawn_[] pawns;
    public bool[] walls_horizontal;
    public bool[] walls_vertical;

    public Board(MatchManager manager)
    {
        this.pawns = manager.pawns;
        this.walls_horizontal = AIController.Flatten(manager.walls.horizontal);
        this.walls_vertical = AIController.Flatten(manager.walls.vertical);
    }
}

[System.Serializable]
public class Game
{
    public Board board;
    public int winner;
    public int _turn;
    public bool[] _validNextWalls_horizontal;
    public bool[] _validNextWalls_vertical;
    public bool[] _probableNextWalls_horizontal;
    public bool[] _probableNextWalls_vertical;
    public string _probableValidNextWalls;
    public bool _probableValidNextWallsUpdated;
    public bool[] openWays_upDown;
    public bool[] openWays_leftRight;
    public bool[] _validNextPositions;
    public bool _validNextPositionsUpdated = false;


    public Game(GameLogic gameLogic, MatchManager manager)
    {
        this.board = new Board(manager);
        if (gameLogic != null)
        {
            this.winner = (int)gameLogic.Winner;
            this._turn = (int)gameLogic.Turn + 2 * AIController.num;
        }

        this._validNextWalls_horizontal = AIController.Flatten(manager._validNextWalls.horizontal);
        this._validNextWalls_vertical = AIController.Flatten(manager._validNextWalls.vertical);

        this._probableNextWalls_horizontal = AIController.Flatten(manager._probableNextWalls.horizontal);
        this._probableNextWalls_vertical = AIController.Flatten(manager._probableNextWalls.vertical);

        this._probableValidNextWalls = null;
        this._probableValidNextWallsUpdated = false;

        this.openWays_upDown = AIController.Flatten(manager._openWays.upDown);
        this.openWays_leftRight = AIController.Flatten(manager._openWays.leftRight);

        this._validNextPositions = AIController.Flatten((new ValidNextPosition(9,9,false, board.pawns[1]))._validNextPosition);

    }
    // Other properties...
}



[System.Serializable]
public class GameData
{
    public Game game;
    public int level;
}

public class RootObject
{
    public GameData data;
}