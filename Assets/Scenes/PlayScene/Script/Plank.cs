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
    private EDirection direction;

    public void SetPlank(Vector2Int coord, EDirection eDirection)
    {
        mCoordinate = coord;
        direction = eDirection;
    }
    public void SetPlank(Vector2Int coord)
    {
        mCoordinate = coord;
    }
    public void SetPlank(EDirection eDirection)
    {
        direction = eDirection;
    }

    public Vector2Int GetCoordinate()
    {
        return mCoordinate;
    }

    public EDirection GetDirection()
    {
        return direction;
    }

}
