using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System.CodeDom;


/* Cell: 
 *   - Cell은 보드판의 칸 하나를 의미합니다.
 *   - Pawn, Plank를 표시하고, 놓을 수 있게 관리하는 클래스입니다. 
 *   - 'Cell'프리팹에 붙여서 사용됩니다.
 * 
 *   - Cell은 크게 다음의 것으로 이루어져있습니다: 
 *     - _coordinate: 해당 Cell이 보드판의 어느 위치(좌표)에 있는지 저장
 *     - _box: 보드의 칸 - 말(pawn)들이 위치할 수 있는 박스를 말합니다.
 *     - _pawn: pawn을 표시하거나, pawn이 이동할 수 있는 곳을 표시하고, 버튼을 생성해 유저가 pawn이 이동할 위치를 고를 수 있게 합니다.
 *     - _plankDot: _box우측 하단에 표시되는 작은 원 버튼으로, 유저가 plank를 놓을 수 있게 합니다.
 *     - _rightPlank, _bottomPlank, _bottomRightPlank: 플랭크를 표시합니다.
 */

public class Cell : MonoBehaviour
{
    private Vector2Int _coordinate = new Vector2Int(); //해당 Cell이 보드판의 어느 위치(좌표)에 있는지 저장

//rightPlank, _bottomPlank, _bottomRightPlank: 플랭크를 표시합니다.
    [SerializeField] GameObject _rightPlank;
    [SerializeField] GameObject _bottomPlank;
    [SerializeField] GameObject _bottomRightPlank;

    [SerializeField] GameObject _plankDot; //_box우측 하단에 표시되는 작은 원 버튼으로, 유저가 plank를 놓을 수 있게 합니다.
    [SerializeField] GameObject _box; //보드의 칸 - 말(pawn)들이 위치할 수 있는 박스를 말합니다.
    [SerializeField] GameObject _pawn; //pawn을 표시하거나, pawn이 이동할 수 있는 곳을 표시하고, 버튼을 생성해 유저가 pawn이 이동할 위치를 고를 수 있게 합니다.

    private bool _isRightEdge; // 맨 오른쪽에 있는 Cell인가?
    private bool _isBottomEdge; // 맨 아래에 있는 Cell인가?

    // 표시되는 이미지들
    private Image _rightPlankImage;
    private Image _bottomPlankImage;
    private Image _pawnImage;
    private Image _plankDotImage;
    [SerializeField] Image _plankDotImg;
    private Button _pawnButton;
    private Button _plankDotButton;
    private Dictionary<string, Image> _bottomRightDictionary;

    //필요한 Component를 가져오고, initialize합니다. 보드 오른/아래 끝에 있는 셀일 경우, plank/PlankDot을 비활성화 시킵니다.
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

    // 오른쪽/아래 끝 셀인지 세팅합니다. (bool값)
    public void SetEdge(bool rightEdge, bool bottomEdge)
    {
        _isRightEdge = rightEdge;
        _isBottomEdge = bottomEdge;
    }
    // 좌표를 설정합니다
    public void SetCoordinate(int col, int row)
    {
        _coordinate.x = col;
        _coordinate.y = row;
    }
    // 셀에 아무것도 표시되지 않게 합니다
    public void ClearCell()
    {
        RemovePawn();
        SetPlankDot(false, Color.white);
        SetRightPlank(false, Color.white);
        SetBottomPlank(false, Color.white);
        SetBottomRightPlank("Horizontal", false, Color.white);
    }
   
    // plank/PlankDot/Pawn를 표시를 제어합니다.
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
        _plankDotImg.enabled = visible;
        if (visible == true)
        {
            _plankDotImg.color = color;

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

    //pawn표시를 제거합니다.
    public void RemovePawn()
    {
        _pawnImage.color = Color.white;
        _pawnButton.enabled = false;
        _pawnButton.interactable = false;
        _pawnImage.enabled = false;

    }
    // 'pawn을 놓을 수 있는 버튼' 표시를 제어합니다.
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
    
    // 오른쪽 아래 플랭크 관련 매서드들 입니다
    // 초기화 관련 매서드
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
    // 다른 이미지들을 끕니다. 오른쪽 아래 플랭크의 모양은 단 하나이기 때문에, key와 다른 모양의 이미지들을 제거할때 쓰입니다.
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

    // Cell이 보드판의 (오른쪽/아래)끝에 위치해 있을 경우, plank/plankdot등을 비활성화 시킵니다.
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

    // PawnButton (pawan이 이동할 수 있는 경우, 표시되는 버튼)이 눌렸을때 불립니다.
    // - MatchManager에 pawn 이동할 좌표값을 전달합니다.
    private void PawnButtonClicked()
    {
        MatchManager.SetRequestedPawnCoord.Invoke(_coordinate);
    }
    // PlankDot이 눌릴경우 불립니다.
    // - MatchManager에 어느 위치에 Plank를 놓을지 좌표값을 전달합니다.
    private void PlankDotClicked()
    {
        MatchManager.SetRequestedPlank.Invoke(_coordinate);
    }
    public GameObject GetPlankDot()
    {
        return _plankDot;
    }
}