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
        _textComponent.text = _currentTime.ToString();
        _textComponent.color = new Color(0.96f, 0.57f, 0.15f);
    }
}
