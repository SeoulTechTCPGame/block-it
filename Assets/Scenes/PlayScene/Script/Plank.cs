using System;
using UnityEngine;

public enum EDirection
{
    Vertical,
    Horizontal
}

public class Plank
{
    private Vector2Int _coordinate;
    private EDirection _direction;

    public void SetPlank(Vector2Int coord, EDirection eDirection)
    {
        _coordinate = coord;
        _direction = eDirection;
    }
    public void SetPlank(Vector2Int coord)
    {
        _coordinate = coord;
    }
    public void SetPlank(EDirection eDirection)
    {
        _direction = eDirection;
    }

    public Vector2Int GetCoordinate()
    {
        return _coordinate;
    }

    public EDirection GetDirection()
    {
        return _direction;
    }

    public void SetDirection(EDirection direction)
    {
        _direction = direction;
    }

    public void SetCoordinate(Vector2Int vector2Int)
    {
        _coordinate = vector2Int;
    }

    public void SetAll(EDirection direction, Vector2Int vector2Int)
    {
        _direction = direction;
        _coordinate = vector2Int;
    }

}
