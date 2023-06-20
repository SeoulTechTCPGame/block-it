using UnityEngine;
using UnityEngine.UI;

public class RemainPlank : MonoBehaviour
{
    [SerializeField] Image PlankImage;

    public int PlankNum;

    void Start()
    {
        setPlank();
    }

    private void setPlank()
    {
        // Clear existing plank images
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Instantiate and position plank images based on plankNum
        for (int i = 0; i < PlankNum; i++)
        {
            Image plankImage = Instantiate(PlankImage, transform);// plankContainer);
            // Set plank image properties if needed (e.g., sprite, color, etc.)
        }
    }
}
