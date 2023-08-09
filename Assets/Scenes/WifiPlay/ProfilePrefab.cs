using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfilePrefab : MonoBehaviour
{
    private Button _btn;
    private Image _panel;

    private void Start()
    {
        _btn.onClick.AddListener(TransferInformation);
    }
    private void TransferInformation()
    {
        // _panel 키고 정보 전달하기
    }
}