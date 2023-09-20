using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// 가져온 언어를 저장하는 클래스
[System.Serializable]
public class Lang
{
    public string lang, langLocalize;   // 현재 언어 버전, 현재 번역
    public List<string> value = new List<string>(); // 모든 번역들
}
// 언어 버전, 언어 가져오는 클래스
public class Singleton : MonoBehaviour
{
    const string langURL = "https://docs.google.com/spreadsheets/d/1Dsj19n_rK5MEaxfu_4dn2NMJHAj3pcd4A-eY-7MjJ2M/export?format=tsv"; // 번역 구글 스프라이트 시트와 연결
    public event System.Action<int> LocalizeChanged = (index) => { };   // 현재 번역 바꾸기
    public event System.Action LocalizeSettingChanged = () => { };  // 언어 버전 바꾸기
    public int curLangIndex;    // 현재 언어 버전 Enums.ELanguage로 판단
    public List<Lang> Langs;    // 저장

    #region 싱글톤
    public static Singleton S;

    private void Awake()
    {
        if (null == S)
        {
            S = this;
            DontDestroyOnLoad(this);
        }
        else Destroy(this);
        InitLang();
    }
    #endregion
    // 현재 세팅된 언어로 번역 가져오기
    private void InitLang()
    {
        int langIndex = PlayerPrefs.GetInt("LangIndex", -1);
        int systemIndex = Langs.FindIndex(x => x.lang.ToLower() == Application.systemLanguage.ToString().ToLower());
        if (systemIndex == -1) 
        {
            systemIndex = 0; 
        }
        int index = langIndex == -1 ? systemIndex : langIndex;

        SetLangIndex(index);
    }
    public void SetLangIndex(int index)
    {
        curLangIndex = index;
        PlayerPrefs.SetInt("LangIndex", curLangIndex);
        LocalizeChanged(curLangIndex);
        LocalizeSettingChanged();
    }
    [ContextMenu("언어 가져오기")]
    // 필요한 언어 번역 가져오기
    private void GetLang()
    {
        StartCoroutine(GetLangCo());
    }
    private IEnumerator GetLangCo()
    {
        UnityWebRequest www = UnityWebRequest.Get(langURL);
        yield return www.SendWebRequest();
        SetLangList(www.downloadHandler.text);
    }
    // 모든 언어 번역 가져오기
    private void SetLangList(string tsv)
    {
        // 이차원 배열(세로, 가로)
        string[] row = tsv.Split('\n');
        int rowSize = row.Length;
        int columnSize = row[0].Split('\t').Length;
        string[,] Sentence = new string[rowSize, columnSize];

        for (int i = 0; i < rowSize; i++)
        {
            string[] column = row[i].Split('\t');
            for (int j = 0; j < columnSize; j++) Sentence[i, j] = column[j];
        }

        // 클래스 리스트
        Langs = new List<Lang>();
        for (int i = 0; i < columnSize; i++)
        {
            Lang lang = new Lang();
            lang.lang = Sentence[0, i];
            lang.langLocalize = Sentence[1, i];

            for (int j = 2; j < rowSize; j++) lang.value.Add(Sentence[j, i]);
            Langs.Add(lang);
        }
    }
}