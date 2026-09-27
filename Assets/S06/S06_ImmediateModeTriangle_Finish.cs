using UnityEngine;

[ExecuteAlways]
public class S06_ImmediateModeTriangle_Finish : MonoBehaviour
{
    [SerializeField] private Material glMaterial;
    // 0~1 사이의 직관적인 화면 비율 좌표
    [SerializeField] private Vector3 vertexA = new Vector3(0.5f, 0.8f, 0f); // 중앙 위
    [SerializeField] private Vector3 vertexB = new Vector3(0.2f, 0.2f, 0f); // 왼쪽 아래
    [SerializeField] private Vector3 vertexC = new Vector3(0.8f, 0.2f, 0f); // 오른쪽 아래
    [SerializeField] private Color triangleColor = new Color(1f, 0.4f, 0.6f, 1f);

    private void OnRenderObject()
    {
        if (glMaterial == null)
        {
            glMaterial = new Material(Shader.Find("Hidden/Internal-Colored"));
        }

        glMaterial.SetPass(0);

        // --- [핵심 추가] GL 좌표계를 화면 전체(0~1) 기준 직교 투영으로 설정 ---
        GL.PushMatrix();
        GL.LoadOrtho(); 

        GL.Begin(GL.TRIANGLES);
        GL.Color(triangleColor);
        GL.Vertex3(vertexA.x, vertexA.y, vertexA.z);
        GL.Vertex3(vertexB.x, vertexB.y, vertexB.z);
        GL.Vertex3(vertexC.x, vertexC.y, vertexC.z);
        GL.End();

        GL.PopMatrix(); // --- 행렬 복원 ---
    }
}