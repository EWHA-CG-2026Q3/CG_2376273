using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
[ExecuteInEditMode]
public class MakePlate : MonoBehaviour
{
    public int segments = 16;
    public float radius = 0.5f;

    void Start()
    {
        MeshFilter mf = GetComponent<MeshFilter>();
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[segments + 1];
        Vector2[] uvs = new Vector2[vertices.Length];

        vertices[0] = Vector3.zero;
        uvs[0] = new Vector2(0.5f, 0.5f);

        float angleStep = 360f / segments;
        for (int i = 0; i < segments; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;

            vertices[i + 1] = new Vector3(x, 0, z);
            uvs[i + 1] = new Vector2((x / radius + 1) * 0.5f, (z / radius + 1) * 0.5f);
        }

        int[] triangles = new int[segments * 3];
        for (int i = 0; i < segments; i++)
        {
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = (i + 1) % segments + 1;
            triangles[i * 3 + 2] = i + 1;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();

        mf.mesh = mesh;
    }
}