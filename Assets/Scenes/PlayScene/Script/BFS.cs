using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public static class BFS
{
    public static bool IsThereAtLeastOneWay(Pawn pawn)
    {
        int pawnRow = pawn.GetCoordinate().y;
        int pawnCol = pawn.GetCoordinate().x;
        int pawnNum = pawn.GetPawnNum();
        Queue<Vector2Int> que = new Queue<Vector2Int>();
        que.Enqueue(new Vector2Int(pawnCol, pawnRow));
        
        
        int[,] visited = new int[9, 9];

        //// NSEW
        while (que.Count != 0)
        {
            pawnRow = que.Peek().y;
            pawnCol = que.Peek().x;
            visited[pawnRow,pawnCol] = 1;
            if (pawnNum == 1 && pawnRow == 8 || pawnNum == 2 && pawnRow == 0) return true;
            // NORTH

            if (pawnRow + 1 < 9 && visited[pawnRow+1,pawnCol] != 1 && !GameLogic.instance.IsPlankInTheNorth(pawnRow, pawnCol))
            { 
                que.Enqueue(new Vector2Int(pawnCol, pawnRow+1));
            }
            // SOUTH
            if (pawnRow - 1 >= 0 && visited[pawnRow-1, pawnCol] != 1 && !GameLogic.instance.IsPlankInTheSouth(pawnRow, pawnCol))
            {
                que.Enqueue(new Vector2Int(pawnCol, pawnRow-1));
            }
            // EAST
            if (pawnCol + 1 < 9 && visited[pawnRow, pawnCol+1] != 1 && !GameLogic.instance.IsPlankInTheEast(pawnRow, pawnCol))
            {
                que.Enqueue(new Vector2Int(pawnCol+1, pawnRow));
            }
            // WEST
            if (pawnCol - 1 >= 0 && visited[pawnRow, pawnCol-1] != 1 && !GameLogic.instance.IsPlankInTheWest(pawnRow, pawnCol))
            {
                que.Enqueue(new Vector2Int(pawnCol-1, pawnRow));
            }

            que.Dequeue();
        }

        return false;
    }
 
}