using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[ExecuteAlways]
public class S07_Clipping : MonoBehaviour
{
    [SerializeField] private int canvasWidth = 256;
    [SerializeField] private int canvasHeight = 256;
    [SerializeField] private int clipMargin = 40;

    // 왼쪽(x < 40)과 위쪽(y > 216) 두 경계를 동시에 넘어가도록 꼭짓점 설정
    [SerializeField] private List<Vector2> polygon = new List<Vector2> {
        new Vector2(10, 130),   // 왼쪽 경계(margin 40) 밖으로 나감
        new Vector2(130, 250),  // 위쪽 경계(256 - 40 = 216) 밖으로 나감
        new Vector2(240, 130)   // 안쪽 영역
    };

    private Texture2D canvasTexture;
    private RawImage targetImage;

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

        Color backgroundColor = new Color(0.7f, 0.7f, 0.7f, 1f);
        for (int x = 0; x < canvasWidth; x++)
        {
            for (int y = 0; y < canvasHeight; y++)
            {
                canvasTexture.SetPixel(x, y, backgroundColor);
            }
        }

        // Clip 수행
        List<Vector2> clipped = polygon;
        clipped = ClipLeft(clipped, clipMargin);
        clipped = ClipRight(clipped, canvasWidth - 1 - clipMargin);
        clipped = ClipBottom(clipped, clipMargin);
        clipped = ClipTop(clipped, canvasHeight - 1 - clipMargin);

        // 잘린 다각형 채우기
        FillPolygon(clipped, Color.orange);

        // 외각 여백 라인 그리기
        DrawMarginOutline(clipMargin, Color.black);

        canvasTexture.Apply();
        targetImage.texture = canvasTexture;
    }

    private void DrawMarginOutline(int margin, Color color)
    {
        int minX = margin;
        int maxX = canvasWidth - 1 - margin;
        int minY = margin;
        int maxY = canvasHeight - 1 - margin;

        for (int x = minX; x <= maxX; x++)
        {
            canvasTexture.SetPixel(x, minY, color);
            canvasTexture.SetPixel(x, maxY, color);
        }
        for (int y = minY; y <= maxY; y++)
        {
            canvasTexture.SetPixel(minX, y, color);
            canvasTexture.SetPixel(maxX, y, color);
        }
    }

    private List<Vector2> ClipLeft(List<Vector2> input, float boundary)
    {
        List<Vector2> output = new List<Vector2>();
        if (input == null || input.Count == 0) return output;

        for (int i = 0; i < input.Count; i++)
        {
            Vector2 current = input[i];
            Vector2 prev = input[(i + input.Count - 1) % input.Count];

            bool currInside = current.x >= boundary;
            bool prevInside = prev.x >= boundary;

            if (currInside)
            {
                if (!prevInside)
                    output.Add(GetIntersectionX(prev, current, boundary));
                output.Add(current);
            }
            else if (prevInside)
            {
                output.Add(GetIntersectionX(prev, current, boundary));
            }
        }
        return output;
    }

    // 오른쪽 경계(x <= boundary)로 자르는 함수
    private List<Vector2> ClipRight(List<Vector2> input, float boundary)
    {
        List<Vector2> output = new List<Vector2>();
        if (input == null || input.Count == 0) return output;

        for (int i = 0; i < input.Count; i++)
        {
            Vector2 current = input[i];
            Vector2 prev = input[(i + input.Count - 1) % input.Count];

            bool currInside = current.x <= boundary;
            bool prevInside = prev.x <= boundary;

            if (currInside)
            {
                if (!prevInside)
                    output.Add(GetIntersectionX(prev, current, boundary));
                output.Add(current);
            }
            else if (prevInside)
            {
                output.Add(GetIntersectionX(prev, current, boundary));
            }
        }
        return output;
    }

    // 아래쪽 경계(y >= boundary)로 자르는 함수
    private List<Vector2> ClipBottom(List<Vector2> input, float boundary)
    {
        List<Vector2> output = new List<Vector2>();
        if (input == null || input.Count == 0) return output;

        for (int i = 0; i < input.Count; i++)
        {
            Vector2 current = input[i];
            Vector2 prev = input[(i + input.Count - 1) % input.Count];

            bool currInside = current.y >= boundary;
            bool prevInside = prev.y >= boundary;

            if (currInside)
            {
                if (!prevInside)
                    output.Add(GetIntersectionY(prev, current, boundary));
                output.Add(current);
            }
            else if (prevInside)
            {
                output.Add(GetIntersectionY(prev, current, boundary));
            }
        }
        return output;
    }

    // 위쪽 경계(y <= boundary)로 자르는 함수
    private List<Vector2> ClipTop(List<Vector2> input, float boundary)
    {
        List<Vector2> output = new List<Vector2>();
        if (input == null || input.Count == 0) return output;

        for (int i = 0; i < input.Count; i++)
        {
            Vector2 current = input[i];
            Vector2 prev = input[(i + input.Count - 1) % input.Count];

            bool currInside = current.y <= boundary;
            bool prevInside = prev.y <= boundary;

            if (currInside)
            {
                if (!prevInside)
                    output.Add(GetIntersectionY(prev, current, boundary));
                output.Add(current);
            }
            else if (prevInside)
            {
                output.Add(GetIntersectionY(prev, current, boundary));
            }
        }
        return output;
    }

    private Vector2 GetIntersectionX(Vector2 p1, Vector2 p2, float boundaryX)
    {
        if (Mathf.Approximately(p1.x, p2.x)) return p1;

        float t = (boundaryX - p1.x) / (p2.x - p1.x);
        float intersectionY = p1.y + t * (p2.y - p1.y);
        return new Vector2(boundaryX, intersectionY);
    }

    // y 기준 교차점을 구하는 함수
    private Vector2 GetIntersectionY(Vector2 p1, Vector2 p2, float boundaryY)
    {
        if (Mathf.Approximately(p1.y, p2.y)) return p1;

        float t = (boundaryY - p1.y) / (p2.y - p1.y);
        float intersectionX = p1.x + t * (p2.x - p1.x);
        return new Vector2(intersectionX, boundaryY);
    }

    private void FillPolygon(List<Vector2> poly, Color color)
    {
        if (poly == null || poly.Count < 3) return;

        Vector2 v0 = poly[0];
        for (int i = 1; i < poly.Count - 1; i++)
        {
            DrawTriangle(v0, poly[i], poly[i + 1], color);
        }
    }

    private void DrawTriangle(Vector2 a, Vector2 b, Vector2 c, Color color)
    {
        for (int x = 0; x < canvasWidth; x++)
        {
            for (int y = 0; y < canvasHeight; y++)
            {
                Vector2 p = new Vector2(x + 0.5f, y + 0.5f);
                if (IsInsideTriangle(p, a, b, c))
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