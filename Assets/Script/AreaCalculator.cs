using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.EventSystems; // EventSystem ���

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class AreaCalculator : MonoBehaviour
{
    public Button calculateAreaButton; // ���� ��� ��ư
    public Button cancelButton; // ���� ���� ��� ��ư
    public TextMeshProUGUI areaText; // ������ ǥ���� TextMeshProUGUI
    private MeshFilter meshFilter;
    private List<Vector3> points = new List<Vector3>();
    private bool isMeasuring = false; // ���� ���� ��� �÷���
    private List<GameObject> visualPoints = new List<GameObject>();

    void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        calculateAreaButton.onClick.AddListener(ToggleMeasuringMode);
        cancelButton.onClick.AddListener(CancelMeasurement); // ��� ��ư ������ �߰�
    }

    void Update()
    {
        if (isMeasuring && Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject()) // UI �������� Ŭ���� ����
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    points.Add(hit.point);
                    // ������: ����Ʈ �߰� �� �ð�ȭ
                    GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    sphere.transform.position = hit.point;
                    sphere.transform.localScale = Vector3.one * 0.1f;
                }
                if (Physics.Raycast(ray, out hit))
                {
                    points.Add(hit.point);
                    GameObject sphere = CreateVisualPoint(hit.point);
                    visualPoints.Add(sphere); // ��ü�� ������ ����Ʈ�� �߰�
                }
            }
        }

    }
    GameObject CreateVisualPoint(Vector3 point)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = point;
        sphere.transform.localScale = Vector3.one * 0.1f;
        return sphere; // ������ ��ü�� ������ ��ȯ
    }
    void ToggleMeasuringMode()
    {
        isMeasuring = !isMeasuring; // ���� ���� ��� ���
        if (!isMeasuring) // ���� ��带 �����ϴ� ���
        {
            if (points.Count >= 3)
            {
                DisplayPolygon(points);
                float area = CalculateArea(points);
                areaText.text = $"Calculated Area: {area.ToString("F2")} m��";
            }
            points.Clear(); // �� ����Ʈ Ŭ����
        }
        else
        {
            areaText.text = "Start clicking to define the polygon...";
        }
    }
    void CancelMeasurement()
    {
        // �ð������� ǥ�õ� ��ü���� ����Ʈ�� ���� �ı�
        foreach (GameObject visualPoint in visualPoints)
        {
            Destroy(visualPoint);
        }
        visualPoints.Clear(); // �ð��� ����Ʈ ����Ʈ�� Ŭ����
        points.Clear(); // �� ����Ʈ Ŭ����
        meshFilter.mesh.Clear(); // �޽� Ŭ����
        areaText.text = "Area measurement canceled."; // �ؽ�Ʈ ������Ʈ
        isMeasuring = false; // ���� ��� ��Ȱ��ȭ
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
        // �̰��� �� ������ �ﰢ���� ������ �����ϰų� �ܺ� ���̺귯���� ����� �� �ֽ��ϴ�.
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
        mesh.triangles = Triangulate(vertices); // �ٰ��� �ﰢ����
        mesh.RecalculateNormals(); // ��� ����
        mesh.RecalculateBounds(); // ��� ����
        meshFilter.mesh = mesh; // ������ �޽��� �޽� ���Ϳ� �Ҵ�
    }
}
