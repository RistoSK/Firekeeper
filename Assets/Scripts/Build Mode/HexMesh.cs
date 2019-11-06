using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class HexMesh : MonoBehaviour
{
    private Mesh _hexMesh;

    [NonSerialized] private List<Vector3> _vertices;
    [NonSerialized] private List<Color> _colors;
    [NonSerialized] private List<int> _triangles;

    private MeshCollider _meshCollider;

    private void Awake()
    {
        _hexMesh = new Mesh();
        GetComponent<MeshFilter>().mesh = _hexMesh;
        _meshCollider = gameObject.AddComponent<MeshCollider>();

        _hexMesh.name = "Hex Mesh";
    }

    public void Clear()
    {
        _hexMesh.Clear();
        _vertices = ListPool<Vector3>.Get();

        _colors = ListPool<Color>.Get();
        
        _triangles = ListPool<int>.Get();
    }

    public void Apply()
    {
        _hexMesh.SetVertices(_vertices);
        ListPool<Vector3>.Add(_vertices);

        _hexMesh.SetColors(_colors);
        ListPool<Color>.Add(_colors);
        
        _hexMesh.SetTriangles(_triangles, 0);
        ListPool<int>.Add(_triangles);

        _hexMesh.RecalculateNormals();

        _meshCollider.sharedMesh = _hexMesh; 
    }

    public void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        int vertexIndex = _vertices.Count;

        _vertices.Add(v1);
        _vertices.Add(v2);
        _vertices.Add(v3);

        _triangles.Add(vertexIndex);
        _triangles.Add(vertexIndex + 1);
        _triangles.Add(vertexIndex + 2);  
    }

    //TODO see if used
    public void AddTriangleColor(Color color)
    {
        _colors.Add(color);
        _colors.Add(color);
        _colors.Add(color);
    }

    //TODO see if used
    public void AddTriangleColor(Color c1, Color c2, Color c3)
    {
        _colors.Add(c1);
        _colors.Add(c2);
        _colors.Add(c3);
    }

    public void AddQuad(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4)
    {
        int vertexIndex = _vertices.Count;

        _vertices.Add(v1);
        _vertices.Add(v2);
        _vertices.Add(v3);
        _vertices.Add(v4);

        _triangles.Add(vertexIndex);
        _triangles.Add(vertexIndex + 2);
        _triangles.Add(vertexIndex + 1);
        _triangles.Add(vertexIndex + 1);
        _triangles.Add(vertexIndex + 2);
        _triangles.Add(vertexIndex + 3);
    }

    public void AddQuadColor(Color c1, Color c2)
    {
        _colors.Add(c1);
        _colors.Add(c1);
        _colors.Add(c2);
        _colors.Add(c2);
    }
}
