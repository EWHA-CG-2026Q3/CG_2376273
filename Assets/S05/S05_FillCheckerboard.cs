using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class S05_FillCheckerboard : MonoBehaviour
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

        FillCheckerboard(patternSize, colorA, colorB);

        canvasTexture.Apply();
        targetImage.texture = canvasTexture;
    }

    private void FillCheckerboard(int size, Color colorA, Color colorB)
    {
        for (int x = 0; x < canvasWidth; x++)
        {
            for (int y = 0; y < canvasHeight; y++)
            {
                // TODO: 줄무늬는 x만 봤지만, 체스판은 x와 y를 함께 고려해야 합니다.
                // 힌트: (x / size) + (y / size)의 결과를 활용해보세요.
                bool isColorA = ((x / size) + (y / size)) % 2 == 0;
                Color checkColor = isColorA ? colorA : colorB;

                canvasTexture.SetPixel(x, y, checkColor);
            }
        }
    }
}