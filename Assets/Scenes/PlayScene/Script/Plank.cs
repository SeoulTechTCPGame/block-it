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

    public Vector2Int GetPlankCoord()
    {
        return mCoordinate;
    }

    public EDirection GetPlankDirection()
    {
        return direction;
    }

}
