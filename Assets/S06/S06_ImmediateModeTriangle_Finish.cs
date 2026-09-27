using UnityEngine;

[ExecuteAlways]
public class S06_ImmediateModeTriangle_Finish : MonoBehaviour
{
    [SerializeField] private Material glMaterial;
    [SerializeField] private Vector3 vertexA = new Vector3(0.5f, 0.8f, 0f);
    [SerializeField] private Vector3 vertexB = new Vector3(0.2f, 0.2f, 0f);
    [SerializeField] private Vector3 vertexC = new Vector3(0.8f, 0.2f, 0f);
    [SerializeField] private Color triangleColor = new Color(1f, 0.4f, 0.6f, 1f);

    private void OnRenderObject()
    {
        if (glMaterial == null)
        {
            // Material이 할당되지 않은 경우 기본 UI 머티리얼 사용
            glMaterial = new Material(Shader.Find("Hidden/Internal-Colored"));
        }

        glMaterial.SetPass(0);
        GL.Begin(GL.TRIANGLES);
        GL.Color(triangleColor);
        GL.Vertex3(vertexA.x, vertexA.y, vertexA.z);
        GL.Vertex3(vertexB.x, vertexB.y, vertexB.z);
        GL.Vertex3(vertexC.x, vertexC.y, vertexC.z);
        GL.End();
    }
}