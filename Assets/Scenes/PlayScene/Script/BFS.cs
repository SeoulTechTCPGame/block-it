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

        GameLogic gameLogic = new GameLogic();
        int[,] visited = new int[9, 9];

        //// NSEW
        while (que.Count != 0)
        {
            row = que.Peek().y;
            col = que.Peek().x;
            visited[row,col] = 1;
            if (pawnNum == 1 && row == 9 || pawnNum == 2 && row == 1) return true;
            // NORTH
            if (visited[row+1,col] != 1 && !gameLogic.IsPlankInTheNorth(row, col))
            { 
                que.Enqueue(new Vector2Int(col, row+1));
            }
            // SOUTH
            if (visited[row-1, col] != 1 && !gameLogic.IsPlankInTheSouth(row, col))
            {
                que.Enqueue(new Vector2Int(col, row-1));
            }
            // EAST
            if (visited[row, col+1] != 1 && !gameLogic.IsPlankInTheEast(row, col))
            {
                que.Enqueue(new Vector2Int(col+1, row));
            }
            // WEST
            if (visited[row, col-1] != 1 && !gameLogic.IsPlankInTheWest(row, col))
            {
                que.Enqueue(new Vector2Int(col-1, row));
            }

            que.Dequeue();
        }

        return false;
    }
 
}