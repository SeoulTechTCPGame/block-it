using UnityEngine;
using System.Collections.Generic;

enum EPawns { Pawn1, Pawn2 }


public class GameLogic
{
    private Pawn P1;
    private Pawn P2;

    Plank[] planks = new Plank[20];

    void SetGame() {
        P1.SetCoordinate(new Vector2Int(5, 1));
        P2.SetCoordinate(new Vector2Int(5, 9));
    }

    Vector2Int GetPawnCoordinate(EPawns Pawn) {
        if (Pawn == EPawns.Pawn1)
        {
            return P1.GetCoordinate();
        }
        else
        {
            return P2.GetCoordinate();
        }
    }

    int GetPawnPlankNum(EPawns Pawn) {
        if (Pawn == EPawns.Pawn1)
        {
            return P1.GetPlankNum();
        }
        else
        {
            return P2.GetPlankNum();
        }
    }

    /*
     
     
    List<Vector2Int> GetMoveablePawnCoords(EPawns Pawn) {
        //Encounter.NewEncounterPawn newEncounterPawn = new Encounter.NewEncounterPawn();
        if (Pawn == EPawns.Pawn1)
        {
          //  return newEncounterPawn.ValidCoordinates(P1);
        }
        else
        {
          //  return newEncounterPawn.ValidCoordinates(P2);
        }
        
    }
 */
    //bool IsPlankPlaceable(int PawnNum, Vector2Int coordinate, EDirection direction) {
    //    Encounter.NewEncounterPlank newEncounterPlank = new Encounter.NewEncounterPlank();
    //    if (Pawn == EPawns.Pawn1)
    //    {

    //    } else
    //    {

    //    }
    //}

    void SetPawnPlace(int PawnNum, Vector2Int coordinate) {
        if (PawnNum == 1)
        {
          //  Board.NewBoard.mainBoard[coordinate.y][coordinate.x] = '1';
        } else
        {
            //Board.NewBoard.mainBoard[coordinate.y][coordinate.x] = '2';
        }
    }

    void SetPlank(int PawnNum, Vector2Int coordinate, EDirection direction) {
        
    }

    bool Wins(EPawns Pawn) {
        if (Pawn == EPawns.Pawn1)
        {
            return true;
        }
        else if (Pawn == EPawns.Pawn2)
        {
            return true;
        }

        return false;
    }
}

/*
 *         public List<Vector2Int> ValidCoordinates(Plank plank)
        {
            List<Vector2Int> validCoords = new List<Vector2Int>();

            int rowCoord = plank.GetPlankCoord().y;
            int colCoord = plank.GetPlankCoord().x;
            // Up
            if (IsUpCoordValid(rowCoord, colCoord))
            {
                validCoords.Add(new Vector2Int(rowCoord - 1, colCoord));
            }
            // Down
            if (IsDownCoordValid(rowCoord, colCoord))
            {
                validCoords.Add(new Vector2Int(rowCoord + 1, colCoord));
            }
            // Left
            if (IsLeftCoordValid(rowCoord, colCoord))
            {
                validCoords.Add(new Vector2Int(rowCoord, colCoord - 1));
            }
            // Right
            if (IsRightCoordValid(rowCoord, colCoord))
            {
                validCoords.Add(new Vector2Int(rowCoord, colCoord + 1));
            }

            return validCoords;
        }

        private bool IsUpCoordValid(int rowCoord, int colCoord)
        {
            if (Board.NewBoard.mainBoard[rowCoord - 1][colCoord] == '0')
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool IsDownCoordValid(int rowCoord, int colCoord)
        {
            if (Board.NewBoard.mainBoard[rowCoord + 1][colCoord] == '0')
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool IsLeftCoordValid(int rowCoord, int colCoord)
        {
            if (Board.NewBoard.mainBoard[rowCoord][colCoord - 2] == '0')
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool IsRightCoordValid(int rowCoord, int colCoord)
        {
            if (Board.NewBoard.mainBoard[rowCoord][colCoord + 1] == '0')
            {
                return true;
            }
            else
            {
                return false;
            }
        }*/