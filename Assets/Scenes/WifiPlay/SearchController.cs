using UnityEngine;
using UnityEngine.UI;
using Mirror;

// 대전 상대를 Wifi로 찾는 클래스
public class SearchController : MonoBehaviour
{
    [SerializeField] Image Panel;   // 수락 거절 패널
    [SerializeField] Button Back;   // 뒤로 가기 버튼
    [SerializeField] Button Accept; // 수락 버튼
    [SerializeField] Button Refusal;    // 거절 버튼
}