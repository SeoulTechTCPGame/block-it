using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;



public class Cell : MonoBehaviour
{
    private Vector2Int Coordinate = new Vector2Int();

    [SerializeField] GameObject _rightPlank;
    [SerializeField] GameObject _bottomPlank;
    [SerializeField] GameObject _bottomRightPlank;
    [SerializeField] GameObject _box;
    [SerializeField] GameObject _pawn;

    private bool bRightEdge;
    private bool bBottomEdge;

    private Image _rightPlankImage;
    private Image _bottomPlankImage;
    private Image _pawnImage;
    public Button _pawnButton;
    private Dictionary<string, Image> _bottomRightDictionary;

    // Start is called before the first frame update

    private void Awake()
    {
        _bottomRightDictionary = new Dictionary<string, Image>();
        _rightPlankImage = _rightPlank.GetComponentInChildren<Image>();
        _bottomPlankImage = _bottomPlank.GetComponentInChildren<Image>();
        _pawnImage = _pawn.GetComponentInChildren<Image>();
        _pawnButton = _pawn.GetComponent<Button>();

        _pawnButton.onClick.AddListener(() => ButtonClicked());

        initBottomRightPlanks();
        offEdge();
    }

    public void SetEdge(bool rightEdge, bool bottomEdge)
    {
        bRightEdge = rightEdge;
        bBottomEdge = bottomEdge;
    }
    public void SetCoordinate(int col, int row)
    {
        Coordinate.x = col;
        Coordinate.y = row;
    }
    public void SetRightPlank(bool visible, Color color)
    {
//        _rightPlank.gameObject.SetActive(visible);
        _rightPlankImage.enabled = visible;
        _rightPlankImage.color = color;
    }
    public void SetBottomPlank(bool visible, Color color)
    {
//        _bottomPlank.gameObject.SetActive(visible);
        _bottomPlankImage.enabled = visible;
        _bottomPlankImage.color = color;
    }
    public void SetBottomRightPlank(string key, bool visible, Color color) 
    {
        Image target = _bottomRightDictionary[key];
        target.enabled = visible;
        target.color = color;

        offOtherImages(key);
    }

    public void SetPawn(bool visible, Color color)
    {
        _pawnImage.enabled = visible;
        _pawnImage.color = color;

        _pawnButton.enabled = false;
        _pawnButton.interactable = false;
    }
    public void RemovePawn()
    {
        _pawnImage.color = Color.white;
        _pawnButton.enabled = true;
        _pawnButton.interactable = false;

    }
    public void SetClickablePawn(bool bClickable)
    {
        _pawnButton.enabled = true;
        _pawnButton.interactable = bClickable;
    }

    private void initBottomRightPlanks()
    {
        initBottomRightDictionary();

        Transform parentTransform = _bottomRightPlank.transform;
        List<string> dictionaryKeys = new List<string>(_bottomRightDictionary.Keys);

        foreach(string name in dictionaryKeys)
        {
            Transform targetTransform = parentTransform.Find(name);
            if(targetTransform != null)
            {
                _bottomRightDictionary[name] = targetTransform.GetComponent<Image>();
            }
        }

    }
    private void initBottomRightDictionary()
    {
        /*
        _bottomRightDictionary.Add("Top", null);
        _bottomRightDictionary.Add("Left", null);
        _bottomRightDictionary.Add("Bottom", null);
        _bottomRightDictionary.Add("Right", null);
         */
        _bottomRightDictionary.Add("Horizontal", null);
        _bottomRightDictionary.Add("Vertical", null);
    }
    private void offOtherImages(string key)
    {
        List<string> keysToTurnOff = new List<string>();

        foreach (string name in _bottomRightDictionary.Keys)
        {
            if (name != key)
            {
                keysToTurnOff.Add(name);
            }
        }

        foreach (string name in keysToTurnOff)
        {
            _bottomRightDictionary[name].enabled = false;
        }
    }
    private void offEdge()
    {
        if (bRightEdge)
        {
            _rightPlank.gameObject.SetActive(false);
            _bottomRightPlank.gameObject.SetActive(false);
        }
        if (bBottomEdge)
        {
            _bottomPlank.gameObject.SetActive(false);
            _bottomRightPlank.gameObject.SetActive(false);
        }
    }

    private void ButtonClicked()
    {
        MatchManager.setRequestedPawnCoord.Invoke(Coordinate);
    }
}
