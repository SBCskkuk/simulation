using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class PolygonVisualizer : MonoBehaviour
{
    public Material material; // 사용할 재질
    private MeshFilter meshFilter;

    private void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
    }

    public void CreatePolygonMesh(List<Vector3> vertices)
    {
        Mesh mesh = new Mesh();

        // 삼각분할 알고리즘을 사용하여 트라이앵글 인덱스를 계산합니다 (여기서는 간략화했습니다)
        int[] triangles = Triangulate(vertices);

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles;

        mesh.RecalculateNormals(); // 노멀을 재계산하여 메쉬가 올바르게 렌더링되도록 합니다
        mesh.RecalculateBounds(); // 메쉬의 바운딩 볼륨을 재계산합니다

        meshFilter.mesh = mesh; // 메쉬 필터에 메쉬 할당
        GetComponent<MeshRenderer>().material = material; // 재질 설정
    }

    private int[] Triangulate(List<Vector3> vertices)
    {
        // 이 부분에서 적절한 삼각분할 알고리즘을 구현해야 합니다.
        // 예시를 위해 임의의 값을 반환합니다.
        return new int[] { 0, 1, 2, 0, 2, 3 };
    }
}
