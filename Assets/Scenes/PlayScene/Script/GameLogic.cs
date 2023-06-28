using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GameLogic
{
    private Pawn P1;
    private Pawn P2;

    public List<Plank> planks = new List<Plank>();

    private void SetGame()
    {
        P1.SetCoordinate(new Vector2Int(5, 1));
        P2.SetCoordinate(new Vector2Int(5, 9));
    }

    Vector2Int GetPawnCoordinate(Pawn pawn)
    {  
        return pawn.GetCoordinate();  
    }

    List<Vector2Int> GetMoveablePawnCoords(Pawn pawn)
    {
        List<Vector2Int> validCoords = new List<Vector2Int>();

        int rowCoord = pawn.GetCoordinate().y;
        int colCoord = pawn.GetCoordinate().x;

        bool[] NSEW = WhichCoordsAreValid(rowCoord, colCoord);

        // NORTH
        if (NSEW[0])
        {
            validCoords.Add(new Vector2Int(colCoord, rowCoord - 1));
        }
        // SOUTH
        if (NSEW[1])
        {
            validCoords.Add(new Vector2Int(colCoord, rowCoord + 1));
        }
        // EAST
        if (NSEW[2])
        {
            validCoords.Add(new Vector2Int(colCoord + 1, rowCoord));
        }
        // WEST
        if (NSEW[3])
        {
            validCoords.Add(new Vector2Int(colCoord - 1, rowCoord));
        }

        return validCoords;
    }

    bool IsPlankPlaceable(Vector2Int coor)
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

    void SetPawnPlace(Pawn pawn, Vector2Int coordinate)
    {
        pawn.SetCoordinate(coordinate); 
    }

    void SetPlank(Plank plank)
    {
        BFS bfs = new BFS();
        if (bfs.IsThereAtLeastOneWay(P1) && bfs.IsThereAtLeastOneWay(P2))
        {
            // TODO 좌표가 동일한 plank 는 추가해서는 안됨
            planks.Add(plank);
        }
    }

    bool Wins(Pawn pawn)
    {
        if (pawn.GetPlankNum() == 1 && pawn.GetCoordinate().y == 9)
        {
            return true;
        }
        else if (pawn.GetPlankNum() == 2 && pawn.GetCoordinate().y == 1)
        {
            return true;
        }

        return false;
    }

    private bool IsOutOfBoundary(int row, int col)
    {
        if (row <= 0 || row > 9 || col <= 0 || col > 9)
        {
            return true;
        }
        return false;
    }


    private bool[] WhichCoordsAreValid(int row, int col)
    {
        bool[] NSEW = { true, true, true, true };

        foreach (Plank plank in planks)
        {
            // NORTH 
            if (IsNorthNotValid(plank, row, col))
            {
                NSEW[0] = false;
            }

            // EAST
            if (IsEastNotValid(plank, row, col))
            {
                NSEW[2] = false;
            }

            // SOUTH
            if (IsSouthNotValid(plank, row, col))
            {
                NSEW[1] = false;
            }

            // WEST
            if (IsWestNotValid(plank, row, col))
            {
                NSEW[3] = false;
            }
        }

        return NSEW;
    }

    public bool IsNorthNotValid(Plank plank, int row, int col)
    {
        Vector2Int plankCoord = plank.GetCoordinate();
        EDirection plankDirection = plank.GetDirection();

        return plankDirection == EDirection.Horizontal && plankCoord.y == row - 1 && plankCoord.x == col - 1 || plankCoord.y == row - 1 && plankCoord.x == col;
    }

    public bool IsEastNotValid(Plank plank, int row, int col)
    {
        Vector2Int plankCoord = plank.GetCoordinate();
        EDirection plankDirection = plank.GetDirection();

        return plankDirection == EDirection.Vertical && plankCoord.y == row - 1 && plankCoord.x == col || plankCoord.y == row && plankCoord.x == col;
    }

    public bool IsSouthNotValid(Plank plank, int row, int col)
    {
        Vector2Int plankCoord = plank.GetCoordinate();
        EDirection plankDirection = plank.GetDirection();

        return plankDirection == EDirection.Horizontal && plankCoord.y == row && plankCoord.x == col - 1 || plankCoord.y == row && plankCoord.x == col;
    }

    public bool IsWestNotValid(Plank plank, int row, int col)
    {
        Vector2Int plankCoord = plank.GetCoordinate();
        EDirection plankDirection = plank.GetDirection();

        return plankDirection == EDirection.Vertical && plankCoord.y == row - 1 && plankCoord.x == col - 1 || plankCoord.y == row && plankCoord.x == col - 1;
    }
}
