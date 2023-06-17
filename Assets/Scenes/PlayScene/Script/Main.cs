using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player player1; // 플레이어 1
    public Player player2; // 플레이어 2
    public Board board; // 게임 보드

    private void Start()
    {
        Player p1 = new Player();
        p1.SetPlayerNum(1);
        p1.SetRowPos(1);
        p1.SetColPos(9);

        Player p2 = new Player();
        p2.SetPlayerNum(2);
        p2.SetRowPos(17);
        p2.SetColPos(9);


        board = GetComponent<Board>(); // Board 클래스를 Unity 게임 오브젝트에 추가해야 합니다.
        board.Initialize();
    }

    private void Update()
    {
        if (!player1.Wins())
        {
            player1.Turn(player1, player2);
            
        }

        if (!player2.Wins())
        {
            player2.Turn(player1, player2);
        }
    }
}
