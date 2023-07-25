using UnityEngine;
using System.Collections.Generic;

public enum EPlayer
{
    Player1,
    Player2
}

public class GameLogic : MonoBehaviour
{
private Pawn P1 = new Pawn();
private Pawn P2 = new Pawn();

public EPlayer turn;

public List<Plank> planks = new List<Plank>();

public static GameLogic instance;
private void Awake()
{
    instance = this;
}

private void Start()
{
    SetGame();
}

private void SetGame()
{
    P1.SetCoordinate(new Vector2Int(4, 7));

    P2.SetCoordinate(new Vector2Int(4, 1));

    turn = EPlayer.Player1;
}

public Vector2Int GetPawnCoordinate(EPlayer ePlayer)//Pawn pawn)
{
    Pawn targetPawn = GetTargetPawn(ePlayer);
    Vector2Int targetCoord = targetPawn.GetCoordinate();

    return targetCoord;
}

public List<Vector2Int> GetMoveablePawnCoords(EPlayer ePlayer)//(Pawn pawn)
{        

    Pawn targetPawn = GetTargetPawn(ePlayer);
    Pawn opponentPawn = GetOpponentPawn(targetPawn);

    List<Vector2Int> validCoords = new List<Vector2Int>() { };

    int targetRow = targetPawn.GetCoordinate().y;
    int targetCol = targetPawn.GetCoordinate().x;

    int opponentRow = opponentPawn.GetCoordinate().y;
    int opponentCol = opponentPawn.GetCoordinate().x;

    // Plank, Pawn 둘 모두에 대한 검사를 하여야 함.

    // NORTH
    if (targetRow - 1 >= 0 && opponentRow == targetRow - 1 && targetCol == opponentCol) // pawn 이 있는지 검사 
    {
        if (targetRow - 2 >= 0 && !IsPlankInTheNorth(targetCol, targetRow - 1))
        {
            validCoords.Add(new Vector2Int(targetCol, targetRow - 2));
        }
    }
    else if (targetRow - 1 >= 0 && !IsPlankInTheNorth(targetCol, targetRow - 1)) // plank 가 있는지 검사
    {
        validCoords.Add(new Vector2Int(targetCol, targetRow - 1));
    }

    // SOUTH
    if (targetRow + 1 < 9 && opponentRow == targetRow + 1 && targetCol == opponentCol)
    {
        if (targetRow + 2 < 9 && !IsPlankInTheSouth(targetCol, targetRow + 1))
        {
            validCoords.Add(new Vector2Int(targetCol, targetRow + 2));
        }
    }
    else if (targetRow + 1 < 9 && !IsPlankInTheSouth(targetCol, targetRow + 1))
    {
        validCoords.Add(new Vector2Int(targetCol, targetRow + 1));
    }

    // EAST
    if (targetCol + 1 < 9 && opponentRow == targetRow && targetCol + 1 == opponentCol)
    {
        if (targetCol + 2 < 9 && !IsPlankInTheEast(targetCol + 1, targetRow))
        {
            validCoords.Add(new Vector2Int(targetCol + 2, targetRow));
        }
    }
    else if (targetCol + 1 < 9 && !IsPlankInTheEast(targetCol + 1, targetRow))
    {
        validCoords.Add(new Vector2Int(targetCol + 1, targetRow));
    }

    // WEST
    if (targetCol - 1 >= 0 && opponentRow == targetRow && targetCol - 1 == opponentCol)
    {
        if (targetCol - 2 >= 0 && !IsPlankInTheWest(targetCol - 1, targetRow))
        {
            validCoords.Add(new Vector2Int(targetCol - 2, targetRow));
        }
    }
    else if (targetCol - 1 >= 0 && !IsPlankInTheWest(targetCol - 1, targetRow))
    {
        validCoords.Add(new Vector2Int(targetCol - 1, targetRow));
    }

        //foreach (Vector2Int coor in validCoords)
        //{
        //    Debug.Log("Pawn" + ePlayer + "\'s Coord: " + coor);
        //}

        return validCoords;
}

// !! FILL THIS METHOD !!
public List<Vector2Int> GetPlaceablePlankCoords(EDirection direction)
{
        List<Vector2Int> target = new List<Vector2Int>();
        //fill the code
        int plankRow, plankCol;
        bool flag;
        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                flag = true;
                foreach (Plank plank in planks)
                {
                    plankRow = plank.GetCoordinate().y;
                    plankCol = plank.GetCoordinate().x;

                    if (row == plankRow && col == plankCol)
                    {
                        flag = false;
                        break;
                    }
                    
                    if (direction == EDirection.Horizontal)
                    {
                        if (plank.GetDirection() == EDirection.Horizontal
                            && row == plankRow && (col == plankCol + 1 || col == plankCol - 1))
                        {
                            flag = false;
                            break;
                        }
                    }
                }
                if (flag)
                {
                    target.Add(new Vector2Int(col, row));
                }
            }
        }
        return target;
}
        
public bool IsPlankInTheNorth(int targetRow, int targetCol)
{
    foreach (Plank plank in planks)
    {
        if (plank.GetDirection() == EDirection.Horizontal && (plank.GetCoordinate().y == targetRow - 1 && plank.GetCoordinate().x == targetCol - 1)
            || (plank.GetCoordinate().y == targetRow - 1 && plank.GetCoordinate().x == targetCol))
        {
            return true;
        }
    }

    return false;
}

public bool IsPlankInTheSouth(int targetRow, int targetCol)
{
    foreach (Plank plank in planks)
    {
        if (plank.GetDirection() == EDirection.Horizontal && (plank.GetCoordinate().y == targetRow && plank.GetCoordinate().x == targetCol) ||
            plank.GetCoordinate().y == targetRow && plank.GetCoordinate().x == targetCol - 1)
        {
            return true;
        }
    }
    return false;
}

public bool IsPlankInTheEast(int targetRow, int targetCol)
{
    foreach (Plank plank in planks)
    {
        if (plank.GetDirection() == EDirection.Vertical && (plank.GetCoordinate().y == targetRow - 1 && plank.GetCoordinate().x == targetCol) ||
            plank.GetCoordinate().y == targetRow && plank.GetCoordinate().x == targetCol)
        {
            return true;
        }
    }
    return false;
}

public bool IsPlankInTheWest(int targetRow, int targetCol)
{
    foreach (Plank plank in planks)
    {
        if (plank.GetDirection() == EDirection.Vertical && (plank.GetCoordinate().y == targetRow - 1 && plank.GetCoordinate().x == targetCol - 1) ||
            plank.GetCoordinate().y == targetRow && plank.GetCoordinate().x == targetCol - 1)
        {
            return true;
        }
    }
    return false;
}

public Pawn GetOpponentPawn(Pawn targetPawn)
{
    if (targetPawn == P1)
    {
        return P2;
    }
    else
    {
        return P1;
    }
}

public bool IsPlankPlaceable(Vector2Int coor)
{
    foreach (Plank plank in planks)
    {
        if (plank.GetCoordinate() == coor)
        {
            return false;
        }
    }
    return true;
}

public void SetPawnPlace(EPlayer ePlayer, Vector2Int coordinate)
{
    Pawn targetPawn = GetTargetPawn(ePlayer);
    targetPawn.SetCoordinate(coordinate);
}

public void SetPlank(Plank plank)
{

    if (IsThereAtLeastOneWay(EPlayer.Player1) && IsThereAtLeastOneWay(EPlayer.Player2))
    {
        // TODO 좌표가 동일한 plank 는 추가해서는 안됨
        planks.Add(plank);
    }
    else
    {
        Debug.LogWarning("Plank NOT Added: x " + plank.GetCoordinate().x + " , y = " + plank.GetCoordinate().y);
    }

    Debug.Log("---Length: " + planks.Count);

    foreach (Plank _plank in planks)
    {
        Debug.Log("Plank: direction = " + _plank.GetDirection() + " x = " + _plank.GetCoordinate().x + " , y " + _plank.GetCoordinate().y);
    }
    Debug.Log("---");
}

public bool Wins(EPlayer ePlayer)
{
    Pawn targetPawn = GetTargetPawn(ePlayer);
    int targetY;

    if(ePlayer == EPlayer.Player1) 
    {
         targetY = 0;
    }
	else
	{
      targetY = 8;
	}

    if (targetPawn.GetCoordinate().y == targetY)
    {
        return true;
    }
    return false;
}

public bool IsOutOfBoundary(int row, int col)
{
    if (row <= 0 || row > 9 || col <= 0 || col > 9)
    {
        return true;
    }
    return false;
}

public void changeTurn()
{
    if (turn == EPlayer.Player1)
    {
        turn = EPlayer.Player2;
    }
    else
    {
        turn = EPlayer.Player1;
    }
}

public Pawn GetTargetPawn(EPlayer ePlayer)
{
    Pawn targetPawn;
    if (ePlayer == EPlayer.Player1)
    {
        targetPawn = P1;
    }
    else
    {
        targetPawn = P2;
    }

    return targetPawn;
}

public bool IsThereAtLeastOneWay(EPlayer player)
{
    Pawn pawn;
    if (player == EPlayer.Player1) pawn = P1;
    else pawn = P2;

    int pawnRow = pawn.GetCoordinate().y;
    int pawnCol = pawn.GetCoordinate().x;
    Queue<Vector2Int> que = new Queue<Vector2Int>();
    que.Enqueue(new Vector2Int(pawnCol, pawnRow));

    int[,] visited = new int[9, 9];

    //// NSEW
    while (que.Count != 0)
    {
        pawnRow = que.Peek().y;
        pawnCol = que.Peek().x;
        visited[pawnRow, pawnCol] = 1;
        if ( (player == EPlayer.Player1 && pawnRow == 0) || (player == EPlayer.Player2 && pawnRow == 8)) return true;
        // NORTH

        if (pawnRow + 1 < 9 && visited[pawnRow + 1, pawnCol] != 1 && !IsPlankInTheNorth(pawnRow, pawnCol))
        {
            que.Enqueue(new Vector2Int(pawnCol, pawnRow + 1));
        }
        // SOUTH
        if (pawnRow - 1 >= 0 && visited[pawnRow - 1, pawnCol] != 1 && !IsPlankInTheSouth(pawnRow, pawnCol))
        {
            que.Enqueue(new Vector2Int(pawnCol, pawnRow - 1));
        }
        // EAST
        if (pawnCol + 1 < 9 && visited[pawnRow, pawnCol + 1] != 1 && !IsPlankInTheEast(pawnRow, pawnCol))
        {
            que.Enqueue(new Vector2Int(pawnCol + 1, pawnRow));
        }
        // WEST
        if (pawnCol - 1 >= 0 && visited[pawnRow, pawnCol - 1] != 1 && !IsPlankInTheWest(pawnRow, pawnCol))
        {
            que.Enqueue(new Vector2Int(pawnCol - 1, pawnRow));
        }

        que.Dequeue();
    }

    return false;
}

}
