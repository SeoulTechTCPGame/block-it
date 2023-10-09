using UnityEngine;
using UnityEngine.UI;

public class ReplayButton : MonoBehaviour
{
    [SerializeField] GameObject _replayText;
    [SerializeField] GameObject _replayButton;
    [SerializeField] GameObject _leftButton;
    [SerializeField] GameObject _rightButton;

    private bool _replayClicked = false;
    private int _index = 0;
    private int _MaxIndex = 0;

    public void SetMaxIndex(int maxIndex)
    {
        _MaxIndex = maxIndex;
        _leftButton.GetComponent<Button>().onClick.AddListener(OnLeftButtonClicked);
        _rightButton.GetComponent<Button>().onClick.AddListener(OnRightButtonClicked);
    }
    public void ReplayPressed()
    {
        _replayClicked = true;
        SetButton(true, _index);
    }
    public void SetButton(bool pressed, int index)
    {
        _replayClicked = pressed;
        _index = index;
        if (pressed) 
        { 
            _leftButton.SetActive(true);
            _rightButton.SetActive(true);
            _replayText.SetActive(false);

            if(_index == 0)
            {
                _rightButton.GetComponent<Button>().interactable = true;
                _leftButton.GetComponent<Button>().interactable = false;
            }
            else if(index >= _MaxIndex) 
            { 
                _rightButton.GetComponent<Button>().interactable = false;
                _leftButton.GetComponent<Button>().interactable = true;
            }
            MatchManager.ShowRecord.Invoke(_index);
        }
        else 
        {
            _leftButton.SetActive(false);
            _rightButton.SetActive(false);
            _replayText.SetActive(true);
        }

    }
    public void OnRightButtonClicked()
    {
        _index++;
        SetButton(true, _index);
    }
    public void OnLeftButtonClicked()
    {
        _index--;
        SetButton(true, _index);
    }
}
