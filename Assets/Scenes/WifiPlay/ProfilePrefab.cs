using UnityEngine;
using UnityEngine.UI;

// Ž���� �÷��̾ �����ְ� ��� ���� �Ǵ� Ŭ����
public class ProfilePrefab : MonoBehaviour
{
    private GameObject _btn;    // Ž���� �÷��̾ ������ ��ư ������
    private Image _panel;   // ���� ���� �г�

    private void Start()
    {
        _btn.GetComponent<Button>().onClick.AddListener(TransferInformation);
    }
    private void TransferInformation()
    {
        // _panel Ű�� ���� �����ϱ�
    }
}