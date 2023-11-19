using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pawn_
{
    public int index;
    public bool isHumanSide;
    public bool isHumanPlayer;
    public Position position;
    public int goalRow;
    public int numberOfLeftWalls;

    public Pawn_(int index, bool isHumanSide, bool isHumanPlayer)
    {
        this.index = index;
        this.isHumanSide = isHumanSide;
        this.isHumanPlayer = isHumanPlayer;
        if (isHumanPlayer)
        {
            position = new Position(8, 4);
            goalRow = 0;
        }
        else
        {
            position = new Position(0, 4);
            goalRow = 8;
        }
        this.numberOfLeftWalls = 10;
    }
}
