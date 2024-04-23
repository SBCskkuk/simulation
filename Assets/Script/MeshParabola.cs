using UnityEngine;
using System.Collections.Generic;

public class MeshParabola : MonoBehaviour
{
    [Header("Component")]
    [Tooltip("시작 위치")]
    public Transform from;
    [Tooltip("종료 위치")]
    public Transform to;

    [Header("Draw")]
    [Tooltip("면의 개수. 클수록 부드러운 선이 된다.")]
    [Min(1)] public int face = 20;
    [Tooltip("선의 높이")]
    public float height = 0.15f;
    [Tooltip("거리에 따라 높이를 조정한다.")]
    public bool heightByDistance = true;
    [Tooltip("선의 두께")]
    public float thickness = 0.05f;
    [Tooltip("Off : 양면 렌더링, Front : 뒷면 렌더링, Rear : 앞면 렌더링, Both : 렌더링 안함.")]
    public Cull cull = Cull.Off;
    [Tooltip("사용할 쉐이더")]
    public string shader = "Unlit/Transparent";

    [Header("Texture")]
    [Tooltip("머티리얼에 적용할 텍스쳐")]
    public Texture texture;
    [Tooltip("텍스쳐 반복 횟수")]
    public float tiling = 1f;
    [Tooltip("흐르는 속도")]
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
        Off = 0, //Cull 하지 않는 상태. 양면 렌더링.
        Front = 1, //Front를 Cull하는 상태. 뒷면 렌더링.
        Back = 2, //Back을 Cull하는 상태. 앞면 렌더링.
        Both = 3 //모두 Cull하는 상태. 렌더링 안함.
    }

    //__________________________________________________________________________ Initialize
    private void Awake()
    {
        initializeMesh();
        initializeComponent();
    }
    private void initializeMesh()
    {
        _mesh = new Mesh(); //새로운 Mesh 생성

        _vertices = new List<Vector3>(); //정점 데이터를 저장할 List
        _uvs = new List<Vector2>(); //UV 데이터를 저장할 List
        _triangles = new List<int>(); //Triangle 데이터를 저장할 List
    }
    [ContextMenu("Initialize Component")]
    private void initializeComponent()
    {
        _meshRenderer = GetComponentInChildren<MeshRenderer>(true); //MeshRenderer 찾아보고,
        if (_meshRenderer == null) _meshRenderer = gameObject.AddComponent<MeshRenderer>(); //없으면 추가
        _meshRenderer.material = new Material(Shader.Find(shader)); //쉐이더 적용한 머티리얼 생성하고,
        _meshRenderer.material.mainTexture = texture; //텍스쳐도 지정

        _meshFilter = GetComponentInChildren<MeshFilter>(true); //MeshFilter 찾아보고,
        if (_meshFilter == null) _meshFilter = gameObject.AddComponent<MeshFilter>(); //없으면 추가
        _meshFilter.mesh = _mesh; //위에서 생성한 Mesh 지정
    }

    //__________________________________________________________________________ Draw
    private void Update()
    {
        draw();
    }
    private void draw()
    {
        if (from != null && to != null) //From과 To 모두 있어야한다.
        {
            float division = 1f / face; //각 면의 t(보간비율)를 구하기 위한 값
            float currV = 0f; //현재까지의 V값. UV중 V값이다.
            float distance = Vector3.Distance(from.position, to.position); //From과 To의 거리

            for (int i = 0; i < face; ++i) //면의 개수만큼 반복
            {
                float startT = division * i; //시작 t
                float endT = startT + division; //끝 t
                Vector3 adjFromPos = from.position - transform.position; //현재 오브젝트가 원점이 아닐 수 있으므로 조정
                Vector3 adjToPos = to.position - transform.position; //현재 오브젝트가 원점이 아닐 수 있으므로 조정

                GetPoints(adjFromPos, adjToPos, height, startT, thickness, out Vector3 startL, out Vector3 startR); //시작 지점의 양쪽 포인트를 구한다.
                GetPoints(adjFromPos, adjToPos, height, endT, thickness, out Vector3 endL, out Vector3 endR); //끝 지점의 양쪽 포인트를 구한다.

                float startV = currV + _flow; //시작 지점의 V. Flow 값을 더해준다.
                float endV = startV + distance * tiling / face / thickness; //끝 지점의 V. V값에 영향을 주는 요소들을 계산한다.

                stackMeshSquare(
                    new Vector3[] { startL, endL, endR, startR },
                    new Vector2[] { new Vector2(0f, startV), new Vector2(0f, endV), new Vector2(1f, endV), new Vector2(1f, startV) }); //면(사각형) 하나를 쌓는다.
                currV = endV - _flow; //다음 V값을 넘겨준다.
            }
        }

        calculateFlow();
        applyMesh();
    }
    private void calculateFlow()
    {
        _flow -= flowSpeed * Time.deltaTime; //Flow 속도를 적용한다.
        _flow = _flow > 1f ? _flow % 1f : _flow < 0f ? 1f - _flow + Mathf.Ceil(_flow) : _flow; //항상 0과 1사이의 값을 가지도록 한다.
    }

    //__________________________________________________________________________ Mesh
    private void stackMeshSquare(Vector3[] vertices, Vector2[] uvs)
    {
        int vertLen = _vertices.Count; //이전 정점의 개수를 가져온다.
        if (Cull.Front != (cull & Cull.Front)) //비트 연산으로 Front가 포함되지 않으면 시계방향으로 사각형을 쌓는다.
        {
            _triangles.Add(vertLen);
            _triangles.Add(vertLen + 1);
            _triangles.Add(vertLen + 2);

            _triangles.Add(vertLen + 2);
            _triangles.Add(vertLen + 3);
            _triangles.Add(vertLen);
        }
        if (Cull.Back != (cull & Cull.Back)) //비트 연산으로 Back이 포함되지 않으면 반시계방향으로 사각형을 쌓는다.
        {
            _triangles.Add(vertLen);
            _triangles.Add(vertLen + 3);
            _triangles.Add(vertLen + 2);

            _triangles.Add(vertLen + 2);
            _triangles.Add(vertLen + 1);
            _triangles.Add(vertLen);
        }

        for (int i = 0, l = vertices.Length; i < l; ++i) //정점을 추가한다.
            _vertices.Add(vertices[i]);

        for (int i = 0, l = uvs.Length; i < l; ++i) //UV를 추가한다.
            _uvs.Add(uvs[i]);
    }
    private void applyMesh()
    {
        if (_mesh == null) return;

        // 지금까지 쌓은 정점, UV, 사각형 데이터를 적용한다.
        if (_mesh.vertices.Length > _vertices.Count) //새로운 정점의 개수가 더 적으면
        {
            _mesh.triangles = _triangles.ToArray(); //Triangle 먼저 적용한다.
            _mesh.vertices = _vertices.ToArray();
            _mesh.uv = _uvs.ToArray();
        }
        else //새로운 정점의 개수가 더 많거나 같으면
        {
            _mesh.vertices = _vertices.ToArray(); //정점 & UV 먼저 적용한다.
            _mesh.uv = _uvs.ToArray();
            _mesh.triangles = _triangles.ToArray();
        }
        _mesh.RecalculateNormals(); //노멀 재계산
        _mesh.bounds = new Bounds(Vector3.zero, Vector3.one * float.MaxValue); //Bound를 최대로 지정하여 카메라에 컬링되지 않도록 한다.

        _vertices.Clear(); //정점 데이터 초기화
        _uvs.Clear(); //UV 데이터 초기화
        _triangles.Clear(); //삼각형 데이터 초기화
    }

    //__________________________________________________________________________ Formula
    public void GetPoints(Vector3 from, Vector3 to, float height, float t, float thickness, out Vector3 left, out Vector3 right) //포물선 공식으로 양쪽 포인트를 구하는 함수
    {
        Vector3 mid = GetPoint(from, to, height, t); //포물선 위치 저장
        Vector3 dir = Vector3.Scale(to - from, new Vector3(1f, 0f, 1f)); //From에서 To로 가는 방향벡터를 높이를 무시하고 구한다.
        Quaternion lookRot = Quaternion.LookRotation(dir); //방향 벡터를 바라보는 각도 저장
        left = mid + lookRot * Vector3.left * thickness * 0.5f; //왼쪽 포인트 반환
        right = mid + lookRot * Vector3.right * thickness * 0.5f; //오른쪽 포인트 반환
    }
    public Vector3 GetPoint(Vector3 from, Vector3 to, float height, float t) //포물선 공식 함수
    {
        Vector3 midPos = Vector3.Lerp(from, to, t); //중간 위치
        if (heightByDistance) height *= Vector3.Distance(from, to); //거리에 따라 높이를 조절
        float form = -4 * height * t * t + 4 * height * t; //포물선 공식
        return new Vector3(midPos.x, Mathf.Lerp(from.y, to.y, t) + form, midPos.z); //결과 위치 반환
    }
}