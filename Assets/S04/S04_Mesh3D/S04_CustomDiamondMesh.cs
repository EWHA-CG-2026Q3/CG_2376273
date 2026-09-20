using UnityEngine;
[ExecuteAlways]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class S04_CustomDiamondMesh : MonoBehaviour
{
    void Start()
    {
        Vector3[] vertices = new Vector3[]
        {
            new Vector3(0f, 0f, 0f),     // 0 
            new Vector3(1f, 0f, 0f),     // 1 
            new Vector3(1f, 0f, 1f),     // 2 
            new Vector3(0f, 0f, 1f),     // 3 
            new Vector3(0.5f, 1f, 0.5f),  // 4 
            new Vector3(0.5f, -1f, 0.5f), // 5 
        };

        // TODO: 위쪽 삼각형 4개(정점 4 + 허리띠 인접 두 점)와
        //       아래쪽 삼각형 4개(정점 5 + 허리띠 인접 두 점)를 채우세요.
        int[] triangles = new int[]
        {
            // 위쪽 4면 (정점 4 사용)
            4, 1, 0, // (0->1 방향)
            4, 2, 1, // (1->2 방향)
            4, 3, 2, // (2->3 방향)
            4, 0, 3, // (3->0 방향)

            // 아래쪽 4면 (정점 5 사용)
            5, 0, 1, // 앞쪽 면
            5, 1, 2, // 오른쪽 면
            5, 2, 3, // 뒤쪽 면
            5, 3, 0  // 왼쪽 면     
            // 아래쪽 4면 (정점 5 사용)
        };

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
    }
}