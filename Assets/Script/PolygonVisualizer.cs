using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class PolygonVisualizer : MonoBehaviour
{
    public Material material; // ����� ����
    private MeshFilter meshFilter;

    private void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
    }

    public void CreatePolygonMesh(List<Vector3> vertices)
    {
        Mesh mesh = new Mesh();

        // �ﰢ���� �˰����� ����Ͽ� Ʈ���̾ޱ� �ε����� ����մϴ� (���⼭�� ����ȭ�߽��ϴ�)
        int[] triangles = Triangulate(vertices);

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles;

        mesh.RecalculateNormals(); // ����� �����Ͽ� �޽��� �ùٸ��� �������ǵ��� �մϴ�
        mesh.RecalculateBounds(); // �޽��� �ٿ�� ������ �����մϴ�

        meshFilter.mesh = mesh; // �޽� ���Ϳ� �޽� �Ҵ�
        GetComponent<MeshRenderer>().material = material; // ���� ����
    }

    private int[] Triangulate(List<Vector3> vertices)
    {
        // �� �κп��� ������ �ﰢ���� �˰����� �����ؾ� �մϴ�.
        // ���ø� ���� ������ ���� ��ȯ�մϴ�.
        return new int[] { 0, 1, 2, 0, 2, 3 };
    }
}
