using UnityEngine;
using static Enums;

public class Plank // Plank 클래스
{
    private Vector2Int _coordinate;
    private EDirection _direction;

    public void SetPlank(Vector2Int coord, EDirection eDirection) // 보드판에 Plank 놓기1
    {
        _coordinate = coord;
        _direction = eDirection;
    }
    public void SetPlank(Vector2Int coord) // 보드판에 Plank 놓기2
    {
        _coordinate = coord;
    }
    public void SetPlank(EDirection eDirection) // 보드판에 Plank 놓기3
    {
        _direction = eDirection;
    }

    public Vector2Int GetCoordinate() // Plank의 좌표 리턴
    {
        return _coordinate;
    }

    public EDirection GetDirection() // Plank가 놓여 있는 방향(수평/수직) 리턴
    {
        return _direction;
    }

    public void SetDirection(EDirection direction) // Plank의 방향 (수평/수직) 설정
    {
        _direction = direction;
    }

    public void SetCoordinate(Vector2Int vector2Int) // Plank의 좌표 설정 
    {
        _coordinate = vector2Int;
    }

    public void SetAll(EDirection direction, Vector2Int vector2Int) // Plank의 방향과 좌표를 한 번에 설정
    {
        _direction = direction;
        _coordinate = vector2Int;
    }
}