using UnityEngine;
using System.Collections;
using System;

public class Move
{  // 이 클래스는 사실 Player 와 Wood 둘 다 사용할 수 있어야 함. 

    static public string direction;

    public bool SelectDirection(Player player)
    {  // 코드 정리 한 번 해야함. 
        Move move = new Move();
        Board board = new Board();
        bool check = true;
        int currRow;
        int currCol;
        while (check)
        {  // check가 false 면 해당 방향에 장애물이 없는 것, true 면 장애물이 있는 것. 
            Debug.Log("상 w, 좌 a, 하 x, 우 d");
            direction = Console.ReadLine();
            currRow = player.GetRowPos();
            currCol = player.GetColPos();
            try
            {
                if (direction.Equals("w"))
                {
                    board.CheckBoundary(currRow - 2);
                    check = move.MoveToUp(player, currRow, currCol);
                }
                else if (direction.Equals("a"))
                {
                    board.CheckBoundary(currCol - 2);
                    check = move.MoveToLeft(player, currRow, currCol);
                }
                else if (direction.Equals("x"))
                {
                    board.CheckBoundary(currRow + 2);
                    check = move.MoveToDown(player, currRow, currCol);
                }
                else if (direction.Equals("d"))
                {
                    board.CheckBoundary(currCol + 2);
                    check = move.MoveToRight(player, currRow, currCol);
                }
                else
                {
                    Debug.Log("상 w, 좌 a, 하 x, 우 d 로만 입력해주세요.");
                    continue;
                }
                return check; // true 는 사용자로부터 입력을 다시 받아야 하는 상태. 
            }
            catch (Exception e)
            {
                Debug.Log("보드판의 바운더리를 넘습니다. 다시 입력하세요. ");
            }
        }
        return false;
    }

    

    Encounter encounter = new Encounter();

    private bool MoveToUp(Player player, int currRow, int currCol)
    {
        Board board = new Board();
        bool check = encounter.CheckPlank(currRow - 1, currCol);
        if (check) return true; // 바로 위에 나무 판자가 있음.
        currRow -= 2;
        board.CheckBoundary(currRow);
        check = encounter.CheckPlayer(player, currRow, currCol); // 나무 판자가 없다면 이번에는 해당 방향에 플레이어가 있나 확인. 
        if (check)
        { // 해당 방향에 플레이어 있으면, 건너 뛰어야 함. 
            // 상대 플레이어의 위쪽에 나무 판자가 있으면 왼쪽으로 튀거나 오른쪽으로 튀어야 함. 
            if (currRow - 1 < 1 || Board.mainBoard[currRow - 1, currCol] == 'ㅡ')
            {

                //board.checkBoundary(currRow - 1);
                Debug.Log("상대 플레이어의 위쪽에 나무 판자가 있거나 보드판의 바운더리를 넘습니다.");

                if (encounter.WoodInBothSideLeftRight(currRow, currCol))
                {
                    // 만약 플레이어 양쪽에 나무 판자가 있다면, 처음부터 다시 입력을 받아야 함. 
                    return true;
                }
                Debug.Log("상대를 뛰어 넘어 왼쪽으로 이동하시겠습니까 (a), 오른쪽으로 이동하시겠습니까 (d)");
                check = encounter.PlayerOnTheUpDownSide(player, currRow, currCol);

                // check 가 true 면은 나무 판자를 적절한 위치에 둔 것에 성공.
                if (check)
                {
                    return false;
                }
                return true;

            }
            // 상대 플레이어의 왼쪽이나 오른쪽으로 튀어야 함. 
            currRow -= 2;
        }
        // 해당 방향에 플레이가 없다면, 
        player.SetRowPos(currRow);
        return false;
    }

    private bool MoveToLeft(Player player, int currRow, int currCol)
    {
        Board board = new Board();
        bool check = encounter.CheckPlank(currRow, currCol - 1);
        if (check) return true;
        currCol -= 2;
        board.CheckBoundary(currCol);
        check = encounter.CheckPlayer(player, currRow, currCol);  // 나무 판자가 없다면 이번에는 해당 방향에 플레이어가 있나 확인.
        if (check)
        {  // 해당 방향에 플레이어 있으면, 건너 뛰어야 함. 

            if (currCol - 1 < 1 || Board.mainBoard[currRow, currCol - 1] == '|')
            {
                //board.checkBoundary(currCol - 1);
                Debug.Log("상대 플레이어의 왼쪽에 나무 판자가 있거나 보드판의 바운더리를 넘습니다.");

                if (encounter.WoodInBothSideUpDown(currRow, currCol))
                {
                    return true;
                }

                Debug.Log("상대 플레이어의 왼쪽에 나무 판자가 있습니다. 상대를 뛰어 넘어 위쪽으로 이동하시겠습니까 (w), 아래쪽으로 이동하시겠습니까 (x)");
                check = encounter.PlayerOnTheLeftRightSide(player, currRow, currCol);

                if (check)
                {
                    return false;
                }
                return true;
            }

            currCol -= 2;
        }
        player.SetColPos(currCol);
        return false;
    }

    private bool MoveToDown(Player player, int currRow, int currCol)
    {
        Board board = new Board();
        bool check = encounter.CheckPlank(currRow + 1, currCol);
        if (check) return true;  // 바로 주위에 나무 판자가 있음.
        currRow += 2;
        board.CheckBoundary(currRow);
        check = encounter.CheckPlayer(player, currRow, currCol); // 나무 판자가 없다면 이번에는 해당 방향에 플레이어가 있나 확인.
        if (check)
        {  // 해당 방향에 플레이어 있으면, 건너 뛰어야 함. 

            if (currRow + 1 > 17 || Board.mainBoard[currRow + 1, currCol] == 'ㅡ')
            {

                //board.checkBoundary(currRow + 1);
                Debug.Log("상대 플레이어의 아래쪽에 나무 판자가 있거나 보드판의 바운더리를 넘습니다.");

                if (encounter.WoodInBothSideLeftRight(currRow, currCol))
                {
                    return true;
                }

                Debug.Log("상대를 뛰어 넘어 왼쪽으로 이동하시겠습니까 (a), 오른쪽으로 이동하시겠습니까 (d)");
                check = encounter.PlayerOnTheUpDownSide(player, currRow, currCol);

                if (check)
                {
                    return false;
                }
                return true;
            }

            currRow += 2;
        }

        player.SetRowPos(currRow);
        return false;
    }

    private bool MoveToRight(Player player, int currRow, int currCol)
    {
        Board board = new Board();
        bool check = encounter.CheckPlank(currRow, currCol + 1);
        if (check) return true;  // 바로 주위에 나무 판자가 있음.
        currCol += 2;
        board.CheckBoundary(currCol);
        check = encounter.CheckPlayer(player, currRow, currCol);
        if (check)
        {  // 해당 방향에 플레이어 있으면, 건너 뛰어야 함.

            if (currCol + 1 > 17 || Board.mainBoard[currRow, currCol + 1] == '|')
            {

                //board.checkBoundary(currCol + 1);
                Debug.Log("상대 플레이어의 오른쪽에 나무 판자가 있거나 보드판의 바운더리를 넘습니다.");

                if (encounter.WoodInBothSideLeftRight(currRow, currCol))
                {
                    return true;
                }
                Debug.Log("상대 플레이어의 오른쪽에 나무 판자가 있습니다. 상대를 뛰어 넘어 위쪽으로 이동하시겠습니까 (w), 아래쪽으로 이동하시겠습니까 (x)");
                check = encounter.PlayerOnTheLeftRightSide(player, currRow, currCol);

                if (check)
                {
                    return false;
                }
                return true;
            }

            currCol += 2;
        }
        player.SetColPos(currCol);
        return false;
    }
}