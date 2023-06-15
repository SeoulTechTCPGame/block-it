using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    private Vector2Int Coordinate;

    [SerializeField] GameObject _rightPlank;
    [SerializeField] GameObject _bottomPlank;
    [SerializeField] GameObject _bottomRightPlank;
    [SerializeField] GameObject _box;

    private Image _rightPlankImage;
    private Image _bottomPlankImage;
    private Image[] _bottomRightPlankImages;

    // Start is called before the first frame update
    void Start()
    {
        _rightPlankImage = _rightPlank.GetComponentInChildren<Image>();
        _bottomPlankImage = _bottomPlank.GetComponentInChildren<Image>();

        GetAllImagesInChildren(_bottomRightPlank.transform);

        //
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetRightPlank(bool visible, Color color)
    {
        _rightPlank.gameObject.SetActive(visible);
        _rightPlankImage.enabled = visible;
        _rightPlankImage.color = color;
    }
    public void SetBottomPlank(bool visible, Color color)
    {
        _bottomPlank.gameObject.SetActive(visible);
        _bottomPlankImage.enabled = visible;
        _bottomPlankImage.color = color;
    }

    private void GetAllImagesInChildren(Transform parent)
    {
        Image[] childImages = parent.GetComponentsInChildren<Image>();
        _bottomRightPlankImages = new Image[childImages.Length];

        for (int i = 0; i < childImages.Length; i++)
        {
            _bottomRightPlankImages[i] = childImages[i];
        }
    }

}
