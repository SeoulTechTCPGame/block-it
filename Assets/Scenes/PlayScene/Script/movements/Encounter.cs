using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Encounter : MonoBehaviour
{
    Board board = new Board();

    public bool CheckPlayer(Player player, int row, int col)
    {

        char cmpPlayer = Board.mainBoard[row, col];
        if (cmpPlayer == '1' || cmpPlayer == '2')
        {
            board.CheckBoundary(row);
            board.CheckBoundary(col);
            return true; // true 면 있다. 
        }
        return false;  // false 면 없다.
    }

    public bool CheckPlank(int row, int col)
    {
        Move move = new Move();
        Plank plank = new Plank();
        string direction = Move.direction != null ? Move.direction : plank.GetPlankDirection();
        char boardVal = Board.mainBoard[row, col];

        if (boardVal == 'ㅡ' || boardVal == '|')
        {
            if (direction.Equals("w")) Debug.Log("위에 장애물이 있어 이동할 수 없습니다. 다시 입력하세요: ");
            else if (direction.Equals("a")) Debug.Log("왼쪽에 장애물이 있어 이동할 수 없습니다. 다시 입력하세요: ");
            else if (direction.Equals("d")) Debug.Log("오른쪽에 장애물이 있어 이동할 수 없습니다. 다시 입력하세요: ");
            else if (direction.Equals("x")) Debug.Log("아래쪽에 장애물이 있어 이동할 수 없습니다. 다시 입력하세요: ");
            return true;  // true 면 나무 판자가 이동 방향에 있다. 
        }
        return false;
    }

    public bool PlayerOnTheUpDownSide(Player player, int currRow, int currCol)
    {
        // 만약에 왼쪽 오른쪽 모두에 나무 판자가 있다면 입력을 다시 받아야 함. 
        bool check = true;

        while (check)
        {
            String leftOrRight = Console.ReadLine();
            if (leftOrRight.Equals("a"))
            {
                if (Board.mainBoard[currRow, currCol - 1] == '|')
                { // 왼쪽으로 가려는데 나무 판자가 있을 때. 
                    Debug.Log("해당 방향에 나무 판자가 있습니다. 다시 입력하세요.");
                }
                else
                {
                    player.SetRowPos(currRow);
                    player.SetColPos(currCol - 2);
                    return true;
                }
            }
            else if (leftOrRight.Equals("d"))
            {
                if (Board.mainBoard[currRow, currCol + 1] == '|')
                { // 오른쪽으로 가려는데 나무 판자가 있을 때. 
                    Debug.Log("해당 방향에 나무 판자가 있습니다. 다시 입력하세요.");
                }
                else
                {
                    player.SetRowPos(currRow);
                    player.SetColPos(currCol + 2);
                    return true;
                }
            }
            else
            {
                Debug.Log("다시 입력하세요. 왼쪽: a, 오른쪽: d");
            }
        }
        return false;
    }

    public bool PlayerOnTheLeftRightSide(Player player, int currRow, int currCol)
    {
        bool check = true;

        while (check)
        {
            String UpOrDown = Console.ReadLine();
            if (UpOrDown.Equals("w"))
            {
                if (Board.mainBoard[currRow - 1, currCol] == 'ㅡ')
                { // 왼쪽으로 가려는데 나무 판자가 있을 때. 
                    Debug.Log("해당 방향에 나무 판자가 있습니다. 다시 입력하세요.");
                }
                else
                {
                    player.SetRowPos(currRow - 2);
                    player.SetColPos(currCol);
                    return true;
                }
            }
            else if (UpOrDown.Equals("d"))
            {
                if (Board.mainBoard[currRow + 1, currCol] == 'ㅡ')
                { // 오른쪽으로 가려는데 나무 판자가 있을 때. 
                    Debug.Log("해당 방향에 나무 판자가 있습니다. 다시 입력하세요.");
                }
                else
                {
                    player.SetRowPos(currRow + 2);
                    player.SetColPos(currCol);
                    return true;
                }
            }
            else
            {
                Debug.Log("다시 입력하세요. 위쪽: w, 아래쪽: x");
            }
        }
        return false;
    }


    public bool WoodInBothSideLeftRight(int currRow, int currCol)
    {
        if (Board.mainBoard[currRow, currCol - 1] == '|' && Board.mainBoard[currRow, currCol + 1] == '|') return true;
        return false;
    }

    public bool WoodInBothSideUpDown(int currRow, int currCol)
    {
        if (Board.mainBoard[currRow, currCol - 1] == 'ㅡ' && Board.mainBoard[currRow, currCol + 1] == 'ㅡ') return true;
        return false;
    }
    
}
