using UnityEngine;
using UnityEngine.UI;

public class Emotes : MonoBehaviour
{
    [SerializeField]
    private GameObject _emotePanel;

    [SerializeField]
    private GameObject _emotePopUpObject;

    [SerializeField]
    private Sprite _goodEmoteImg;
    [SerializeField]
    private Sprite _badEmoteImg;
    [SerializeField]
    private Sprite _hurryEmoteImg;

    private bool _isEmotes = false;
    private float _timer;


    private void Update()
    {
        if(_isEmotes)
        {
            if(_timer <= 0)
            {
                _isEmotes = false;
                _emotePopUpObject.SetActive(false);
            }
            else
            {
                _timer -= Time.deltaTime;    
            }
        }
    }

    public void ToggleObject()
    {
        if (_emotePanel.activeSelf)
        {
            _emotePanel.SetActive(false);
        }
        else
        {
            _emotePanel.SetActive(true);
        }
    }
    public void GoodEmotesClicked()
    {
        PopUpEmote(Enums.EEmotes.Good);
    }
    public void BadEmotesClicked()
    {
        PopUpEmote(Enums.EEmotes.Bad);
    }
    public void HurryEmotesClicked()
    {
        PopUpEmote(Enums.EEmotes.Hurry);
    }
    private void PopUpEmote(Enums.EEmotes eEmotes)
    {
        Sprite targetImage;

        targetImage = _goodEmoteImg;
        switch(eEmotes)
        {
        case Enums.EEmotes.Good:
                targetImage = _goodEmoteImg;
            break;
        case Enums.EEmotes.Bad:
                targetImage = _badEmoteImg;
            break;
        case Enums.EEmotes.Hurry:
                targetImage = _hurryEmoteImg;
            break;        
        }

        _emotePopUpObject.SetActive(true);
        _emotePopUpObject.GetComponent<Image>().sprite = targetImage;
        _isEmotes = true;
        _timer = 10f;
    }

}