using System.Collections.Generic;
using UnityEngine;

class BFS
{
    public bool IsThereAtLeastOneWay(Pawn pawn)
    {
        int row = pawn.GetCoordinate().y;
        int col = pawn.GetCoordinate().x;
        int pawnNum = pawn.GetPawnNum();
        Queue<Vector2Int> que = new Queue<Vector2Int>();
        que.Enqueue(new Vector2Int(col, row));

        //// NSEW
        while (que.Count != 0)
        {
            row = que.Peek().y;
            col = que.Peek().x;
            if (pawnNum == 1 && row == 9 || pawnNum == 2 && row == 1) return true;
            // NORTH
            if (IsNorthValid(row, col))
            { 
                que.Enqueue(new Vector2Int(col, row+1));
            }
            // SOUTH
            if (IsSouthValid(row, col))
            {
                que.Enqueue(new Vector2Int(col, row-1));
            }
            // EAST
            if (IsEastValid(row, col))
            {
                que.Enqueue(new Vector2Int(col+1, row));
            }
            // WEST
            if (IsWestValid(row, col))
            {
                que.Enqueue(new Vector2Int(col-1, row));
            }

            que.Dequeue();
        }

        return false;
    }

    private bool IsNorthValid(int row, int col)
    {
        GameLogic gameLogic = new GameLogic();
        foreach (Plank plank in gameLogic.planks)
        {
            if (gameLogic.IsNorthNotValid(plank, row, col))
            {
                return false;
            }
        }
        return true;
    }

    private bool IsSouthValid(int row, int col)
    {
        GameLogic gameLogic = new GameLogic();
        foreach (Plank plank in gameLogic.planks)
        {
            if (gameLogic.IsSouthNotValid(plank, row, col))
            {
                return false;
            }
        }
        return true;
    }

    private bool IsEastValid(int row, int col)
    {
        GameLogic gameLogic = new GameLogic();
        foreach (Plank plank in gameLogic.planks)
        {
            if (gameLogic.IsEastNotValid(plank, row, col))
            {
                return false;
            }
        }
        return true;
    }

    private bool IsWestValid(int row, int col)
    {
        GameLogic gameLogic = new GameLogic();
        foreach (Plank plank in gameLogic.planks)
        {
            if (gameLogic.IsWestNotValid(plank, row, col))
            {
                return false;
            }
        }
        return true;
    }
}