using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LineRender : MonoBehaviour
{
    public TextMeshProUGUI statusText; // 상태 메시지를 표시할 TextMeshPro 텍스트
    public LineRenderer lineRenderer;
    private List<Vector3> points = new List<Vector3>();
    private bool isDrawing = false;  // 그리기 활성화 상태

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            Debug.LogError("LineRenderer component not found on this GameObject");
        }
    }

    void Update()
    {
        if (isDrawing)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    AddPoint(hit.point);
                    if (points.Count >= 2)
                    {
                        statusText.text = "Stop drawing? Press ESC to cancel.";
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                isDrawing = false;
                points.Clear();
                lineRenderer.positionCount = 0;
                statusText.text = "Drawing canceled.";
            }
        }
        else
        {
            if (statusText.text != "" && Input.GetMouseButtonDown(0))
            {
                statusText.text = "";  // 상태 메시지 지우기
            }
        }
    }

    public void StartDrawing()
    {
        isDrawing = true;
        statusText.text = "Drawing mode active. Click to draw.";
    }

    void AddPoint(Vector3 newPoint)
    {
        points.Add(newPoint);
        if (points.Count > 3)
        {
            var splinePoints = CalculateCatmullRomSpline(10); // 10은 곡선의 해상도입니다.
            lineRenderer.positionCount = splinePoints.Length;
            lineRenderer.SetPositions(splinePoints);
        }
        else
        {
            lineRenderer.positionCount = points.Count;
            lineRenderer.SetPositions(points.ToArray());
        }
    }

    Vector3[] CalculateCatmullRomSpline(int resolution)
    {
        List<Vector3> splinePoints = new List<Vector3>();
        for (int i = 0; i < points.Count - 3; i++)
        {
            for (int j = 0; j <= resolution; j++)
            {
                float t = j / (float)resolution;
                Vector3 p0 = points[i];
                Vector3 p1 = points[i + 1];
                Vector3 p2 = points[i + 2];
                Vector3 p3 = points[i + 3];

                Vector3 position = 0.5f * ((2 * p1) + (-p0 + p2) * t + (2 * p0 - 5 * p1 + 4 * p2 - p3) * t * t + (-p0 + 3 * p1 - 3 * p2 + p3) * t * t * t);
                splinePoints.Add(position);
            }
        }
        return splinePoints.ToArray();
    }

}