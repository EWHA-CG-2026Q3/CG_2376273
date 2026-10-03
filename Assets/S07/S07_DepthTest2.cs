using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class S07_DepthTest2 : MonoBehaviour
{
    [SerializeField] private int canvasWidth = 256;
    [SerializeField] private int canvasHeight = 256;

    // 겹치는 영역 안에서 앞뒤가 교차되도록 z값을 서로 엇갈리게 설계
    [SerializeField] private Vector3 vertexA1 = new Vector3(120, 220, 0.2f); // 삼각형 1 (파랑)
    [SerializeField] private Vector3 vertexB1 = new Vector3(40, 60, 0.8f);
    [SerializeField] private Vector3 vertexC1 = new Vector3(200, 60, 0.8f);
    [SerializeField] private Color color1 = new Color(0.2f, 0.5f, 1f, 1f);

    [SerializeField] private Vector3 vertexA2 = new Vector3(80, 180, 0.8f);  // 삼각형 2 (주황)
    [SerializeField] private Vector3 vertexB2 = new Vector3(10, 40, 0.2f);
    [SerializeField] private Vector3 vertexC2 = new Vector3(150, 40, 0.8f);
    [SerializeField] private Color color2 = new Color(1f, 0.5f, 0.2f, 1f);

    private Texture2D canvasTexture;
    private RawImage targetImage;
    private float[,] depthBuffer;

    private void OnEnable() { RedrawAll(); }
    private void OnValidate() { RedrawAll(); }

    private void RedrawAll()
    {
        targetImage = GetComponent<RawImage>();
        if (targetImage == null) return;

        if (canvasTexture == null || canvasTexture.width != canvasWidth || canvasTexture.height != canvasHeight)
        {
            canvasTexture = new Texture2D(canvasWidth, canvasHeight);
            canvasTexture.filterMode = FilterMode.Point;
        }

        depthBuffer = new float[canvasWidth, canvasHeight];
        for (int x = 0; x < canvasWidth; x++)
        {
            for (int y = 0; y < canvasHeight; y++)
            {
                canvasTexture.SetPixel(x, y, Color.black);
                depthBuffer[x, y] = float.MaxValue;
            }
        }

        // TODO 0: 어느 순서로 그려도 z-buffer 덕분에 결과가 동일함
        DrawTriangle(vertexA1, vertexB1, vertexC1, color1);
        DrawTriangle(vertexA2, vertexB2, vertexC2, color2);

        canvasTexture.Apply();
        targetImage.texture = canvasTexture;
    }

    private void DrawTriangle(Vector3 a, Vector3 b, Vector3 c, Color color)
    {
        for (int x = 0; x < canvasWidth; x++)
        {
            for (int y = 0; y < canvasHeight; y++)
            {
                Vector2 p = new Vector2(x + 0.5f, y + 0.5f);
                float denom = a.x * (b.y - c.y) + b.x * (c.y - a.y) + c.x * (a.y - b.y);
                if (Mathf.Approximately(denom, 0f)) continue;

                float w1 = (p.x * (b.y - c.y) + b.x * (c.y - p.y) + c.x * (p.y - b.y)) / denom;
                float w2 = (a.x * (p.y - c.y) + p.x * (c.y - a.y) + c.x * (a.y - p.y)) / denom;
                float w3 = 1f - w1 - w2;

                if (w1 >= 0f && w2 >= 0f && w3 >= 0f)
                {
                    // TODO 1: w1, w2, w3와 a.z, b.z, c.z를 이용해 보간된 z 계산
                    float interpolatedZ = w1 * a.z + w2 * b.z + w3 * c.z;

                    // TODO 2: interpolatedZ가 depthBuffer[x, y]보다 작을 때만 갱신
                    if (interpolatedZ < depthBuffer[x, y])
                    {
                        canvasTexture.SetPixel(x, y, color);
                        depthBuffer[x, y] = interpolatedZ;
                    }
                }
            }
        }
    }
}