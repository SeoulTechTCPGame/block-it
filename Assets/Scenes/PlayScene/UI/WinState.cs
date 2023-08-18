using UnityEngine;
using UnityEngine.UI;

// 플레이어 중 하나가 이기면, Win Massage를 띄운다.
public class WinState : MonoBehaviour
{
    private Text _textComponent;

    private void Start()
    {
        // Get the Text component attached to this GameObject
        _textComponent = GetComponent<Text>();

        if (_textComponent == null)
        {
            Debug.LogError("No Text component found on this GameObject!");
            return;
        }

        _textComponent.enabled = false;
    }

    public void DisplayWin(Enums.EPlayer ePlayer)
    {
        _textComponent.enabled = true;

        if (ePlayer == Enums.EPlayer.Player1)
        {
            // Change text content
            _textComponent.text = "Player 1 Wins!";
            // Change text color
            _textComponent.color = new Color(0.96f, 0.57f, 0.15f);

        }
        else
        {
            // Change text content
            _textComponent.text = "Player 2 Wins!";
            // Change text color
            _textComponent.color = new Color(0.45f, 0.76f, 0.96f);
        }
    }
}
