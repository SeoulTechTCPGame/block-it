using UnityEngine;
using UnityEngine.UI;

public class RemainPlank : MonoBehaviour
{
    [SerializeField] Image PlankImage;

    private int plankNum;
    private Image[] plankImages;

    void Start()
    {
        
    }

    public void CreatePlank(int defaultPlankNum)
    {
        plankNum = defaultPlankNum;
        plankImages = new Image[plankNum];

        // Clear existing plank images
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }


        // Instantiate and position plank images based on plankNum
        for (int i = 0; i < plankNum; i++)
        {
            Image plankImage = Instantiate(PlankImage, transform);
            plankImages[i] = plankImage;
        }
    }
    public void ReduceRemainPlank()
    {
        if(plankNum > 0)
        {
            plankNum--;
            Destroy(plankImages[plankNum].gameObject);
        }

    }
}
