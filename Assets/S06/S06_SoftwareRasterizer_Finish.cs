using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class S06_SoftwareRasterizer_Finish : MonoBehaviour
{
    [SerializeField] private int canvasWidth = 256;
    [SerializeField] private int canvasHeight = 256;
    
    // 요구사항: 꼭짓점 좌표와 색상을 수정 (뾰족하거나 넓은 다른 모양으로 설정)
    [SerializeField] private Vector2 vertexA = new Vector2(128, 230); // 위쪽
    [SerializeField] private Vector2 vertexB = new Vector2(20, 30);   // 왼쪽 아래
    [SerializeField] private Vector2 vertexC = new Vector2(236, 30);  // 오른쪽 아래
    [SerializeField] private Color fillColor = new Color(0.2f, 0.8f, 0.4f, 1f); // 보라/초록 등 수정된 색상
    [SerializeField] private Color backgroundColor = new Color(0f, 0f, 0f, 1f);

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

        DrawTriangle(vertexA, vertexB, vertexC, fillColor);

        canvasTexture.Apply();
        targetImage.texture = canvasTexture;
    }

    private void FillBackground(Color color)
    {
        for (int x = 0; x < canvasWidth; x++)
        {
            for (int y = 0; y < canvasHeight; y++)
            {
                canvasTexture.SetPixel(x, y, color);
            }
        }
    }

    private void DrawTriangle(Vector2 a, Vector2 b, Vector2 c, Color color)
    {
        FillBackground(backgroundColor);

        for (int x = 0; x < canvasWidth; x++)
        {
            for (int y = 0; y < canvasHeight; y++)
            {
                Vector2 pixelCenter = new Vector2(x + 0.5f, y + 0.5f);
                if (IsInsideTriangle(pixelCenter, a, b, c))
                {
                    canvasTexture.SetPixel(x, y, color);
                }
            }
        }
    }

    private bool IsInsideTriangle(Vector2 p, Vector2 a, Vector2 b, Vector2 c)
    {
        float denom = a.x * (b.y - c.y) + b.x * (c.y - a.y) + c.x * (a.y - b.y);
        if (Mathf.Approximately(denom, 0f)) return false;

        float w1 = (p.x * (b.y - c.y) + b.x * (c.y - p.y) + c.x * (p.y - b.y)) / denom;
        float w2 = (a.x * (p.y - c.y) + p.x * (c.y - a.y) + c.x * (a.y - p.y)) / denom;
        float w3 = 1f - w1 - w2;

        return w1 >= 0f && w2 >= 0f && w3 >= 0f;
    }
}