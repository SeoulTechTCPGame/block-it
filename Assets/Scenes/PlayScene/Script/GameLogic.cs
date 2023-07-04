using UnityEngine;
using System.Collections.Generic;
using System.Linq;

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
        P1.SetCoordinate(new Vector2Int(4, 6));//8));

        P2.SetCoordinate(new Vector2Int(4, 2));// 0));

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


        List<Vector2Int> validCoords = new List<Vector2Int>();

        int rowCoord = targetPawn.GetCoordinate().y;
        int colCoord = targetPawn.GetCoordinate().x;

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

    private Pawn getTargetPawn(EPlayer ePlayer)
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
