using UnityEngine;
[RequireComponent(typeof(RectTransform))]
public class BackgroundScaler : MonoBehaviour
{
    private RectTransform rectTransform;

    private int lastScreenWidth;
    private int lastScreenHeight;

    public Vector2 referenceResolution = new Vector2(1080, 1920); 

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        lastScreenWidth = Screen.width;
        lastScreenHeight = Screen.height;

        FitToScreen();
    }

    void Update()
    {
        if (Screen.width != lastScreenWidth || Screen.height != lastScreenHeight)
        {
            lastScreenWidth = Screen.width;
            lastScreenHeight = Screen.height;
            FitToScreen();
        }
    }

    void FitToScreen()
    {
        float screenRatio = (float)Screen.width / Screen.height;
        float refRatio = referenceResolution.x / referenceResolution.y;

        Vector2 size = Vector2.zero;

        if (screenRatio >= refRatio)
        {
            float height = referenceResolution.y;
            float width = height * screenRatio;
            size = new Vector2(width, height);
        }
        else
        {
            float width = referenceResolution.x;
            float height = width / screenRatio;
            size = new Vector2(width, height);
        }

        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.sizeDelta = size;
        rectTransform.anchoredPosition = Vector2.zero;
    }
}
