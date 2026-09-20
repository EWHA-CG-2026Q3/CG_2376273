using UnityEngine;
[ExecuteAlways]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class S03_CustomPolygonMesh_Square : MonoBehaviour
{
    void Start()
    {
        // TODO 1: 정점 5개 좌표 작성 (오각형 형태)
        Vector3[] vertices = new Vector3[]
        {
            new Vector3(0f, 1.5f, 0f),    // 0번: 맨 위 꼭짓점
            new Vector3(1.43f, 0.46f, 0f),  // 1번: 오른쪽 위
            new Vector3(0.88f, -1.21f, 0f),     // 2번: 오른쪽 아래
            new Vector3(-0.88f, -1.21f, 0f),    // 3번: 왼쪽 아래
            new Vector3(-1.43f, 0.46f, 0f)  // 4번: 왼쪽 위
        };

        // TODO 2: 시계 방향으로 삼각형 3개 연결 (총 9개 인덱스)
        int[] triangles = new int[]
        {
            0, 1, 4, // 첫 번째 삼각형 (위쪽)
            1, 2, 4, // 두 번째 삼각형 (중앙)
            2, 3, 4  // 세 번째 삼각형 (아래쪽)
        };

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
    }
}