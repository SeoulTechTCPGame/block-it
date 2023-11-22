using UnityEngine;

public class Pawn // Pawn 클래스 
{
    private Vector2Int _coordinate;
    private int _plankNum = 10;
    private int _pawnNum;

    public void SetCoordinate(Vector2Int coordinate) // Pawn의 좌표 설정
    {
        _coordinate = coordinate;
    }

    public int GetPawnNum() // 조작 중인 Pawn의 번호 리턴
    {
        return _pawnNum;
    }

    public Vector2Int GetCoordinate() // Pawn의 좌표 리턴
    {
        return _coordinate;
    }

    public int GetPlankNum() // 해당 Pawn이 가지고 있는 Plank의 개수 리턴
    {
        return _plankNum;
    }

    public void UsePlank() // Plank 사용
    {
        _plankNum--;
    }

    public bool IsPlankValid() // 사용 가능한 Plank가 있는지 확인
    {
        if (_plankNum <= 0) return false;
        else return true;
    }
}