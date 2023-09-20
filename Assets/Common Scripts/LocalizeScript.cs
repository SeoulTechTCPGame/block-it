using UnityEngine;
using TMPro;
using static Singleton;

// 필요한 번역을 입력하여 번역을 가져오는 클래스
public class LocalizeScript : MonoBehaviour
{
    public string TextKey;  // 번역할 내용(영어로)

    private void Start()
    {
        LocalizeChanged(S.curLangIndex);
        S.LocalizeChanged += LocalizeChanged;
    }
    private void OnDestroy()
    {
        S.LocalizeChanged -= LocalizeChanged;
    }
    // 번역
    private string Localize(string key, int langIndex)
    {
        int keyIndex = S.Langs[0].value.FindIndex(x => x.ToLower() == key.ToLower());
        return S.Langs[langIndex].value[keyIndex];
    }
    // 텍스트 변환
    public void LocalizeChanged(int langIndex)
    {
        if (GetComponent<TMP_Text>() != null)
        {
            GetComponent<TMP_Text>().text = Localize(TextKey, langIndex);
        }
    }
}