using UnityEngine;

public class Pawn // Pawn Ŭ���� 
{
    private Vector2Int _coordinate;
    private int _plankNum = 10;
    private int _pawnNum;

    public void SetCoordinate(Vector2Int coordinate) // Pawn�� ��ǥ ����
    {
        _coordinate = coordinate;
    }

    public int GetPawnNum() // ���� ���� Pawn�� ��ȣ ����
    {
        return _pawnNum;
    }

    public Vector2Int GetCoordinate() // Pawn�� ��ǥ ����
    {
        return _coordinate;
    }

    public int GetPlankNum() // �ش� Pawn�� ������ �ִ� Plank�� ���� ����
    {
        return _plankNum;
    }

    public void UsePlank() // Plank ���
    {
        _plankNum--;
    }

    public bool IsPlankValid() // ��� ������ Plank�� �ִ��� Ȯ��
    {
        if (_plankNum <= 0) return false;
        else return true;
    }
}