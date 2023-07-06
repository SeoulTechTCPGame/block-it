using UnityEngine;
using UnityEngine.UI;
using static Singleton;

public class LocalizeScript : MonoBehaviour
{
    public string textKey;

    private void Start()
    {
        LocalizeChanged();
        S.LocalizeChanged += LocalizeChanged;
    }
    private void OnDestroy()
    {
        S.LocalizeChanged -= LocalizeChanged;
    }
    private string Localize(string key)
    {
        int keyIndex = S.Langs[0].value.FindIndex(x => x.ToLower() == key.ToLower());
        return S.Langs[S.curLangIndex].value[keyIndex];
    }
    private void LocalizeChanged()
    {
        if (GetComponent<Text>() != null)
        {
            GetComponent<Text>().text = Localize(textKey);
        }
    }
}