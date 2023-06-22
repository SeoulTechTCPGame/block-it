using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    [SerializeField] float _TouchTimeout = 1f;
    private float _timer;

    private void Update()
    {
        _timer += Time.deltaTime;
        if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)||_timer >= _TouchTimeout)
        {
            SceneManager.LoadScene("Home");
        }
    }
}