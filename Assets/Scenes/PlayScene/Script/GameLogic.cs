using UnityEngine;
using System.Collections.Generic;

public enum EPlayer
{
    Player1,
    Player2
}

public class GameLogic : MonoBehaviour
{
private Pawn P1 = new Pawn();   // P1 은 Player1 
private Pawn P2 = new Pawn();   // P2 는 Player2

public EPlayer turn;

public List<Plank> planks = new List<Plank>();  // 플랭크 (나무판자) 인스턴스들을 담은 리스트

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
    P1.SetCoordinate(new Vector2Int(4, 7));  // P1 의 보드판에서의 처음 좌표는 2차원 평면 상에서의 (4, 7) 이

    P2.SetCoordinate(new Vector2Int(4, 1));  // P2 의 보드판에서의 처음 좌표는 2차원 평면 상에서의 (4, 1) 이다 

    turn = EPlayer.Player1;  // 게임이 시작되었을 때 처음 턴은 P1 이 가져간
}

public Vector2Int GetPawnCoordinate(EPlayer ePlayer)  // ePlayer 에 대한 Pawn 의 좌표를 리턴한다
    {
    Pawn targetPawn = GetTargetPawn(ePlayer);    
    Vector2Int targetCoord = targetPawn.GetCoordinate();  

    return targetCoord;
}

public int GetRemainPlank(EPlayer ePlayer)  // 해당 Player 에게 남은 플랭크의 수를 리턴한ㄷ
{
    Pawn targetPawn = GetTargetPawn(ePlayer);
    return targetPawn.GetPlankNum();
}

public List<Vector2Int> GetMoveablePawnCoords(EPlayer ePlayer) // 상하좌우의 보드판의 경계선이나 플랭크의 위치 등을 고려하여 Player가 움직일 수 있는 좌표들을 리턴한
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

    return validCoords;
}


public List<Vector2Int> GetPlaceablePlankCoords(EDirection direction)  // 플랭크의 방향 (수직인지 수평인지) 을 고려하여 보드판에서 플랭크를 둘 수 있는 좌표들을 리턴한다
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

                    if (direction == EDirection.Vertical)
                    {
                        if (plank.GetDirection() == EDirection.Vertical
                            && col == plankCol && (row == plankRow + 1 || row == plankRow - 1))
                        {
                            flag = false;
                            break;
                        }
                    }

                }

                Plank _plank = new Plank();
                _plank.SetCoordinate(new Vector2Int(col, row));
                _plank.SetDirection(direction);
                planks.Add(_plank);

                if (!IsThereAtLeastOneWay(EPlayer.Player1) || !IsThereAtLeastOneWay(EPlayer.Player2))
                {
                    flag = false;
                }

                planks.RemoveAt(planks.Count - 1);

                if (flag)
                {
                    target.Add(new Vector2Int(col, row));
                }
            }
        }

        return target;
}
        
public bool IsPlankInTheNorth(int coorX, int coorY) // Player 의 위치를 기준으로 북쪽에 플랭크가 있는지를 검사한다
{
    foreach (Plank plank in planks)
    {
        if ( plank.GetDirection() == EDirection.Horizontal && (plank.GetCoordinate().y == coorY - 1 ) && ( (plank.GetCoordinate().x == coorX) || (plank.GetCoordinate().x == coorX - 1) ) )
        {
            return true;
        }
    }

    return false;
}

public bool IsPlankInTheSouth(int coorX, int coorY) // Player 의 위치를 기준으로 남쪽에 플랭크가 있는지를 검사한다
    {
        foreach (Plank plank in planks)
        {
            if (plank.GetDirection() == EDirection.Horizontal && (plank.GetCoordinate().y == coorY) && ((plank.GetCoordinate().x == coorX) || (plank.GetCoordinate().x == coorX - 1)))
        {
                return true;
        }
    }
    return false;
}

public bool IsPlankInTheEast(int coorX, int coorY)  // Player 의 위치를 기준으로 동쪽에 플랭크가 있는지를 검사한다
    {
    foreach (Plank plank in planks)
    {
        if (plank.GetDirection() == EDirection.Vertical && (plank.GetCoordinate().x == coorX) && ((plank.GetCoordinate().y == coorY) || (plank.GetCoordinate().y == coorY - 1)))
        {
                return true;
        }
    }
    return false;
}

public bool IsPlankInTheWest(int coorX, int coorY)  // Player 의 위치를 기준으로 서쪽에 플랭크가 있는지를 검사한다
    {
    foreach (Plank plank in planks)
    {
        if (plank.GetDirection() == EDirection.Vertical && (plank.GetCoordinate().x == coorX - 1) && ((plank.GetCoordinate().y == coorY) || (plank.GetCoordinate().y == coorY - 1) ) )
        {
                return true;
        }
    }
    return false;
}

public Pawn GetOpponentPawn(Pawn targetPawn)  // 상대방의 Pawn 객체를 리턴한다
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

public bool IsPlankPlaceable(Vector2Int coor)  // 해당 좌표에 플랭크를 둘 수 있는지를 검사한 
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

public void SetPawnPlace(EPlayer ePlayer, Vector2Int coordinate) // Player 를 해당 좌표에 위치시킨다
{
    Pawn targetPawn = GetTargetPawn(ePlayer);
    targetPawn.SetCoordinate(coordinate);
}

public void SetPlank(Plank plank)  // 플랭크를 실제로 보드판에 둔다
{
    planks.Add(plank);

    bool player1 = IsThereAtLeastOneWay(EPlayer.Player1);
    bool player2 = IsThereAtLeastOneWay(EPlayer.Player2);

        if (!player1 || !player2)
        {
            planks.RemoveAt(planks.Count - 1);
        }
}

public bool Wins(EPlayer ePlayer)  // Player 가 이겼음을 확인한다
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

public bool IsOutOfBoundary(int row, int col)  // 해당 좌표가 보드판의 경계를 넘었는지를 확인한다
{
    if (row <= 0 || row > 9 || col <= 0 || col > 9)
    {
        return true;
    }
    return false;
}

public void changeTurn() // Player 의 턴을 바꾼다
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

public Pawn GetTargetPawn(EPlayer ePlayer)  // 원하는 Player 의 객체를 리턴한다
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

private bool IsThereAtLeastOneWay(EPlayer player)  // 플랭크가 Player 의 사방을 막고 있는지를 확인한다
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
                Debug.Log("Success");
                return true;
        }
        // NORTH
        if (!IsPlankInTheNorth(coorX, coorY))
        {
                if (coorY - 1 >= 0 && visited[coorY - 1, coorX] != 1)
                {
                    visited[coorY - 1, coorX] = 1;
                    que.Enqueue(new Vector2Int(coorX, coorY - 1));
                }
        }
        // SOUTH
        if (!IsPlankInTheSouth(coorX, coorY))
        {
                if (coorY + 1 < 9 && visited[coorY + 1, coorX] != 1)
                {
                    visited[coorY + 1, coorX] = 1;
                    que.Enqueue(new Vector2Int(coorX, coorY + 1));
                }
        }
        // WEST
        if (!IsPlankInTheWest(coorX, coorY))
        {
                if (coorX - 1 >= 0 && visited[coorY, coorX - 1] != 1)
                {
                    visited[coorY, coorX - 1] = 1;
                    que.Enqueue(new Vector2Int(coorX - 1, coorY));
                }
        }
        // EAST
        if (!IsPlankInTheEast(coorX, coorY))
        {
                if (coorX + 1 < 9 && visited[coorY, coorX + 1] != 1)
                {
                    visited[coorY, coorX + 1] = 1;
                    que.Enqueue(new Vector2Int(coorX + 1, coorY));
                }
        }

        

        que.Dequeue();
    }

    return false;
}

}
