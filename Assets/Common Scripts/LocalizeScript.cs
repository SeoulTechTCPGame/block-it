using UnityEngine;
using TMPro;
using static Singleton;

public class LocalizeScript : MonoBehaviour
{
    public string TextKey;

    private void Start()
    {
        LocalizeChanged(S.curLangIndex);
        S.LocalizeChanged += LocalizeChanged;
    }
    private void OnDestroy()
    {
        S.LocalizeChanged -= LocalizeChanged;
    }
    private string Localize(string key, int langIndex)
    {
        int keyIndex = S.Langs[0].value.FindIndex(x => x.ToLower() == key.ToLower());
        return S.Langs[langIndex].value[keyIndex];
    }
    public void LocalizeChanged(int langIndex)
    {
        if (GetComponent<TMP_Text>() != null)
        {
            GetComponent<TMP_Text>().text = Localize(TextKey, langIndex);
        }
    }
}