using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class S07_DepthTest2 : MonoBehaviour
{
    [SerializeField] private int canvasWidth = 256;
    [SerializeField] private int canvasHeight = 256;

    // 강의 자료 슬라이드(S07_DepthTest2)의 정확한 배치 및 z값 설정
    // 삼각형 1 (파란색 - 가장 뒤쪽)
    [SerializeField] private Vector3 vertexA1 = new Vector3(145, 205, 0.7f);
    [SerializeField] private Vector3 vertexB1 = new Vector3(90, 45, 0.7f);
    [SerializeField] private Vector3 vertexC1 = new Vector3(200, 45, 0.7f);
    [SerializeField] private Color color1 = new Color(0.2f, 0.5f, 1f, 1f);

    // 삼각형 2 (주황색 - 중간)
    [SerializeField] private Vector3 vertexA2 = new Vector3(95, 180, 0.5f);
    [SerializeField] private Vector3 vertexB2 = new Vector3(55, 65, 0.5f);
    [SerializeField] private Vector3 vertexC2 = new Vector3(180, 65, 0.5f);
    [SerializeField] private Color color2 = new Color(1f, 0.4f, 0.2f, 1f);

    // 삼각형 3 (초록색 - 가장 앞쪽)
    [SerializeField] private Vector3 vertexA3 = new Vector3(210, 235, 0.3f);
    [SerializeField] private Vector3 vertexB3 = new Vector3(160, 35, 0.3f);
    [SerializeField] private Vector3 vertexC3 = new Vector3(250, 35, 0.3f);
    [SerializeField] private Color color3 = new Color(0.2f, 0.9f, 0.4f, 1f);

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

        // 강의 자료처럼 배경색을 회색(Color.gray / new Color(0.7f, 0.7f, 0.7f))으로 설정
        Color backgroundColor = new Color(0.7f, 0.7f, 0.7f, 1f);

        for (int x = 0; x < canvasWidth; x++)
        {
            for (int y = 0; y < canvasHeight; y++)
            {
                canvasTexture.SetPixel(x, y, backgroundColor);
                depthBuffer[x, y] = float.MaxValue;
            }
        }


        DrawTriangle(vertexA1, vertexB1, vertexC1, color1);
        DrawTriangle(vertexA2, vertexB2, vertexC2, color2);
        DrawTriangle(vertexA3, vertexB3, vertexC3, color3);

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

                bool isInside = w1 >= 0f && w2 >= 0f && w3 >= 0f;

                if (isInside)
                {
                    // TODO 1: w1, w2, w3와 a.z, b.z, c.z를 이용해 보간된 z를 계산
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