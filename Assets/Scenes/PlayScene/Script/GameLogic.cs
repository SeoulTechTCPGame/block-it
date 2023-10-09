using UnityEngine;
using System.Collections.Generic;
using static Enums;

public class GameLogic : MonoBehaviour
{
    public EPlayer Turn;
    public List<Plank> Planks = new List<Plank>();  // 생성된 Plank 인스턴스를 담은 리스트  
    public List<MoveRecord> Moves = new List<MoveRecord>(); // 경기 기록
    public static GameLogic s_instance;

    private Pawn _p1 = new Pawn();  // Player1 인스턴스 생성 
    private Pawn _p2 = new Pawn();  // Player2 인스턴스 생성  

    #region 게임 시작 관리
    private void Awake()
    {
        s_instance = this;
    }

    private void Start()
    {
        SetGame();
    }

    private void SetGame()
    {
        _p1.SetCoordinate(new Vector2Int(4, 8));  // 게임이 시작되었을 때 Player1의 좌표는 2차원 평면상의 (4, 7) 

        _p2.SetCoordinate(new Vector2Int(4, 0));  // 게임이 시작되었을 때 Player2의 좌표는 2차원 평면상의 (4, 1) 

        Turn = EPlayer.Player1; // 게임이 시작되었을 때 첫 번째 턴은 Player1 이 가져간다 
    }
    #endregion

    public void AddMoveRecord()
    {
        Vector2Int p1Coord = _p1.GetCoordinate();
        Vector2Int p2Coord = _p2.GetCoordinate();
        int p1PlankNum = _p1.GetPlankNum();
        int p2PlankNum = _p2.GetPlankNum();

        List<Plank> planks = new List<Plank>(Planks);

        MoveRecord moveRecord = new MoveRecord();
        moveRecord.P1Coordinate = p1Coord;
        moveRecord.P2Coordinate = p2Coord;
        moveRecord.P1PlankNum = p1PlankNum;
        moveRecord.P2PlankNum = p2PlankNum;
        moveRecord.Planks = planks;

        Moves.Add(moveRecord);
    }

    #region Get 메서드 관리
    public Pawn GetTargetPawn(EPlayer ePlayer)  // Player 의 Pawn 인스턴스를 리턴한다  
    {
        Pawn targetPawn;
        if (ePlayer == EPlayer.Player1)
        {
            targetPawn = _p1;
        }
        else
        {
            targetPawn = _p2;
        }

        return targetPawn;
    }

    public Pawn GetOpponentPawn(Pawn targetPawn)  // 상대 Player 의 객체를 리턴한다  
    {
        if (targetPawn == _p1)
        {
            return _p2;
        }
        else
        {
            return _p1;
        }
    }

    public Vector2Int GetPawnCoordinate(EPlayer ePlayer)  // 해당 Player 의 보드판에서의 좌표를 리턴한다 
    {
        Pawn targetPawn = GetTargetPawn(ePlayer);
        Vector2Int targetCoord = targetPawn.GetCoordinate();

        return targetCoord;
    }

    public int GetRemainPlank(EPlayer ePlayer)  // 해당 Player 에게 남은 나무 판자의 개수를 리턴한다  
    {
        Pawn targetPawn = GetTargetPawn(ePlayer);
        return targetPawn.GetPlankNum();
    }

    public List<Vector2Int> GetMoveablePawnCoords(EPlayer ePlayer)  // 보드판의 경계와 Plank 의 유무를 판단하여 해당 Player가 상하좌우 중 갈 수 있는 좌표들을 리턴한다  
    {

        Pawn targetPawn = GetTargetPawn(ePlayer);
        Pawn opponentPawn = GetOpponentPawn(targetPawn);

        List<Vector2Int> validCoords = new List<Vector2Int>() { };

        int coorY = targetPawn.GetCoordinate().y;
        int coorX = targetPawn.GetCoordinate().x;

        int opponentRow = opponentPawn.GetCoordinate().y;
        int opponentCol = opponentPawn.GetCoordinate().x;

        // NORTH
        if (coorY - 1 >= 0 && opponentRow == coorY - 1 && coorX == opponentCol)
        {
            if (coorY - 2 >= 0 && !IsPlankInTheNorth(coorX, coorY - 1))
            {
                validCoords.Add(new Vector2Int(coorX, coorY - 2));
            }
        }
        else if (coorY - 1 >= 0 && !IsPlankInTheNorth(coorX, coorY))
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
        else if (coorY + 1 < 9 && !IsPlankInTheSouth(coorX, coorY))
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
        else if (coorX + 1 < 9 && !IsPlankInTheEast(coorX, coorY))
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
        else if (coorX - 1 >= 0 && !IsPlankInTheWest(coorX, coorY))
        {
            validCoords.Add(new Vector2Int(coorX - 1, coorY));
        }

        return validCoords;
    }



    public List<Vector2Int> GetPlaceablePlankCoords(EDirection Direction)  // 보드판에서 Plank 가 놓일 수 있는 좌표들을 모두 리턴한다  
    {
        List<Vector2Int> target = new List<Vector2Int>();

        int plankRow, plankCol;
        bool flag;
        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                flag = true;

                foreach (Plank plank in Planks)
                {
                    plankRow = plank.GetCoordinate().y;
                    plankCol = plank.GetCoordinate().x;

                    if (row == plankRow && col == plankCol)
                    {
                        flag = false;
                        break;
                    }

                    if (Direction == EDirection.Horizontal)
                    {
                        if (plank.GetDirection() == EDirection.Horizontal
                            && row == plankRow && (col == plankCol + 1 || col == plankCol - 1))
                        {
                            flag = false;
                            break;
                        }
                    }

                    if (Direction == EDirection.Vertical)
                    {
                        if (plank.GetDirection() == EDirection.Vertical
                            && col == plankCol && (row == plankRow + 1 || row == plankRow - 1))
                        {
                            flag = false;
                            break;
                        }
                    }

                }

                Plank newPlank = new Plank();
                newPlank.SetCoordinate(new Vector2Int(col, row));
                newPlank.SetDirection(Direction);
                Planks.Add(newPlank);

                if (!IsThereAtLeastOneWay(EPlayer.Player1) || !IsThereAtLeastOneWay(EPlayer.Player2))
                {
                    flag = false;
                }

                Planks.RemoveAt(Planks.Count - 1);

                if (flag)
                {
                    target.Add(new Vector2Int(col, row));
                }
            }
        }

        return target;
    }
    #endregion

    #region Plank 유무 관리
    public bool IsPlankInTheNorth(int coorX, int coorY)  // 인자로 받은 좌표를 기준으로 북쪽에 Plank 가 있는지를 판단하여 bool 값을 리턴한다  
    {
        foreach (Plank plank in Planks)
        {
            if (plank.GetDirection() == EDirection.Horizontal && (plank.GetCoordinate().y == coorY - 1) && ((plank.GetCoordinate().x == coorX) || (plank.GetCoordinate().x == coorX - 1)))
            {
                return true;
            }
        }

        return false;
    }

    public bool IsPlankInTheSouth(int coorX, int coorY)  // 인자로 받은 좌표를 기준으로 남쪽에 Plank 가 있는지를 판단하여 bool 값을 리턴한다  
    {
        foreach (Plank plank in Planks)
        {
            if (plank.GetDirection() == EDirection.Horizontal && (plank.GetCoordinate().y == coorY) && ((plank.GetCoordinate().x == coorX) || (plank.GetCoordinate().x == coorX - 1)))
            {
                return true;
            }
        }
        return false;
    }

    public bool IsPlankInTheEast(int coorX, int coorY)  // 인자로 받은 좌표를 기준으로 동쪽에 Plank 가 있는지를 판단하여 bool 값을 리턴한다  
    {
        foreach (Plank plank in Planks)
        {
            if (plank.GetDirection() == EDirection.Vertical && (plank.GetCoordinate().x == coorX) && ((plank.GetCoordinate().y == coorY) || (plank.GetCoordinate().y == coorY - 1)))
            {
                return true;
            }
        }
        return false;
    }

    public bool IsPlankInTheWest(int coorX, int coorY)   // 인자로 받은 좌표를 기준으로 서쪽에 Plank 가 있는지를 판단하여 bool 값을 리턴한다   
    {
        foreach (Plank plank in Planks)
        {
            if (plank.GetDirection() == EDirection.Vertical && (plank.GetCoordinate().x == coorX - 1) && ((plank.GetCoordinate().y == coorY) || (plank.GetCoordinate().y == coorY - 1)))
            {
                return true;
            }
        }
        return false;
    }

    public bool IsPlankPlaceable(Vector2Int coor)  // 해당 좌표에 Plank 를 둘 수 있는지를 판단하여 bool 값을 리턴한다  
    {
        foreach (Plank plank in Planks)
        {
            if (plank.GetCoordinate() == coor)
            {
                return false;
            }
        }
        return true;
    }
    #endregion

    #region Set 메서드 관리
    public void SetPawnPlace(EPlayer ePlayer, Vector2Int coordinate)  // Player 를 의도한 좌표로 이동시킨다  
    {
        Pawn targetPawn = GetTargetPawn(ePlayer);
        targetPawn.SetCoordinate(coordinate);
    }

    public void SetPlank(Plank plank)  // Plank 를 의도한 좌표에 둔
    {
        Planks.Add(plank);

        bool player1 = IsThereAtLeastOneWay(EPlayer.Player1);
        bool player2 = IsThereAtLeastOneWay(EPlayer.Player2);

        if (!player1 || !player2)
        {
            Planks.RemoveAt(Planks.Count - 1);
        }

    }
    #endregion

    #region 게임 승패 및 턴 관리
    public bool Wins(EPlayer ePlayer)  // 해당 Player 가 이겼는지를 판단한다
    {
        Pawn targetPawn = GetTargetPawn(ePlayer);
        int targetY;

        if (ePlayer == EPlayer.Player1)
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

    public bool IsOutOfBoundary(int row, int col)  // 해당 좌표가 보드판의 경계를 넘는지를 판단한다  
    {
        if (row <= 0 || row > 9 || col <= 0 || col > 9)
        {
            return true;
        }
        return false;
    }

    public void changeTurn()  // 턴을 넘긴다
    {
        if (Turn == EPlayer.Player1)
        {
            Turn = EPlayer.Player2;
        }
        else
        {
            Turn = EPlayer.Player1;
        }
    }
    #endregion

    private bool IsThereAtLeastOneWay(EPlayer player)  // Player 의 사방이 Plank 로 막히는지를 판단한다  
    {
        Pawn pawn;
        if (player == EPlayer.Player1) pawn = _p1;
        else pawn = _p2;

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

public struct MoveRecord
{
    public Vector2Int P1Coordinate;
    public int P1PlankNum;

    public Vector2Int P2Coordinate;
    public int P2PlankNum;

    public List<Plank> Planks;
}