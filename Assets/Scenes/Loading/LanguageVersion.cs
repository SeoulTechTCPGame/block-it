using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// 가져온 언어를 저장하는 클래스
[System.Serializable]
public class Language
{
    public string Lang, LangLocalize;   // 현재 언어 버전, 현재 번역
    public List<string> Value = new List<string>(); // 모든 번역들
}
// 언어 버전, 언어 가져오는 클래스
public class LanguageVersion : MonoBehaviour
{
    const string LANG_URL = "https://docs.google.com/spreadsheets/d/1Dsj19n_rK5MEaxfu_4dn2NMJHAj3pcd4A-eY-7MjJ2M/export?format=tsv"; // 번역 구글 스프라이트 시트와 연결
    public event System.Action<int> LocalizeChanged = (index) => { };   // 현재 번역 바꾸기
    public event System.Action LocalizeSettingChanged = () => { };  // 언어 버전 바꾸기
    public int CurLangIndex;    // 현재 언어 버전 Enums.ELanguage로 판단
    public List<Language> Langs;    // 저장

    #region 싱글톤
    public static LanguageVersion V;

    private void Awake()
    {
        if (null == V)
        {
            V = this;
            DontDestroyOnLoad(this);
        }
        else Destroy(this);
        InitLang();
    }
    #endregion
    // 현재 언어 인덱스 설정
    public void SetLangIndex(int index)
    {
        CurLangIndex = index;
        PlayerPrefs.SetInt("LangIndex", CurLangIndex);
        LocalizeChanged(CurLangIndex);
        LocalizeSettingChanged();
    }

    // 현재 세팅된 언어로 번역 가져오기
    private void InitLang()
    {
        int langIndex = PlayerPrefs.GetInt("LangIndex", -1);
        int systemIndex = Langs.FindIndex(x => x.Lang.ToLower() == Application.systemLanguage.ToString().ToLower());
        if (systemIndex == -1) 
        {
            systemIndex = 0; 
        }
        int index = langIndex == -1 ? systemIndex : langIndex;

        SetLangIndex(index);
    }

    [ContextMenu("언어 가져오기")]
    // 필요한 언어 번역 가져오기
    private void GetLang()
    {
        StartCoroutine(GetLangCo());
    }

    private IEnumerator GetLangCo()
    {
        UnityWebRequest www = UnityWebRequest.Get(LANG_URL);
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
        Langs = new List<Language>();
        for (int i = 0; i < columnSize; i++)
        {
            Language lang = new Language();
            lang.Lang = Sentence[0, i];
            lang.LangLocalize = Sentence[1, i];

            for (int j = 2; j < rowSize; j++) lang.Value.Add(Sentence[j, i]);
            Langs.Add(lang);
        }
    }
}