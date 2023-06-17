using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;


public class Plank : MonoBehaviour
{
    // 판은 '-' 로 표현.
    Board board = new Board();
    private int rowPlank;
    private int colPlank;
    private string plankDirection;

    public int GetRowPlank()
    {
        return this.rowPlank;
    }

    public int GetColPlank()
    {
        return this.colPlank; 
    }

    public string GetPlankDirection()
    {
        return this.plankDirection;
    }
  

    public bool Put(Player player1, Player player2)
    {
        bool check = true;
        BFS bfs = new BFS();

        while (check)
        {
            ChangeRowToInt();
            bool trueIfVertical = DecideVerticalOrHorizontal();
            ChangeColToInt(trueIfVertical);
            WoodDirection(trueIfVertical);

            check = board.PutPlankOnBoard(trueIfVertical);
            if (check) continue;
            // check 가 true 면 이미 해당 방향에 장애물이 있다.

            check = bfs.IsThereAtLeastOneWay(player1);

            if (check)
            {  // false 면 가능성 없음.
                Debug.Log("플레이어가 상대 진영에 도달할 수 있는 경로가 최소 한 개 이상이어야 합니다. 다시 입력하세요.");
                Board.mainBoard[rowPlank, colPlank] = '*';
                ResetBoardVal();
                return true;
            }

            check = bfs.IsThereAtLeastOneWay(player2);

            if (check)
            {  // false 면 가능성 없음.
                Debug.Log("플레이어가 상대 진영에 도달할 수 있는 경로가 최소 한 개 이상이어야 합니다. 다시 입력하세요.");
                Board.mainBoard[rowPlank, colPlank] = '*';
                ResetBoardVal();
                return true;
            }
            return false;
        }
        return false;
    }

    private void ResetBoardVal()
    {

        if (plankDirection.Equals("w"))
        {
            Board.mainBoard[rowPlank - 1, colPlank] = ' ';
            Board.mainBoard[rowPlank - 2, colPlank] = '*';
        }
        else if (plankDirection.Equals("d"))
        {
            Board.mainBoard[rowPlank, colPlank + 1] = ' ';
            Board.mainBoard[rowPlank, colPlank + 2] = '*';
        }
        else if (plankDirection.Equals("x"))
        {
            Board.mainBoard[rowPlank + 1, colPlank] = ' ';
            Board.mainBoard[rowPlank + 2, colPlank] = '*';
        }
        else if (plankDirection.Equals("a"))
        {
            Board.mainBoard[rowPlank, colPlank - 1] = ' ';
            Board.mainBoard[rowPlank, colPlank - 2] = '*';
        }

    }

    private void WoodDirection(bool trueIfVertical)
    {
        Debug.Log("해당 좌표로부터 나무 판자가 놓일 방향을 입력하세요. ");
        Console.ReadLine();
        while (true)
        {
            try
            {
                Exception e2 = new Exception();
                if (trueIfVertical)
                {
                    Debug.Log("나무 판자는 세로로 놓이므로 '상 w 하 x' 만 입력할 수 있습니다. ");
                    plankDirection = Console.ReadLine();
                    if (plankDirection.Equals("w"))
                    {
                        board.CheckBoundary(rowPlank - 2);
                    }
                    else if (plankDirection.Equals("x"))
                    {
                        board.CheckBoundary(rowPlank + 2);
                    }
                    else
                    {
                        throw e2;
                    }
                }
                else
                {
                    Debug.Log("나무 판자는 가로로 놓이므로 '좌 a 우 d' 만 입력할 수 있습니다. ");
                    plankDirection = Console.ReadLine();

                    if (plankDirection.Equals("a"))
                    {
                        board.CheckBoundary(colPlank - 2);
                    }
                    else if (plankDirection.Equals("d"))
                    {
                        board.CheckBoundary(colPlank + 2);
                    }
                    else
                    {
                        throw e2;
                    }
                }
                break;
            }
            catch (Exception e)
            {
                Debug.Log("나무 판자가 놓일 방향을 다시 입력하세요. ");
            }
            
        }

    }

    private void ChangeRowToInt()
    {
        while (true)
        {
            Debug.Log("나무 판자를 놓을 좌표의 row 값을 입력하세요: row");
            Debug.Log("1부터 8까지의 숫자나 a부터 i까지의 문자만 입력하세요.");

            string row = Console.ReadLine();

            Regex pattern1 = new Regex("^[1-8]*$");
            string pattern2 = "^[a-i]*$";

            Match match1 = pattern1.Match(row);
            bool regex2 = Regex.IsMatch(row, pattern2);

            if (match1.Success)
            {
                // 12345678
                rowPlank = 2 * int.Parse(row);
            }
            else if (regex2)
            {
                // abcdefghi
                rowPlank = 2 * ((int)row[0] - (int)'a') + 1;
            }
            if (rowPlank >= 1 && rowPlank <= 17) break;
        }
    }


    private bool DecideVerticalOrHorizontal()
    {
        if (rowPlank % 2 == 1)
        {
            // VERTICAL
            return true;
        }
        else
        {
            // HORIZONTAL
            return false;
        }
    }

    private void ChangeColToInt(bool trueIfVertical)
    {
        Plank plank = new Plank();
        if (trueIfVertical)
        {
            // vertical
            plank.PutVertically();
        }
        else
        {
            // horizontal
            plank.PutHorizontally();
        }
    }

    private void PutVertically()
    {
        Regex pattern1 = new Regex("^[1-8]*$");
        Debug.Log("나무 판자는 세로로 놓입니다. col 값을 입력하세요: (1,2,3,4,5,6,7,8)");
        Debug.Log("1부터 8까지의 숫자나 a부터 i까지의 문자만 입력하세요.");

        while (true)
        {
            string col = Console.ReadLine();
            Match match1 = pattern1.Match(col);
            if (match1.Success)
            {
                colPlank = 2 * int.Parse(col);
                if (colPlank >= 1 && colPlank <= 17)
                    return;
            }
            Debug.Log("나무 판자는 세로로 놓입니다. col 값을 다시 입력하세요: (1,2,3,4,5,6,7,8)");
        }
    }


    private void PutHorizontally()
    {
        string pattern2 = "^[a-i]*$";
        Debug.Log("나무 판자는 가로로 놓입니다. col 값을 입력하세요: (a,b,c,d,e,f,g,h,i)");
        Debug.Log("1부터 8까지의 숫자나 a부터 i까지의 문자만 입력하세요.");

        while (true)
        {
            string col = Console.ReadLine();
            bool regex2 = Regex.IsMatch(col, pattern2);
            if (regex2)
            {
                colPlank = 2 * ((int)col[0] - (int)'a') + 1;
                if (colPlank >= 1 && colPlank <= 17)
                    return;
            }
            Debug.Log("나무 판자는 가로로 놓입니다. col 값을 다시 입력하세요: (a,b,c,d,e,f,g,h,i)");
        }
    }





   
}
