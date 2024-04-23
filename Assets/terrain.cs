using UnityEngine;
using TMPro;
using UnityEngine.UI;


[RequireComponent(typeof(LineRenderer))]
public class TerrainDistanceVisualizer : MonoBehaviour
{
    public Button cancelButton; // ��th
    public Transform startPoint; // ������
    public Transform endPoint; // ����
    public TextMeshProUGUI statusText; // ���� �޽����� ǥ���� TextMeshProUGUI
    public int sampleCount = 50; // ���ø��� ���� ��
    public LineRenderer lineRenderer; // LineRenderer ������Ʈ
    private bool isDrawing = false; // �׸��� Ȱ��ȭ ����
    private float totalDistance = 0f; // �� �Ÿ�

    private void Start()
    {
        if (lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }
        lineRenderer.positionCount = 0; // LineRenderer �ʱ�ȭ
        cancelButton.onClick.AddListener(ResetDrawing); //�ּ� 
    }

    private void Update()
    {
        if (isDrawing)
        {
            if (Input.GetMouseButtonDown(0)) // ���콺 Ŭ�� ����
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    // startPoint�� endPoint�� ��� �������� �ʾ��� ���
                    if (startPoint == null || endPoint == null)
                    {
                        SetPoint(hit.point);
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape)) // ESC�� �׸��� �ߴ�
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
        // �ͷ����� ���� �����͸� ����Ͽ� ����Ʈ�� ���̸� �����մϴ�.
        float terrainHeight = Terrain.activeTerrain.SampleHeight(pointPosition) + Terrain.activeTerrain.transform.position.y;
        pointPosition.y = terrainHeight;

        // ���ο� ����Ʈ�� �����ϰ� ��ġ�� �����մϴ�.
        Transform newPoint = new GameObject("Point").transform;
        newPoint.position = pointPosition;

        // �������� ���ٸ�, ���ο� ����Ʈ�� ���������� �����մϴ�.
        if (startPoint == null)
        {
            startPoint = newPoint;
        }
        // �׷��� �ʰ�, ���ο� ����Ʈ�� �߰��� ������ LineRenderer�� ���� �׸��� ���� ������ �߰��մϴ�.
        else
        {
            // ���ο� ���� �߰��Ǹ� �� �Ÿ��� ������Ʈ�ϰ� ���� �ؽ�Ʈ�� �����մϴ�.
            totalDistance += Vector3.Distance(startPoint.position, pointPosition);
            statusText.text = $"Total Distance: {totalDistance} meters";

            // ���ο� ���� ���� ���������� �����մϴ�.
            startPoint = newPoint;
        }

        // LineRenderer�� ������Ʈ�մϴ�.
        UpdateLineRenderer(newPoint.position);
    }

    private void UpdateLineRenderer(Vector3 newPointPosition)
    {
        // LineRenderer�� ����Ʈ�� �߰��մϴ�.
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, newPointPosition);
    }


    private void ResetDrawing()
    {
        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 0; // LineRenderer���� �׷��� ���� ����ϴ�.
        }

        if (startPoint != null) Destroy(startPoint.gameObject);
        if (endPoint != null) Destroy(endPoint.gameObject);
        startPoint = null;
        endPoint = null;
        totalDistance = 0f;
        statusText.text = "Measurement reset."; // ����ڿ��� �˸��� ǥ���մϴ�.
        isDrawing = false; // �׸��� ��� ��Ȱ��ȭ
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