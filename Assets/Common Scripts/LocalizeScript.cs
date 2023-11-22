using UnityEngine;
using TMPro;
using static LanguageVersion;

// 필요한 번역을 입력하여 번역을 가져오는 클래스
public class LocalizeScript : MonoBehaviour
{
    public string TextKey;  // 번역할 내용(영어로)

    private void Start()
    {
        LocalizeChanged(V.CurLangIndex);
        V.LocalizeChanged += LocalizeChanged;
    }

    private void OnDestroy()
    {
        V.LocalizeChanged -= LocalizeChanged;
    }

    // 번역
    private string Localize(string key, int langIndex)
    {
        int keyIndex = V.Langs[0].Value.FindIndex(x => x.ToLower() == key.ToLower());
        return V.Langs[langIndex].Value[keyIndex];
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