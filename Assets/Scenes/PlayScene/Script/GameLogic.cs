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
        Pawn targetPawn = getTargetPawn(ePlayer);        
        Vector2Int targetCoord = targetPawn.GetCoordinate();

        return targetCoord;
    }

    public List<Vector2Int> GetMoveablePawnCoords(EPlayer ePlayer)//(Pawn pawn)
    {
        Pawn targetPawn = getTargetPawn(ePlayer);
        Pawn opponentPawn = GetOpponentPawn(targetPawn);

        List<Vector2Int> validCoords = new List<Vector2Int>();

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
        if (targetCol - 1 >= 0 && opponentRow == targetRow  && targetCol - 1 == opponentCol)
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

        return validCoords;
    }
    
    // !! FILL THIS METHOD !!
    public List<Vector2Int> GetPlaceablePlankCoords(EDirection direction)
    {
        List<Vector2Int> target = new List<Vector2Int>();
        //fill the code
        bool flag;
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                flag = true;
                foreach (Plank plank in planks)
                {
                    if (plank.GetDirection() == direction && plank.GetCoordinate().x == i && plank.GetCoordinate().y == j)
                    {
                        flag = false;
                        break;
                    }
                }
                if (flag)
                {
                    target.Add(new Vector2Int(i, j));
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
        Pawn targetPawn = getTargetPawn(ePlayer);
        targetPawn.SetCoordinate(coordinate); 
    }

    public void SetPlank(Plank plank)
    {
        BFS bfs = new BFS();
        if (bfs.IsThereAtLeastOneWay(P1) && bfs.IsThereAtLeastOneWay(P2))
        {
            // TODO 좌표가 동일한 plank 는 추가해서는 안됨
            planks.Add(plank);
        }
    }

    public bool Wins(EPlayer ePlayer)//Pawn pawn)
    {
        Pawn targetPawn = getTargetPawn(ePlayer);

        if (targetPawn.GetPlankNum() == 1 && targetPawn.GetCoordinate().y == 9)
        {
            return true;
        }
        else if (targetPawn.GetPlankNum() == 2 && targetPawn.GetCoordinate().y == 1)
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

    public Pawn getTargetPawn(EPlayer ePlayer)
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
 
}
