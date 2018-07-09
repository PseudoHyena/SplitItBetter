using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class PanelGradient : MonoBehaviour {

    [SerializeField] Color leftColor;
    [SerializeField] Color RightColor;

    Image image;

    RectTransform rectTransform;

    private void Start() {
        image = GetComponent<Image>();
        rectTransform = transform.parent.GetComponent<RectTransform>();

        CreateGradient();
        
    }

    private void CreateGradient() {
        int width = (int)rectTransform.rect.width;
        int height = (int)rectTransform.rect.height;

        Texture2D gradientTexture = new Texture2D(width, height);

        Color[] colorMap = new Color[width * height];
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                colorMap[y * width + x] = Color.Lerp(leftColor, RightColor, x / (float)width);
            }
        }
        
        gradientTexture.SetPixels(colorMap);
        gradientTexture.Apply();

        image.sprite = Sprite.Create(gradientTexture, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f), 5);
    }
}
