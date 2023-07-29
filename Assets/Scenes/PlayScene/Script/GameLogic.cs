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

    int coorY = targetPawn.GetCoordinate().y;
    int coorX = targetPawn.GetCoordinate().x;

    int opponentRow = opponentPawn.GetCoordinate().y;
    int opponentCol = opponentPawn.GetCoordinate().x;

    // Plank, Pawn 둘 모두에 대한 검사를 하여야 함.

    // NORTH
    if(IsPlankInTheNorth(coorX, coorY) || coorY <= 0)// plank 가 있는지 검사
    {
            if (coorY - 2 >= 0 && !IsPlankInTheNorth(coorX, coorY - 1))
            {
                validCoords.Add(new Vector2Int(coorX, coorY - 2));
            }
        }
    else if (opponentRow + 1 == coorY && coorX == opponentCol) // pawn 이 있는지 검사 
    {
        Debug.Log("other Pawn is on North");

        if (coorY - 2 >= 0 && !IsPlankInTheNorth(coorX, coorY-1))
        {
            validCoords.Add(new Vector2Int(coorX, coorY - 1));
        }
    }
    else
    {
        validCoords.Add(new Vector2Int(coorX, coorY - 1));
    }

    // SOUTH
    if (coorY + 1 < 9 && opponentRow == coorY + 1 && coorX == opponentCol)
    {
        if (coorY + 2 < 9 && !IsPlankInTheSouth(coorX, coorY + 1))
        {
            validCoords.Add(new Vector2Int(coorX, coorY + 2));
        }
    }
    else if (coorY + 1 < 9 && !IsPlankInTheSouth(coorX, coorY + 1))
    {
        validCoords.Add(new Vector2Int(coorX, coorY + 1));
    }

    // EAST
    if (coorX + 1 < 9 && opponentRow == coorY && coorX + 1 == opponentCol)
    {
        if (coorX + 2 < 9 && !IsPlankInTheEast(coorX + 1, coorY))
        {
            validCoords.Add(new Vector2Int(coorX + 2, coorY));
        }
    }
    else if (coorX + 1 < 9 && !IsPlankInTheEast(coorX + 1, coorY))
    {
        validCoords.Add(new Vector2Int(coorX + 1, coorY));
    }

    // WEST
    if (coorX - 1 >= 0 && opponentRow == coorY && coorX - 1 == opponentCol)
    {
        if (coorX - 2 >= 0 && !IsPlankInTheWest(coorX - 1, coorY))
        {
            validCoords.Add(new Vector2Int(coorX - 2, coorY));
        }
    }
    else if (coorX - 1 >= 0 && !IsPlankInTheWest(coorX - 1, coorY))
    {
        validCoords.Add(new Vector2Int(coorX - 1, coorY));
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
        
public bool IsPlankInTheNorth(int coorX, int coorY)
{
    foreach (Plank plank in planks)
    {
        if ( plank.GetDirection() == EDirection.Horizontal && (plank.GetCoordinate().y == coorY - 1 ) && ( (plank.GetCoordinate().x == coorX) || (plank.GetCoordinate().x == coorX - 1) ) )
        {
                Debug.Log("There is plank in the North");
            return true;
        }
    }

    return false;
}

public bool IsPlankInTheSouth(int coorX, int coorY)
{
        foreach (Plank plank in planks)
        {
            if (plank.GetDirection() == EDirection.Horizontal && (plank.GetCoordinate().y == coorY) && ((plank.GetCoordinate().x == coorX) || (plank.GetCoordinate().x == coorX - 1)))
        {
                Debug.Log("There is plank in the South");
                return true;
        }
    }
    return false;
}

public bool IsPlankInTheEast(int coorX, int coorY)
{
    foreach (Plank plank in planks)
    {
        if (plank.GetDirection() == EDirection.Vertical && (plank.GetCoordinate().x == coorX) && ((plank.GetCoordinate().y == coorY) || (plank.GetCoordinate().y == coorY - 1)))
        {
                Debug.Log("There is plank in the East");
                return true;
        }
    }
    return false;
}

public bool IsPlankInTheWest(int coorX, int coorY)
{
    foreach (Plank plank in planks)
    {
        if (plank.GetDirection() == EDirection.Vertical && (plank.GetCoordinate().x == coorX - 1) && ((plank.GetCoordinate().y == coorY) || (plank.GetCoordinate().y == coorY - 1) ) )
        {
                Debug.Log("There is plank in the West");
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
        Debug.Log("Set Plank");

        planks.Add(plank);

        bool player1 = IsThereAtLeastOneWay(EPlayer.Player1);
        bool player2 = IsThereAtLeastOneWay(EPlayer.Player2);
        //Debug.Log("player1: " + player1);
        //Debug.Log("player2: " + player2);

    if (!player1 || !player2)
    {
            Debug.Log("Removed");
            // TODO 좌표가 동일한 plank 는 추가해서는 안됨
            planks.RemoveAt(planks.Count - 1);
    }

    foreach(Plank _plank in planks)
        {
            Debug.Log(_plank.GetCoordinate());
        }

        Debug.Log("Size: " + planks.Count + "--------");
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

private bool IsThereAtLeastOneWay(EPlayer player)
{
    Pawn pawn;
    if (player == EPlayer.Player1) pawn = P1;
    else pawn = P2;

    int coorY = pawn.GetCoordinate().y;
    int coorX = pawn.GetCoordinate().x;

    Queue<Vector2Int> que = new Queue<Vector2Int>();

    que.Enqueue(new Vector2Int(coorX, coorY));

    int[,] visited = new int[9, 9];
    visited[coorY, coorX] = 1;
        //// NSEW
        while (que.Count != 0)
    {
        coorY = que.Peek().y;
        coorX = que.Peek().x;
        visited[coorY, coorX] = 1;
            
        if ((player == EPlayer.Player1 && coorY == 0) || (player == EPlayer.Player2 && coorY == 8))
        {
                //Debug.Log("------visited------");
                //for (int i = 0; i < 9; i++)
                //{
                //    hey = "";
                //    for (int j = 0; j < 9; j++)
                //    {
                //        hey += visited[i, j].ToString();

                //    }
                //    Debug.Log(hey);
                //}
                //Debug.Log("-------------");

                Debug.Log("Success");
                return true;
        }
        // NORTH
        if (!IsPlankInTheNorth(coorX, coorY))
        {
                if (coorY - 1 >= 0 && visited[coorY - 1, coorX] != 1)
                {
                    visited[coorY - 1, coorX] = 1;
                    Debug.Log("NORTH  y: " + (coorY - 1) + "x: " + coorX);
                    que.Enqueue(new Vector2Int(coorX, coorY - 1));
                }
        }
        // SOUTH
        if (!IsPlankInTheSouth(coorX, coorY))
        {
                if (coorY + 1 < 9 && visited[coorY + 1, coorX] != 1)
                {
                    visited[coorY + 1, coorX] = 1;
                    Debug.Log("SOUTH  y: " + (coorY + 1) + "x: " + coorX);
                    que.Enqueue(new Vector2Int(coorX, coorY + 1));
                }
        }
        // WEST
        if (!IsPlankInTheWest(coorX, coorY))
        {
                if (coorX - 1 >= 0 && visited[coorY, coorX - 1] != 1)
                {
                    visited[coorY, coorX - 1] = 1;
                    Debug.Log("WEST  y: " + coorY + "x: " + (coorX - 1));
                    que.Enqueue(new Vector2Int(coorX - 1, coorY));
                }
        }
        // EAST
        if (!IsPlankInTheEast(coorX, coorY))
        {
                if (coorX + 1 < 9 && visited[coorY, coorX + 1] != 1)
                {
                    visited[coorY, coorX + 1] = 1;
                    Debug.Log("EAST  y: " + coorY + "x: " + (coorX + 1));
                    que.Enqueue(new Vector2Int(coorX + 1, coorY));
                }
        }

        

        que.Dequeue();
    }
        //Debug.Log("Y: " + coorY + "X: " + coorX);
        
        //Debug.Log("------visited------");
        //for (int i = 0; i < 9; i++)
        //{
        //    hey = "";
        //    for (int j = 0; j < 9; j++)
        //    {
        //        hey += visited[i, j].ToString();

        //    }
        //    Debug.Log(hey);
        //}
        //Debug.Log("-------------");


        Debug.Log("Failed");
    return false;
}

}
