using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.EventSystems; // EventSystem 사용

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class AreaCalculator : MonoBehaviour
{
    public Button calculateAreaButton; // 면적 계산 버튼
    public Button cancelButton; // 면적 측정 취소 버튼
    public TextMeshProUGUI areaText; // 면적을 표시할 TextMeshProUGUI
    private MeshFilter meshFilter;
    private List<Vector3> points = new List<Vector3>();
    private bool isMeasuring = false; // 면적 측정 모드 플래그
    private List<GameObject> visualPoints = new List<GameObject>();

    void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        calculateAreaButton.onClick.AddListener(ToggleMeasuringMode);
        cancelButton.onClick.AddListener(CancelMeasurement); // 취소 버튼 리스너 추가
    }

    void Update()
    {
        if (isMeasuring && Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject()) // UI 위에서는 클릭을 무시
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    points.Add(hit.point);
                    // 선택적: 포인트 추가 시 시각화
                    GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    sphere.transform.position = hit.point;
                    sphere.transform.localScale = Vector3.one * 0.1f;
                }
                if (Physics.Raycast(ray, out hit))
                {
                    points.Add(hit.point);
                    GameObject sphere = CreateVisualPoint(hit.point);
                    visualPoints.Add(sphere); // 구체의 참조를 리스트에 추가
                }
            }
        }

    }
    GameObject CreateVisualPoint(Vector3 point)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = point;
        sphere.transform.localScale = Vector3.one * 0.1f;
        return sphere; // 생성된 구체의 참조를 반환
    }
    void ToggleMeasuringMode()
    {
        isMeasuring = !isMeasuring; // 면적 측정 모드 토글
        if (!isMeasuring) // 측정 모드를 종료하는 경우
        {
            if (points.Count >= 3)
            {
                DisplayPolygon(points);
                float area = CalculateArea(points);
                areaText.text = $"Calculated Area: {area.ToString("F2")} m²";
            }
            points.Clear(); // 점 리스트 클리어
        }
        else
        {
            areaText.text = "Start clicking to define the polygon...";
        }
    }
    void CancelMeasurement()
    {
        // 시각적으로 표시된 구체들을 리스트를 통해 파괴
        foreach (GameObject visualPoint in visualPoints)
        {
            Destroy(visualPoint);
        }
        visualPoints.Clear(); // 시각적 포인트 리스트를 클리어
        points.Clear(); // 점 리스트 클리어
        meshFilter.mesh.Clear(); // 메쉬 클리어
        areaText.text = "Area measurement canceled."; // 텍스트 업데이트
        isMeasuring = false; // 측정 모드 비활성화
    }
    float CalculateArea(List<Vector3> vertices)
    {
        float area = 0f;
        int[] triangles = Triangulate(vertices);
        for (int i = 0; i < triangles.Length; i += 3)
        {
            Vector3 a = vertices[triangles[i]];
            Vector3 b = vertices[triangles[i + 1]];
            Vector3 c = vertices[triangles[i + 2]];
            area += Vector3.Cross(b - a, c - a).magnitude * 0.5f;
        }
        return area;
    }

    int[] Triangulate(List<Vector3> vertices)
    {
        // 이곳에 더 정교한 삼각분할 로직을 구현하거나 외부 라이브러리를 사용할 수 있습니다.
        List<int> tris = new List<int>();
        for (int i = 1; i < vertices.Count - 1; i++)
        {
            tris.Add(0);
            tris.Add(i);
            tris.Add(i + 1);
        }
        return tris.ToArray();
    }

    void DisplayPolygon(List<Vector3> vertices)
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = Triangulate(vertices); // 다각형 삼각분할
        mesh.RecalculateNormals(); // 노멀 재계산
        mesh.RecalculateBounds(); // 경계 재계산
        meshFilter.mesh = mesh; // 생성된 메쉬를 메쉬 필터에 할당
    }
}
