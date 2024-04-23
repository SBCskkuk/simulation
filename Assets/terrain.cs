using UnityEngine;
using TMPro;
using UnityEngine.UI;


[RequireComponent(typeof(LineRenderer))]
public class TerrainDistanceVisualizer : MonoBehaviour
{
    public Button cancelButton; // 취th
    public Transform startPoint; // 시작점
    public Transform endPoint; // 끝점
    public TextMeshProUGUI statusText; // 상태 메시지를 표시할 TextMeshProUGUI
    public int sampleCount = 50; // 샘플링할 점의 수
    public LineRenderer lineRenderer; // LineRenderer 컴포넌트
    private bool isDrawing = false; // 그리기 활성화 상태
    private float totalDistance = 0f; // 총 거리

    private void Start()
    {
        if (lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }
        lineRenderer.positionCount = 0; // LineRenderer 초기화
        cancelButton.onClick.AddListener(ResetDrawing); //최소 
    }

    private void Update()
    {
        if (isDrawing)
        {
            if (Input.GetMouseButtonDown(0)) // 마우스 클릭 감지
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    // startPoint와 endPoint가 모두 설정되지 않았을 경우
                    if (startPoint == null || endPoint == null)
                    {
                        SetPoint(hit.point);
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape)) // ESC로 그리기 중단
            {
                isDrawing = false;
                ResetDrawing();
                statusText.text = "Drawing canceled.";
            }
        }
    }

    public void StartDrawing()
    {
        isDrawing = true;
        startPoint = null;
        endPoint = null;
        lineRenderer.positionCount = 0;
        statusText.text = "Drawing mode active. Click to start.";
    }

    private void SetPoint(Vector3 pointPosition)
    {
        // 터레인의 높이 데이터를 사용하여 포인트의 높이를 조정합니다.
        float terrainHeight = Terrain.activeTerrain.SampleHeight(pointPosition) + Terrain.activeTerrain.transform.position.y;
        pointPosition.y = terrainHeight;

        // 새로운 포인트를 생성하고 위치를 설정합니다.
        Transform newPoint = new GameObject("Point").transform;
        newPoint.position = pointPosition;

        // 시작점이 없다면, 새로운 포인트를 시작점으로 설정합니다.
        if (startPoint == null)
        {
            startPoint = newPoint;
        }
        // 그렇지 않고, 새로운 포인트가 추가될 때마다 LineRenderer에 선을 그리기 위한 점들을 추가합니다.
        else
        {
            // 새로운 점이 추가되면 총 거리를 업데이트하고 상태 텍스트를 갱신합니다.
            totalDistance += Vector3.Distance(startPoint.position, pointPosition);
            statusText.text = $"Total Distance: {totalDistance} meters";

            // 새로운 점을 다음 시작점으로 설정합니다.
            startPoint = newPoint;
        }

        // LineRenderer를 업데이트합니다.
        UpdateLineRenderer(newPoint.position);
    }

    private void UpdateLineRenderer(Vector3 newPointPosition)
    {
        // LineRenderer에 포인트를 추가합니다.
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, newPointPosition);
    }


    private void ResetDrawing()
    {
        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 0; // LineRenderer에서 그려진 선을 지웁니다.
        }

        if (startPoint != null) Destroy(startPoint.gameObject);
        if (endPoint != null) Destroy(endPoint.gameObject);
        startPoint = null;
        endPoint = null;
        totalDistance = 0f;
        statusText.text = "Measurement reset."; // 사용자에게 알림을 표시합니다.
        isDrawing = false; // 그리기 모드 비활성화
    }

    float CalculateAndDrawPath(Vector3 start, Vector3 end, int samples)
    {
        Vector3[] points = new Vector3[samples + 1];
        float totalDistance = 0f;
        Vector3 previousPoint = start;

        points[0] = start;

        for (int i = 1; i <= samples; i++)
        {
            Vector3 samplePoint = Vector3.Lerp(start, end, (float)i / samples);
            samplePoint.y = Terrain.activeTerrain.SampleHeight(samplePoint) + Terrain.activeTerrain.transform.position.y;
            totalDistance += Vector3.Distance(previousPoint, samplePoint);
            points[i] = samplePoint;
            previousPoint = samplePoint;
        }

        lineRenderer.positionCount = samples + 1;
        lineRenderer.SetPositions(points);

        return totalDistance;
    }
}