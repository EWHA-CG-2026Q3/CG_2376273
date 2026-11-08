using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class MakeTeaCup : MonoBehaviour
{
    public int segments = 16; // 원기둥 정점 개수
    public float radius = 0.3f; // 찻잔 반지름
    public float height = 0.5f; // 찻잔 높이

    void Start()
    {
        MeshFilter mf = GetComponent<MeshFilter>();
        Mesh mesh = new Mesh();

        // 1. for문으로 원형 정점 배치 (위쪽 원 + 아래쪽 원)
        Vector3[] vertices = new Vector3[segments * 2 + 2];
        Vector2[] uvs = new Vector2[vertices.Length];

        // 중심점
        vertices[0] = new Vector3(0, height, 0); // 위 중심
        vertices[1] = new Vector3(0, 0, 0);      // 아래 중심
        uvs[0] = new Vector2(0.5f, 0.5f);
        uvs[1] = new Vector2(0.5f, 0.5f);

        float angleStep = 360f / segments;
        for (int i = 0; i < segments; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;

            // 위쪽 원 정점
            vertices[2 + i] = new Vector3(x, height, z);
            // 아래쪽 원 정점
            vertices[2 + segments + i] = new Vector3(x, 0, z);

            uvs[2 + i] = new Vector2((x / radius + 1) * 0.5f, (z / radius + 1) * 0.5f);
            uvs[2 + segments + i] = uvs[2 + i];
        }

        // 2. 삼각형 면 연결
        int[] triangles = new int[segments * 12];
        int t = 0;

        for (int i = 0; i < segments; i++)
        {
            int next = (i + 1) % segments;
            int topCurr = 2 + i;
            int topNext = 2 + next;
            int botCurr = 2 + segments + i;
            int botNext = 2 + segments + next;

            // 옆면 (삼각형 2개)
            triangles[t++] = topCurr;
            triangles[t++] = topNext;
            triangles[t++] = botCurr;

            triangles[t++] = topNext;
            triangles[t++] = botNext;
            triangles[t++] = botCurr;

            // 윗면
            triangles[t++] = 0;
            triangles[t++] = topNext;
            triangles[t++] = topCurr;

            // 밑면
            triangles[t++] = 1;
            triangles[t++] = botCurr;
            triangles[t++] = botNext;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();

        mf.mesh = mesh;
    }
}