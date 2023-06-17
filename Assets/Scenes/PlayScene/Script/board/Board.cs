using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{

    public static char[,] mainBoard = new char[18,18];

    public void Initialize()
    {
        for (int i = 0; i <= 17; i++)
        {
            for (int j = 0; j <= 17; j++)
            {
                if (i == 1 && j == 9) mainBoard[i,j] = '1';
                else if (i == 17 && j == 9) mainBoard[i,j] = '2';
                else if (i % 2 == 0 && j % 2 == 0) mainBoard[i,j] = ' ';
                else if (i % 2 == 0 || j % 2 == 0) mainBoard[i,j] = '*';
                else mainBoard[i,j] = '0';
            }
        }
    }

    int rowPlank, colPlank;
    String directionPlank;

    private void PlankAlreadyInPlace()
    {
        Debug.Log("해당 좌표에 이미 나무판자가 있습니다. 좌표와 방향을 다시 입력하세요.");
    }

    public bool PutPlankOnBoard(bool trueIfVertical)
    {
        Plank plank = new Plank();
        Board board = new Board();
        Encounter encounter = new Encounter();
        rowPlank = plank.GetRowPlank();
        colPlank = plank.GetColPlank();
        String directionPlank = plank.GetPlankDirection();

        bool check = true;

        check = encounter.CheckPlank(rowPlank, colPlank);
        if (check)
        {
            PlankAlreadyInPlace();
            return true;
        }
        try
        {
            if (trueIfVertical)
            {
                // vertical
                if (directionPlank.Equals("w"))
                {
                    // 나무 판자가 해당 좌표로부터 위로 놓임.
                    board.PutPlankUp(rowPlank, colPlank);
                }
                else if (directionPlank.Equals("x"))
                {
                    // 나무 판자가 해당 좌표로부터 아래로 놓임.
                    board.PutPlankDown(rowPlank, colPlank);
                }
                mainBoard[rowPlank,colPlank] = '|';

            }
            else
            {
                // horizontal
                if (directionPlank.Equals("a"))
                {
                    // 나무 판자가 해당 좌표로부터 왼쪽으로 놓임.
                    board.PutPlankLeft(rowPlank, colPlank);
                }
                else if (directionPlank.Equals("d"))
                {
                    // 나무 판자가 해당 좌표로부터 오른쪽으로 놓임.
                    board.PutPlankRight(rowPlank, colPlank);
                }
                mainBoard[rowPlank,colPlank] = 'ㅡ';

            }

        }
        catch (ArgumentException e)
        {
            Debug.Log("해당 좌표에 이미 장애물이 있습니다.");
            return true;
        }

        return false;
    }

    private void PutPlankUp(int rowPlank, int colPlank)
    {
        Encounter encounter = new Encounter();
        bool check = encounter.CheckPlank(rowPlank - 1, colPlank);
        check = check || encounter.CheckPlank(rowPlank - 2, colPlank);
        if (check)
        {
            PlankAlreadyInPlace();
            throw new Exception();
        }
        mainBoard[rowPlank - 2,colPlank] = '|';
        mainBoard[rowPlank - 1,colPlank] = '|';
        mainBoard[rowPlank,colPlank] = '|';
    }

    private void PutPlankDown(int rowPlank, int colPlank)
    {
        Encounter encounter = new Encounter();
        bool check = encounter.CheckPlank(rowPlank + 1, colPlank);
        check = check || encounter.CheckPlank(rowPlank + 2, colPlank);
        if (check)
        {
            PlankAlreadyInPlace();
            throw new Exception();
        }
        mainBoard[rowPlank + 2,colPlank] = '|';
        mainBoard[rowPlank + 1,colPlank] = '|';
    }

    private void PutPlankLeft(int rowPlank, int colPlank)
    {
        Encounter encounter = new Encounter();
        bool check = encounter.CheckPlank(rowPlank, colPlank - 1);
        check = check || encounter.CheckPlank(rowPlank, colPlank - 2);
        if (check)
        {
            PlankAlreadyInPlace();
            throw new Exception();
        }
        mainBoard[rowPlank,colPlank - 2] = 'ㅡ';
        mainBoard[rowPlank,colPlank - 1] = 'ㅡ';
    }

    private void PutPlankRight(int rowPlank, int colPlank)
    {
        Encounter encounter = new Encounter();
        bool check = encounter.CheckPlank(rowPlank, colPlank + 1);
        check = check || encounter.CheckPlank(rowPlank, colPlank + 2);
        if (check)
        {
            PlankAlreadyInPlace();
            throw new Exception();
        }
        mainBoard[rowPlank,colPlank + 2] = 'ㅡ';
        mainBoard[rowPlank,colPlank + 1] = 'ㅡ';
    }

    public void SetPos(Player player)
    {
        int row = player.GetRowPos();
        int col = player.GetColPos();
        Debug.Log("row: " + row + "col: " + col);
        int a = player.GetPlayerNum();
        if (a == 1)
        {
            Debug.Log("playerNum: " + '1');
            mainBoard[row,col] = '1';
        }
        else
        {
            Debug.Log("playerNum: " + '2');
            mainBoard[row,col] = '2';
        }
    }

    public static void PrintMainBoard()
    {
        for (int i = 0; i <= 17; i++)
        {
            for (int j = 0; j <= 17; j++)
            {
                if (i == 0 && j % 2 == 0) Debug.Log((char)((j + 1) / 2 + '0') + " ");
                else if (j == 0 && i % 2 == 0) Debug.Log((char)((i + 1) / 2 + '0') + " ");
                else if (i == 0 && j % 2 == 1) Debug.Log((char)((j - 1) / 2 + 'a') + " " );
                else if (j == 0 && i % 2 == 1) Debug.Log((char)((i - 1) / 2 + 'a') + " " );
                else
                {
                    if (i % 2 == 0 && j % 2 == 0) Debug.Log("  ");
                    else if (mainBoard[i,j] == '1')
                        Debug.Log("1 " );
                    else if (mainBoard[i,j] == '2')
                        Debug.Log("2 " );
                    else if (mainBoard[i,j] == '*')
                        Debug.Log( "* " );
                    else if (mainBoard[i,j] == 'ㅡ')
                        Debug.Log("ㅡ" );
                    else if (mainBoard[i,j] == '|')
                        Debug.Log("| ");
                    else Debug.Log(mainBoard[i,j] + " ");
                }
            }
            Debug.Log("\n");
        }
    }

    public void CheckBoundary(int pos)
    {
        if (pos < 1 || pos > 17)
        {
            Debug.Log("경계를 넘습니다. 다시 입력하세요.");
            throw new Exception();
        }
    }

    

    
}
