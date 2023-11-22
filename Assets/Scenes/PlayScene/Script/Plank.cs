using UnityEngine;
using static Enums;

public class Plank // Plank Ŭ����
{
    private Vector2Int _coordinate;
    private EDirection _direction;

    public void SetPlank(Vector2Int coord, EDirection eDirection) // �����ǿ� Plank ����1
    {
        _coordinate = coord;
        _direction = eDirection;
    }
    public void SetPlank(Vector2Int coord) // �����ǿ� Plank ����2
    {
        _coordinate = coord;
    }
    public void SetPlank(EDirection eDirection) // �����ǿ� Plank ����3
    {
        _direction = eDirection;
    }

    public Vector2Int GetCoordinate() // Plank�� ��ǥ ����
    {
        return _coordinate;
    }

    public EDirection GetDirection() // Plank�� ���� �ִ� ����(����/����) ����
    {
        return _direction;
    }

    public void SetDirection(EDirection direction) // Plank�� ���� (����/����) ����
    {
        _direction = direction;
    }

    public void SetCoordinate(Vector2Int vector2Int) // Plank�� ��ǥ ���� 
    {
        _coordinate = vector2Int;
    }

    public void SetAll(EDirection direction, Vector2Int vector2Int) // Plank�� ����� ��ǥ�� �� ���� ����
    {
        _direction = direction;
        _coordinate = vector2Int;
    }
}