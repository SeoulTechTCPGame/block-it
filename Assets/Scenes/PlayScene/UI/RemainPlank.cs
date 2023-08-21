using UnityEngine;
using UnityEngine.UI;

// 남은 플랭크의 갯수를 표시해주는 Class
public class RemainPlank : MonoBehaviour
{
    [SerializeField] Image _plankImage;

    private int _initPlankNum;
    private int _plankNum;
    private Image[] _plankImages;

    void Start()
    {
        
    }

    public void CreatePlank(int defaultPlankNum)
    {
        _initPlankNum = defaultPlankNum;
        _plankNum = defaultPlankNum;
        _plankImages = new Image[_plankNum];

        // Clear existing plank images
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }


        // Instantiate and position plank images based on _plankNum
        for (int i = 0; i < _plankNum; i++)
        {
            Image plankImage = Instantiate(_plankImage, transform);
            _plankImages[i] = plankImage;
        }
    }
    public void ReduceRemainPlank()
    {
        if(_plankNum > 0)
        {
            _plankNum--;
//            Destroy(_plankImages[_plankNum].gameObject);
            _plankImages[_plankNum].enabled = false;
        }

    }
    public void DisplayRemainPlank(int displayNum)
    {
        clearRemainPlank();

        if(displayNum>_initPlankNum || displayNum < 0)
        {
            Debug.Log("Remain Plank: Invalid displayNum has passed");
            return;
        }

        for(int i = 0; i < displayNum; i++)
        {
            _plankImages[i].enabled = true;
        }
    }
    private void clearRemainPlank()
    {
        if(_plankImages == null)
        {
            return;
        }
        foreach(Image image in _plankImages)
        {
            image.enabled = false;
        }
    }

}
