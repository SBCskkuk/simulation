using UnityEngine;
using System.Collections.Generic;

public class MeshParabola : MonoBehaviour
{
    [Header("Component")]
    [Tooltip("���� ��ġ")]
    public Transform from;
    [Tooltip("���� ��ġ")]
    public Transform to;

    [Header("Draw")]
    [Tooltip("���� ����. Ŭ���� �ε巯�� ���� �ȴ�.")]
    [Min(1)] public int face = 20;
    [Tooltip("���� ����")]
    public float height = 0.15f;
    [Tooltip("�Ÿ��� ���� ���̸� �����Ѵ�.")]
    public bool heightByDistance = true;
    [Tooltip("���� �β�")]
    public float thickness = 0.05f;
    [Tooltip("Off : ��� ������, Front : �޸� ������, Rear : �ո� ������, Both : ������ ����.")]
    public Cull cull = Cull.Off;
    [Tooltip("����� ���̴�")]
    public string shader = "Unlit/Transparent";

    [Header("Texture")]
    [Tooltip("��Ƽ���� ������ �ؽ���")]
    public Texture texture;
    [Tooltip("�ؽ��� �ݺ� Ƚ��")]
    public float tiling = 1f;
    [Tooltip("�帣�� �ӵ�")]
    public float flowSpeed = 4f;

    // Component
    private MeshRenderer _meshRenderer;
    private MeshFilter _meshFilter;

    // Mesh
    private Mesh _mesh;
    private List<Vector3> _vertices;
    private List<Vector2> _uvs;
    private List<int> _triangles;

    // Texture
    private float _flow;

    public enum Cull
    {
        Off = 0, //Cull ���� �ʴ� ����. ��� ������.
        Front = 1, //Front�� Cull�ϴ� ����. �޸� ������.
        Back = 2, //Back�� Cull�ϴ� ����. �ո� ������.
        Both = 3 //��� Cull�ϴ� ����. ������ ����.
    }

    //__________________________________________________________________________ Initialize
    private void Awake()
    {
        initializeMesh();
        initializeComponent();
    }
    private void initializeMesh()
    {
        _mesh = new Mesh(); //���ο� Mesh ����

        _vertices = new List<Vector3>(); //���� �����͸� ������ List
        _uvs = new List<Vector2>(); //UV �����͸� ������ List
        _triangles = new List<int>(); //Triangle �����͸� ������ List
    }
    [ContextMenu("Initialize Component")]
    private void initializeComponent()
    {
        _meshRenderer = GetComponentInChildren<MeshRenderer>(true); //MeshRenderer ã�ƺ���,
        if (_meshRenderer == null) _meshRenderer = gameObject.AddComponent<MeshRenderer>(); //������ �߰�
        _meshRenderer.material = new Material(Shader.Find(shader)); //���̴� ������ ��Ƽ���� �����ϰ�,
        _meshRenderer.material.mainTexture = texture; //�ؽ��ĵ� ����

        _meshFilter = GetComponentInChildren<MeshFilter>(true); //MeshFilter ã�ƺ���,
        if (_meshFilter == null) _meshFilter = gameObject.AddComponent<MeshFilter>(); //������ �߰�
        _meshFilter.mesh = _mesh; //������ ������ Mesh ����
    }

    //__________________________________________________________________________ Draw
    private void Update()
    {
        draw();
    }
    private void draw()
    {
        if (from != null && to != null) //From�� To ��� �־���Ѵ�.
        {
            float division = 1f / face; //�� ���� t(��������)�� ���ϱ� ���� ��
            float currV = 0f; //��������� V��. UV�� V���̴�.
            float distance = Vector3.Distance(from.position, to.position); //From�� To�� �Ÿ�

            for (int i = 0; i < face; ++i) //���� ������ŭ �ݺ�
            {
                float startT = division * i; //���� t
                float endT = startT + division; //�� t
                Vector3 adjFromPos = from.position - transform.position; //���� ������Ʈ�� ������ �ƴ� �� �����Ƿ� ����
                Vector3 adjToPos = to.position - transform.position; //���� ������Ʈ�� ������ �ƴ� �� �����Ƿ� ����

                GetPoints(adjFromPos, adjToPos, height, startT, thickness, out Vector3 startL, out Vector3 startR); //���� ������ ���� ����Ʈ�� ���Ѵ�.
                GetPoints(adjFromPos, adjToPos, height, endT, thickness, out Vector3 endL, out Vector3 endR); //�� ������ ���� ����Ʈ�� ���Ѵ�.

                float startV = currV + _flow; //���� ������ V. Flow ���� �����ش�.
                float endV = startV + distance * tiling / face / thickness; //�� ������ V. V���� ������ �ִ� ��ҵ��� ����Ѵ�.

                stackMeshSquare(
                    new Vector3[] { startL, endL, endR, startR },
                    new Vector2[] { new Vector2(0f, startV), new Vector2(0f, endV), new Vector2(1f, endV), new Vector2(1f, startV) }); //��(�簢��) �ϳ��� �״´�.
                currV = endV - _flow; //���� V���� �Ѱ��ش�.
            }
        }

        calculateFlow();
        applyMesh();
    }
    private void calculateFlow()
    {
        _flow -= flowSpeed * Time.deltaTime; //Flow �ӵ��� �����Ѵ�.
        _flow = _flow > 1f ? _flow % 1f : _flow < 0f ? 1f - _flow + Mathf.Ceil(_flow) : _flow; //�׻� 0�� 1������ ���� �������� �Ѵ�.
    }

    //__________________________________________________________________________ Mesh
    private void stackMeshSquare(Vector3[] vertices, Vector2[] uvs)
    {
        int vertLen = _vertices.Count; //���� ������ ������ �����´�.
        if (Cull.Front != (cull & Cull.Front)) //��Ʈ �������� Front�� ���Ե��� ������ �ð�������� �簢���� �״´�.
        {
            _triangles.Add(vertLen);
            _triangles.Add(vertLen + 1);
            _triangles.Add(vertLen + 2);

            _triangles.Add(vertLen + 2);
            _triangles.Add(vertLen + 3);
            _triangles.Add(vertLen);
        }
        if (Cull.Back != (cull & Cull.Back)) //��Ʈ �������� Back�� ���Ե��� ������ �ݽð�������� �簢���� �״´�.
        {
            _triangles.Add(vertLen);
            _triangles.Add(vertLen + 3);
            _triangles.Add(vertLen + 2);

            _triangles.Add(vertLen + 2);
            _triangles.Add(vertLen + 1);
            _triangles.Add(vertLen);
        }

        for (int i = 0, l = vertices.Length; i < l; ++i) //������ �߰��Ѵ�.
            _vertices.Add(vertices[i]);

        for (int i = 0, l = uvs.Length; i < l; ++i) //UV�� �߰��Ѵ�.
            _uvs.Add(uvs[i]);
    }
    private void applyMesh()
    {
        if (_mesh == null) return;

        // ���ݱ��� ���� ����, UV, �簢�� �����͸� �����Ѵ�.
        if (_mesh.vertices.Length > _vertices.Count) //���ο� ������ ������ �� ������
        {
            _mesh.triangles = _triangles.ToArray(); //Triangle ���� �����Ѵ�.
            _mesh.vertices = _vertices.ToArray();
            _mesh.uv = _uvs.ToArray();
        }
        else //���ο� ������ ������ �� ���ų� ������
        {
            _mesh.vertices = _vertices.ToArray(); //���� & UV ���� �����Ѵ�.
            _mesh.uv = _uvs.ToArray();
            _mesh.triangles = _triangles.ToArray();
        }
        _mesh.RecalculateNormals(); //��� ����
        _mesh.bounds = new Bounds(Vector3.zero, Vector3.one * float.MaxValue); //Bound�� �ִ�� �����Ͽ� ī�޶� �ø����� �ʵ��� �Ѵ�.

        _vertices.Clear(); //���� ������ �ʱ�ȭ
        _uvs.Clear(); //UV ������ �ʱ�ȭ
        _triangles.Clear(); //�ﰢ�� ������ �ʱ�ȭ
    }

    //__________________________________________________________________________ Formula
    public void GetPoints(Vector3 from, Vector3 to, float height, float t, float thickness, out Vector3 left, out Vector3 right) //������ �������� ���� ����Ʈ�� ���ϴ� �Լ�
    {
        Vector3 mid = GetPoint(from, to, height, t); //������ ��ġ ����
        Vector3 dir = Vector3.Scale(to - from, new Vector3(1f, 0f, 1f)); //From���� To�� ���� ���⺤�͸� ���̸� �����ϰ� ���Ѵ�.
        Quaternion lookRot = Quaternion.LookRotation(dir); //���� ���͸� �ٶ󺸴� ���� ����
        left = mid + lookRot * Vector3.left * thickness * 0.5f; //���� ����Ʈ ��ȯ
        right = mid + lookRot * Vector3.right * thickness * 0.5f; //������ ����Ʈ ��ȯ
    }
    public Vector3 GetPoint(Vector3 from, Vector3 to, float height, float t) //������ ���� �Լ�
    {
        Vector3 midPos = Vector3.Lerp(from, to, t); //�߰� ��ġ
        if (heightByDistance) height *= Vector3.Distance(from, to); //�Ÿ��� ���� ���̸� ����
        float form = -4 * height * t * t + 4 * height * t; //������ ����
        return new Vector3(midPos.x, Mathf.Lerp(from.y, to.y, t) + form, midPos.z); //��� ��ġ ��ȯ
    }
}