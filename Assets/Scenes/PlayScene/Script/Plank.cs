using System;
using UnityEngine;

public enum EDirection
{
    Vertical,
    Horizontal
}

public class Plank
{
    private Vector2Int mCoordinate;
    private EDirection mDirection;

    public void SetPlank(Vector2Int coord, EDirection eDirection)
    {
        mCoordinate = coord;
        mDirection = eDirection;
    }
    public void SetPlank(Vector2Int coord)
    {
        mCoordinate = coord;
    }
    public void SetPlank(EDirection eDirection)
    {
        mDirection = eDirection;
    }

    public Vector2Int GetCoordinate()
    {
        return mCoordinate;
    }

    public EDirection GetDirection()
    {
        return mDirection;
    }

    public void SetDirection(EDirection direction)
    {
        mDirection = direction;
    }

    public void SetCoordinate(Vector2Int vector2Int)
    {
        mCoordinate = vector2Int;
    }

    public void SetAll(EDirection direction, Vector2Int vector2Int)
    {
        mDirection = direction;
        mCoordinate = vector2Int;
    }

}
