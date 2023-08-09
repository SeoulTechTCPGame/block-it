using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;



public class Cell : MonoBehaviour
{
    private Vector2Int _coordinate = new Vector2Int();

    [SerializeField] GameObject _rightPlank;
    [SerializeField] GameObject _bottomPlank;
    [SerializeField] GameObject _bottomRightPlank;
    [SerializeField] GameObject _plankDot;
    [SerializeField] GameObject _box;
    [SerializeField] GameObject _pawn;

    private bool _isRightEdge;
    private bool _isBottomEdge;
    private Image _rightPlankImage;
    private Image _bottomPlankImage;
    private Image _pawnImage;
    private Image _plankDotImage;
    private Button _pawnButton;
    private Button _plankDotButton;
    private Dictionary<string, Image> _bottomRightDictionary;

    private void Awake()
    {
        _bottomRightDictionary = new Dictionary<string, Image>();
        _rightPlankImage = _rightPlank.GetComponentInChildren<Image>();
        _bottomPlankImage = _bottomPlank.GetComponentInChildren<Image>();
        _pawnImage = _pawn.GetComponentInChildren<Image>();
        _plankDotImage = _plankDot.GetComponent<Image>();
        _pawnButton = _pawn.GetComponent<Button>();
        _plankDotButton = _plankDot.GetComponent<Button>();


        _pawnButton.onClick.AddListener(() => ButtonClicked());
        _plankDotButton.onClick.AddListener(() => PlankDotClicked());

        InitBottomRightPlanks();
        OffEdge();
    }

    public void SetEdge(bool rightEdge, bool bottomEdge)
    {
        _isRightEdge = rightEdge;
        _isBottomEdge = bottomEdge;
    }
    public void SetCoordinate(int col, int row)
    {
        _coordinate.x = col;
        _coordinate.y = row;
    }
    public void ClearCell()
    {
        RemovePawn();
        SetPlankDot(false, Color.white);
        SetRightPlank(false, Color.white);
        SetBottomPlank(false, Color.white);
        SetBottomRightPlank("Horizontal", false, Color.white);
    }
   
    public void SetRightPlank(bool visible, Color color)
    {
        _rightPlankImage.enabled = visible;
        _rightPlankImage.color = color;
    }
    public void SetBottomPlank(bool visible, Color color)
    {
        _bottomPlankImage.enabled = visible;
        _bottomPlankImage.color = color;
    }
    public void SetPlankDot(bool visible)
    {
        _plankDot.gameObject.SetActive(visible);
    }
    public void SetBottomRightPlank(string key, bool visible, Color color) 
    {
        Image target = _bottomRightDictionary[key];
        target.enabled = visible;
        target.color = color;

        OffOtherImages(key);
    }

    public void SetPawn(bool visible, Color color)
    {
        _pawnImage.enabled = visible;
        _pawnImage.color = color;

        _pawnButton.interactable = false;
        _pawnButton.enabled = false;
    }
    public void RemovePawn()
    {
        _pawnImage.color = Color.white;
        _pawnButton.enabled = false;
        _pawnButton.interactable = false;
        _pawnImage.enabled = false;

    }
    public void SetClickablePawn(bool bClickable, Color color)
    {
        _pawnImage.enabled = bClickable;
        _pawnButton.enabled = bClickable;
        _pawnButton.interactable = bClickable;
        if(bClickable== true)
        {
            _pawnImage.color = color;
        }
    }
    public void SetPlankDot(bool visible, Color color)
    {
        _plankDotButton.enabled = visible;
        _plankDotImage.enabled = visible;
        if(visible == true)
        {
            _plankDotImage.color = color;
        }
    }
    private void InitBottomRightPlanks()
    {
        InitBottomRightDictionary();

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
    private void InitBottomRightDictionary()
    {
        _bottomRightDictionary.Add("Horizontal", null);
        _bottomRightDictionary.Add("Vertical", null);
    }
    private void OffOtherImages(string key)
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
    private void OffEdge()
    {
        if (_isRightEdge)
        {
            _rightPlank.gameObject.SetActive(false);
            _bottomRightPlank.gameObject.SetActive(false);
        }
        if (_isBottomEdge)
        {
            _bottomPlank.gameObject.SetActive(false);
            _bottomRightPlank.gameObject.SetActive(false);
        }
    }

    private void ButtonClicked()
    {
        MatchManager.SetRequestedPawnCoord.Invoke(_coordinate);
    }
    private void PlankDotClicked()
    {
        MatchManager.SetRequestedPlank.Invoke(_coordinate);
    }

}