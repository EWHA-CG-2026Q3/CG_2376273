using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(DiamondMesh))]
public class S09_Shear : MonoBehaviour
{
    // [학번 공식] k = (학번 끝자리 + 1) / 5f
    // Inspector에서 조절 가능하도록 노출
    [SerializeField] float k = 1.0f; 

    DiamondMesh diamondMesh;

    void OnEnable()
    {
        diamondMesh = GetComponent<DiamondMesh>();
    }

    void Update()
    {
        if (diamondMesh == null || diamondMesh.BaseVertices == null) return;

        float[,] M = ShearMatrixRaw(k);
        Vector3[] baseVertices = diamondMesh.BaseVertices;
        Vector3[] verts = new Vector3[baseVertices.Length];

        for (int i = 0; i < baseVertices.Length; i++)
        {
            Vector4 h = ToHomogeneous(baseVertices[i]);
            h = MultiplyMatrixVectorRaw(M, h);
            verts[i] = FromHomogeneous(h);
        }

        diamondMesh.SetVertices(verts);
    }

    // Inspector 값을 변경할 때마다 꼭대기 정점 연산 결과를 Console에 출력
    void OnValidate()
    {
        float[,] M = ShearMatrixRaw(k);
        Vector4 topVertex = ToHomogeneous(new Vector3(0.5f, 1f, 0.5f));
        Vector4 transformedTop = MultiplyMatrixVectorRaw(M, topVertex);

        Debug.Log($"[k = {k}] 꼭대기 정점 (0.5, 1, 0.5) 변환 결과 → ({transformedTop.x}, {transformedTop.y}, {transformedTop.z})");
    }

    // ★ 과제 핵심: 4x4 Shear 행렬 (2열에 변환 후 e2 = (k, 1, 0, 0) 대입)
    float[,] ShearMatrixRaw(float k)
    {
        return new float[,] {
            { 1f,  k, 0f, 0f }, // 1열: e1(1,0,0,0) / 2열: e2(k,1,0,0)
            { 0f, 1f, 0f, 0f }, // 3열: e3(0,0,1,0) / 4열: 원점(0,0,0,1)
            { 0f, 0f, 1f, 0f },
            { 0f, 0f, 0f, 1f }
        };
    }

    Vector4 ToHomogeneous(Vector3 v) => new Vector4(v.x, v.y, v.z, 1f);
    Vector3 FromHomogeneous(Vector4 h) => new Vector3(h.x, h.y, h.z);

    Vector4 MultiplyMatrixVectorRaw(float[,] M, Vector4 v)
    {
        float[] input = { v.x, v.y, v.z, v.w };
        float[] result = new float[4];
        for (int row = 0; row < 4; row++)
            for (int col = 0; col < 4; col++)
                result[row] += M[row, col] * input[col];
        return new Vector4(result[0], result[1], result[2], result[3]);
    }
}