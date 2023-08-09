using UnityEngine;
using UnityEngine.UI;

public class WinState : MonoBehaviour
{
    private Text textComponent;

    private void Start()
    {
        // Get the Text component attached to this GameObject
        textComponent = GetComponent<Text>();

        if (textComponent == null)
        {
            Debug.LogError("No Text component found on this GameObject!");
            return;
        }

        textComponent.enabled = false;
    }

    public void DisplayWin(Enums.EPlayer ePlayer)
    {
        textComponent.enabled = true;

        if (ePlayer == Enums.EPlayer.Player1)
        {
            // Change text content
            textComponent.text = "Player 1 Wins!";
            // Change text color
            textComponent.color = new Color(0.96f, 0.57f, 0.15f);

        }
        else
        {
            // Change text content
            textComponent.text = "Player 2 Wins!";
            // Change text color
            textComponent.color = new Color(0.45f, 0.76f, 0.96f);
        }
    }
}
