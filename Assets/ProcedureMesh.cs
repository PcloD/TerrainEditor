using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ProcedureMesh : MonoBehaviour
{
    [SerializeField] private int _widthDivide = 1;
    [SerializeField] private int _heightDivide = 1;

    private MeshFilter _meshFilter;
    private MeshRenderer _meshRenderer;
    private Mesh _mesh;

    private void Awake()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _meshRenderer = GetComponent<MeshRenderer>();

        _mesh = new Mesh();
        _meshFilter.mesh = _mesh;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GenerateMesh();
        }
    }

    private void GenerateMesh()
    {
        Vector3[] vertices = new Vector3[(_widthDivide + 1) * (_heightDivide + 1)];
        Vector2[] uv = new Vector2[vertices.Length];
        for (int h = 0, i = 0; h <= _heightDivide; h++)
        {
            for (int w = 0; w <= _widthDivide; w++, i++)
            {
                vertices[i] = new Vector3(w, 0, h);
                uv[i] = new Vector2((float)w / _widthDivide, (float)h / _heightDivide);
            }
        }

        int[] triangles = new int[_widthDivide * _heightDivide * 6];
        for (int i = 0, mesh = 0; i < triangles.Length; i += 6, mesh++)
        {
            int index = mesh + mesh / _widthDivide;
            triangles[i] = index;
            triangles[i + 1] = index + _widthDivide + 1;
            triangles[i + 2] = index + 1;

            triangles[i + 3] = index + 1;
            triangles[i + 4] = index+ _widthDivide + 1;
            triangles[i + 5] = index + _widthDivide + 2;
        }

        _mesh.vertices = vertices;
        _mesh.triangles = triangles;
        _mesh.uv = uv;

        _mesh.RecalculateNormals();
    }
}
