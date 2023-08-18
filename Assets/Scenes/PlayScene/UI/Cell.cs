using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;


/* Cell: 
 *   - Cell�� �������� ĭ �ϳ��� �ǹ��մϴ�.
 *   - Pawn, Plank�� ǥ���ϰ�, ���� �� �ְ� �����ϴ� Ŭ�����Դϴ�. 
 *   - 'Cell'�����տ� �ٿ��� ���˴ϴ�.
 * 
 *   - Cell�� ũ�� ������ ������ �̷�����ֽ��ϴ�: 
 *     - _coordinate: �ش� Cell�� �������� ��� ��ġ(��ǥ)�� �ִ��� ����
 *     - _box: ������ ĭ - ��(pawn)���� ��ġ�� �� �ִ� �ڽ��� ���մϴ�.
 *     - _pawn: pawn�� ǥ���ϰų�, pawn�� �̵��� �� �ִ� ���� ǥ���ϰ�, ��ư�� ������ ������ pawn�� �̵��� ��ġ�� �� �� �ְ� �մϴ�.
 *     - _plankDot: _box���� �ϴܿ� ǥ�õǴ� ���� �� ��ư����, ������ plank�� ���� �� �ְ� �մϴ�.
 *     - _rightPlank, _bottomPlank, _bottomRightPlank: �÷�ũ�� ǥ���մϴ�.
 */

public class Cell : MonoBehaviour
{
    private Vector2Int _coordinate = new Vector2Int(); //�ش� Cell�� �������� ��� ��ġ(��ǥ)�� �ִ��� ����

//rightPlank, _bottomPlank, _bottomRightPlank: �÷�ũ�� ǥ���մϴ�.
    [SerializeField] GameObject _rightPlank;
    [SerializeField] GameObject _bottomPlank;
    [SerializeField] GameObject _bottomRightPlank;

    [SerializeField] GameObject _plankDot; //_box���� �ϴܿ� ǥ�õǴ� ���� �� ��ư����, ������ plank�� ���� �� �ְ� �մϴ�.
    [SerializeField] GameObject _box; //������ ĭ - ��(pawn)���� ��ġ�� �� �ִ� �ڽ��� ���մϴ�.
    [SerializeField] GameObject _pawn; //pawn�� ǥ���ϰų�, pawn�� �̵��� �� �ִ� ���� ǥ���ϰ�, ��ư�� ������ ������ pawn�� �̵��� ��ġ�� �� �� �ְ� �մϴ�.

    private bool _isRightEdge; // �� �����ʿ� �ִ� Cell�ΰ�?
    private bool _isBottomEdge; // �� �Ʒ��� �ִ� Cell�ΰ�?

    // ǥ�õǴ� �̹�����
    private Image _rightPlankImage;
    private Image _bottomPlankImage;
    private Image _pawnImage;
    private Image _plankDotImage;
    private Button _pawnButton;
    private Button _plankDotButton;
    private Dictionary<string, Image> _bottomRightDictionary;

    //�ʿ��� Component�� ��������, initialize�մϴ�. ���� ����/�Ʒ� ���� �ִ� ���� ���, plank/PlankDot�� ��Ȱ��ȭ ��ŵ�ϴ�.
    private void Awake()
    {
        _bottomRightDictionary = new Dictionary<string, Image>();
        _rightPlankImage = _rightPlank.GetComponentInChildren<Image>();
        _bottomPlankImage = _bottomPlank.GetComponentInChildren<Image>();
        _pawnImage = _pawn.GetComponentInChildren<Image>();
        _plankDotImage = _plankDot.GetComponent<Image>();
        _pawnButton = _pawn.GetComponent<Button>();
        _plankDotButton = _plankDot.GetComponent<Button>();


        _pawnButton.onClick.AddListener(() => PawnButtonClicked());
        _plankDotButton.onClick.AddListener(() => PlankDotClicked());

        InitBottomRightPlanks();
        OffEdge();
    }

    // ������/�Ʒ� �� ������ �����մϴ�. (bool��)
    public void SetEdge(bool rightEdge, bool bottomEdge)
    {
        _isRightEdge = rightEdge;
        _isBottomEdge = bottomEdge;
    }
    // ��ǥ�� �����մϴ�
    public void SetCoordinate(int col, int row)
    {
        _coordinate.x = col;
        _coordinate.y = row;
    }
    // ���� �ƹ��͵� ǥ�õ��� �ʰ� �մϴ�
    public void ClearCell()
    {
        RemovePawn();
        SetPlankDot(false, Color.white);
        SetRightPlank(false, Color.white);
        SetBottomPlank(false, Color.white);
        SetBottomRightPlank("Horizontal", false, Color.white);
    }
   
    // plank/PlankDot/Pawn�� ǥ�ø� �����մϴ�.
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
    public void SetPlankDot(bool visible, Color color)
    {
        _plankDotButton.enabled = visible;
        _plankDotImage.enabled = visible;
        if(visible == true)
        {
            _plankDotImage.color = color;
        }
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

    //pawnǥ�ø� �����մϴ�.
    public void RemovePawn()
    {
        _pawnImage.color = Color.white;
        _pawnButton.enabled = false;
        _pawnButton.interactable = false;
        _pawnImage.enabled = false;

    }
    // 'pawn�� ���� �� �ִ� ��ư' ǥ�ø� �����մϴ�.
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
    
    // ������ �Ʒ� �÷�ũ ���� �ż���� �Դϴ�
    // �ʱ�ȭ ���� �ż���
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
    // �ٸ� �̹������� ���ϴ�. ������ �Ʒ� �÷�ũ�� ����� �� �ϳ��̱� ������, key�� �ٸ� ����� �̹������� �����Ҷ� ���Դϴ�.
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

    // Cell�� �������� (������/�Ʒ�)���� ��ġ�� ���� ���, plank/plankdot���� ��Ȱ��ȭ ��ŵ�ϴ�.
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

    // PawnButton (pawan�� �̵��� �� �ִ� ���, ǥ�õǴ� ��ư)�� �������� �Ҹ��ϴ�.
    // - MatchManager�� pawn �̵��� ��ǥ���� �����մϴ�.
    private void PawnButtonClicked()
    {
        MatchManager.SetRequestedPawnCoord.Invoke(_coordinate);
    }
    // PlankDot�� ������� �Ҹ��ϴ�.
    // - MatchManager�� ��� ��ġ�� Plank�� ������ ��ǥ���� �����մϴ�.
    private void PlankDotClicked()
    {
        MatchManager.SetRequestedPlank.Invoke(_coordinate);
    }

}