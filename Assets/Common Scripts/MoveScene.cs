using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveScene : MonoBehaviour
{
    public void ToOption()
    {
        SceneManager.LoadScene("Option");
    }

    public void ToProfile() 
    {
        SceneManager.LoadScene("Profile");
    }

    public void ToHome()
    {
        SceneManager.LoadScene("Home");
    }
}
