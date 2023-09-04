using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private float _currentTime;
    private Text _textComponent;

    // Start is called before the first frame update
    void Start()
    {
        _textComponent = GetComponent<Text>();

        if (_textComponent == null)
        {
            Debug.LogError("No Text component found on this GameObject!");
            return;
        }

    }

    public void ShowTimer()
    {
        _textComponent.enabled = true;
    }

    public void HideTimer()
    {
        _textComponent.enabled = false;
    }

    public void SetCurrentTime(float currentTime)
    {
        _currentTime = currentTime;
        UpdateTimer();
    }

    private void UpdateTimer()
    {
        int mininute = ((int)(_currentTime / 60f));
        int second = (int)(_currentTime%60f);
        _textComponent.text = mininute.ToString() + " : " + second.ToString();
        if(_currentTime <= 30f)
        {
            _textComponent.color = new Color(1f, 0.3f, 0.3f);
        }
        else
        { 
            _textComponent.color = new Color(0f, 0f, 0f);
        }
    }
}
