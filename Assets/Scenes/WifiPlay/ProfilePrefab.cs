using UnityEngine;
using UnityEngine.UI;

// 탐색된 플레이어를 보여주고 경기 여부 판단 클래스
public class ProfilePrefab : MonoBehaviour
{
    private GameObject _btn;    // 탐색된 플레이어를 보여줄 버튼 프리팹
    private Image _panel;   // 수락 거절 패널

    private void Start()
    {
        _btn.GetComponent<Button>().onClick.AddListener(TransferInformation);
    }
    private void TransferInformation()
    {
        // _panel 키고 정보 전달하기
    }
}