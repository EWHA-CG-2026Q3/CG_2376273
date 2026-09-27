using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class S05_FillVerticalStripes : MonoBehaviour
{
    [SerializeField] private int canvasWidth = 256;
    [SerializeField] private int canvasHeight = 256;
    [SerializeField] private int patternSize = 16;
    [SerializeField] private Color colorA = new Color(1f, 1f, 1f, 1f); // 흰색
    [SerializeField] private Color colorB = new Color(0.3f, 0.5f, 0.8f, 1f); // 하늘색

    private Texture2D canvasTexture;
    private RawImage targetImage;

    private void OnEnable()
    {
        RenderCanvas();
    }

    private void OnValidate()
    {
        RenderCanvas();
    }

    private void RenderCanvas()
    {
        targetImage = GetComponent<RawImage>();
        if (targetImage == null) return;

        canvasTexture = new Texture2D(canvasWidth, canvasHeight);
        canvasTexture.filterMode = FilterMode.Point;

        FillVerticalStripes(patternSize, colorA, colorB);

        canvasTexture.Apply();
        targetImage.texture = canvasTexture;
    }

    private void FillVerticalStripes(int width, Color colorA, Color colorB)
    {
        for (int x = 0; x < canvasWidth; x++)
        {
            // TODO: x를 width로 나눈 몫이 짝수면 colorA, 홀수면 colorB가 되도록
            // isColorA를 올바른 조건식으로 바꾸세요.
            // 힌트: (x / width) % 2 == 0
            bool isColorA = (x / width) % 2 == 0;

            Color stripeColor = isColorA ? colorA : colorB;
            for (int y = 0; y < canvasHeight; y++)
            {
                canvasTexture.SetPixel(x, y, stripeColor);
            }
        }
    }
}