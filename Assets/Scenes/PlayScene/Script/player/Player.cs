using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private int playerNum;
    private int rowPos;
    private int colPos;
    private int numberOfPartitions = 10;

    Board board = new Board();
    Move move = new Move();
    Plank plank = new Plank();

   

    public bool Wins()
    {
        if (playerNum == 1 && rowPos == 17)
        {
            Debug.Log("************************");
            Debug.Log("**  Player 1 WINS!!!  **");
            Debug.Log("************************");
            return true;
        }
        else if (playerNum == 2 && rowPos == 1)
        {
            Debug.Log("************************");
            Debug.Log("**  Player 2 WINS!!!  **");
            Debug.Log("************************");

            return true;
        }
        return false;
    }

    public void Turn(Player p1, Player p2)
    {
        Player p = new Player();
        bool check = true;

        PrintPlayerInfo();

        while (check)
        {
            Debug.Log("Player" + playerNum + ", 말을 움직이려면 1, 판을 놓으려면 0을 입력하세요.");
            int action = InputAction();

            if (action == 1)
            { // 말을 움직이려 할 때 
                if (p.MovePlayer(this)) check = true;
                else check = false;
            }
            else
            { // 판을 놓으려 할 때
                if (this.numberOfPartitions == 0)
                {
                    ThereIsNoPartition();
                    continue;
                }
                check = plank.Put(p1, p2);
                if (check) continue;
                this.numberOfPartitions--;  // 나무 판자 개수 하나 줄어듦.
                check = false;

                BFS.PrintVisited();
            }
        }
    }

    private bool MovePlayer(Player player)
    {
        Board.mainBoard[player.rowPos, player.colPos] = '0';
        if (move.SelectDirection(player))
        {
            if (playerNum == 1) Board.mainBoard[player.rowPos, player.colPos] = '1';
            else if (playerNum == 2) Board.mainBoard[player.rowPos, player.colPos] = '2';
            return true; // move.selecDirection(player) 이 true 면 입력을 다시 받아야 함. 
        }

        board.SetPos(player);
        return false;
    }

    private void PrintPlayerInfo()
    {
        Debug.Log("Player" + playerNum + " 의 차례");
        Debug.Log("현재 위치는 (" + rowPos + ", " + colPos + ") 이며");
        Debug.Log("남은 나무 판자의 개수는 " + numberOfPartitions + " 입니다. ");
    }

    private void ThereIsNoPartition()
    {
        Debug.Log("남은 나무 판자가 없습니다. 플레이어 " + playerNum + "의 남은 나무 판자의 개수: " + numberOfPartitions);
    }



    private int InputAction()
    {
        int action;
        string input;
        while (true)
        {
            try
            {
                input = Console.ReadLine();
                action = Int32.Parse(input);
                if (action != 0 && action != 1) throw new Exception();
                break;
            }
            catch (Exception e)
            {
                Debug.Log("오직 1이나 0만으로 다시 입력하세요: ");
                Console.ReadLine();
            }
        }
        return action;
    }

    public void SetPlayerNum(int playerNum)
    {
        this.playerNum = playerNum;
    }

    public void SetRowPos(int rowPos)
    {
        this.rowPos = rowPos;
    }

    public void SetColPos(int colPos)
    {
        this.colPos = colPos;
    }

    public int GetPlayerNum()
    {
        return this.playerNum;
    }

    public int GetRowPos()
    {
        return this.rowPos;
    }
    public int GetColPos()
    {
        return this.colPos;
    }
    
}
