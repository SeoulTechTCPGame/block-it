using System;
using UnityEngine;


public class Pawn
{
    private Vector2Int _coordinate;
    private int _plankNum = 10;
    private int _pawnNum;

    public void SetCoordinate(Vector2Int coordinate)
    {
        _coordinate = coordinate;
    }

    public int GetPawnNum()
    {
        return _pawnNum;
    }

    public Vector2Int GetCoordinate()
    {
        return _coordinate;
    }

    public int GetPlankNum()
    {
        return _plankNum;
    }



    public void UsePlank()
    {
        _plankNum--;
    }

    public bool IsPlankValid()
    {
        if (_plankNum <= 0) return false;
        else return true;
    }
}