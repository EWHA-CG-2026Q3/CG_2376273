using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
[ExecuteInEditMode]
public class MakeBiscuit : MonoBehaviour
{
    void Start()
    {
        MeshFilter mf = GetComponent<MeshFilter>();
        Mesh mesh = new Mesh();

        // 도톰한 삼각 비스킷
        Vector3[] vertices = new Vector3[]
        {
            new Vector3(-0.15f, 0.05f, -0.1f),
            new Vector3( 0.15f, 0.05f, -0.1f),
            new Vector3( 0.0f,  0.05f,  0.15f),
            new Vector3(-0.15f, 0.0f,  -0.1f),
            new Vector3( 0.15f, 0.0f,  -0.1f),
            new Vector3( 0.0f,  0.0f,   0.15f)
        };

        int[] triangles = new int[]
        {
            0, 1, 2, // 윗면
            3, 5, 4, // 밑면
            0, 3, 1, 1, 3, 4, // 옆면 1
            1, 4, 2, 2, 4, 5, // 옆면 2
            2, 5, 0, 0, 5, 3  // 옆면 3
        };

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        mf.mesh = mesh;
    }
}