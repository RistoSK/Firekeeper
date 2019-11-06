using UnityEngine;

public class HexGridChunk : MonoBehaviour
{
    [SerializeField] private HexMesh _walls;

    private HexCell[] _cells;
    private HexMesh _terrain;
    private MeshCollider _meshCollider;
    private Canvas _gridCanvas;

    private void Awake()
    {
        _gridCanvas = GetComponentInChildren<Canvas>();
        _terrain = GetComponentInChildren<HexMesh>();
        _meshCollider = gameObject.AddComponent<MeshCollider>();

        _cells = new HexCell[HexMetrics.ChunkSizeX * HexMetrics.ChunkSizeZ];
    }

    private void LateUpdate()
    {
        Triangulate();
        enabled = false;
    }

    public void Triangulate()
    {
        _terrain.Clear();
        _walls.Clear();

        for (int i = 0; i < _cells.Length; i++)
        {
            Triangulate(_cells[i]);
        }
        _terrain.Apply();
        _walls.Apply();
    }

    private void Triangulate(HexCell cell)
    {
        for (HexDirection dir = HexDirection.NorthEast; dir <= HexDirection.NorthWest; dir++)
        {
            Triangulate(dir, cell);
        }
    }

    private void Triangulate(HexDirection direction, HexCell cell)
    {
        Vector3 center = cell.transform.localPosition;
        EdgeVertices edgeVertices = new EdgeVertices(center + HexMetrics.GetFirstSolidCorner(direction), center + HexMetrics.GetSecondSolidCorner(direction));

        TriangulateEdgeFan(center, edgeVertices, cell.Color);
        //TriangulateEdgeFan(center, edgeVertices);
        
        if (direction <= HexDirection.SouthEast)
        {
            TriangulateConnection(direction, cell, edgeVertices);
        }
    }

    private void TriangulateConnection(HexDirection direction, HexCell cell, EdgeVertices e1)
    {
        HexCell neighbor = cell.GetNeighbor(direction);

        // TODO: check if removing the null check breaks the logic, neighbor == null
        if (!neighbor)
        {
            return;
        }

        Vector3 bridge = HexMetrics.GetBridge(direction);

        EdgeVertices e2 = new EdgeVertices(
            e1.v1 + bridge,
            e1.v5 + bridge
        );

        TriangulateEdgeStrip(e1, cell.Color, e2, neighbor.Color, cell.HasWallThroughEdge(direction));
        //TriangulateEdgeStrip(e1, e2, cell.HasWallThroughEdge(direction));
        
        HexCell nextNeighbor = cell.GetNeighbor(direction.Next());

        // TODO: check if removing the null check breaks the logic, nextNeighbor == null
        if (direction > HexDirection.East || !nextNeighbor)
        {
            return;
        }
        
        Vector3 v5 = e1.v5 + HexMetrics.GetBridge(direction.Next());

        TriangulateCorner(v5, nextNeighbor, e1.v5, cell, e2.v5, neighbor);
    }

    private void TriangulateCorner(Vector3 bottom, HexCell bottomCell, Vector3 left, HexCell leftCell, Vector3 right, HexCell rightCell)
    {
        _terrain.AddTriangle(bottom, left, right);
        _terrain.AddTriangleColor(bottomCell.Color, leftCell.Color, rightCell.Color);
    }

    private void TriangulateEdgeFan(Vector3 center, EdgeVertices edge, Color color)
    {
        _terrain.AddTriangle(center, edge.v1, edge.v2);
        _terrain.AddTriangleColor(color);
        _terrain.AddTriangle(center, edge.v2, edge.v3);
        _terrain.AddTriangleColor(color);
        _terrain.AddTriangle(center, edge.v3, edge.v4);
        _terrain.AddTriangleColor(color);
        _terrain.AddTriangle(center, edge.v4, edge.v5);
        _terrain.AddTriangleColor(color);
    }

    private void TriangulateEdgeFan(Vector3 center, EdgeVertices edge)
    {
        _terrain.AddTriangle(center, edge.v1, edge.v2);

        _terrain.AddTriangle(center, edge.v2, edge.v3);

        _terrain.AddTriangle(center, edge.v3, edge.v4);

        _terrain.AddTriangle(center, edge.v4, edge.v5);
    }
    
    private void TriangulateEdgeStrip(EdgeVertices e1, Color c1, EdgeVertices e2, Color c2, bool hasWall = false)
    {
        _terrain.AddQuad(e1.v1, e1.v2, e2.v1, e2.v2);
        _terrain.AddQuadColor(c1, c2);
        _terrain.AddQuad(e1.v2, e1.v3, e2.v2, e2.v3);
        _terrain.AddQuadColor(c1, c2);
        _terrain.AddQuad(e1.v3, e1.v4, e2.v3, e2.v4);
        _terrain.AddQuadColor(c1, c2);
        _terrain.AddQuad(e1.v4, e1.v5, e2.v4, e2.v5);
        _terrain.AddQuadColor(c1, c2);

        if (hasWall)
        {
            TriangulateWallSegment(e1.v2, e1.v3, e1.v4, e2.v2, e2.v3, e2.v4);
        }
    }
    private void TriangulateEdgeStrip(EdgeVertices e1, EdgeVertices e2, bool hasWall = false)
    {
        _terrain.AddQuad(e1.v1, e1.v2, e2.v1, e2.v2);

        _terrain.AddQuad(e1.v2, e1.v3, e2.v2, e2.v3);

        _terrain.AddQuad(e1.v3, e1.v4, e2.v3, e2.v4);

        _terrain.AddQuad(e1.v4, e1.v5, e2.v4, e2.v5);

        if (hasWall)
        {
            TriangulateWallSegment(e1.v2, e1.v3, e1.v4, e2.v2, e2.v3, e2.v4);
        }
    }
    
    private void TriangulateWallSegment(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4, Vector3 v5, Vector3 v6)
    {
        _walls.AddQuad(v1, v2, v4, v5);
        _walls.AddQuad(v2, v3, v5, v6);
    }
    
    private void TriangulateWall(Vector3 center, Vector3 mL, Vector3 mR, EdgeVertices e, bool hasWallThroughCellEdge)
    {
        if (hasWallThroughCellEdge)
        {
            Vector3 mC = Vector3.Lerp(mL, mR, 0.5f);
            TriangulateWallSegment(mL, mC, mR, e.v2, e.v3, e.v4);
            _walls.AddTriangle(center, mL, mC);
            _walls.AddTriangle(center, mC, mR);
        }
        else
        {
            TriangulateWallEdge(center, mL, mR);
        }
    }

    private void TriangulateWall(HexDirection direction, HexCell cell, Vector3 center, EdgeVertices e)
    {
        TriangulateEdgeFan(center, e, cell.Color);
        //TriangulateEdgeFan(center, e);

        if (cell.HasWallThroughEdge(direction))
        {
            Vector2 interpolators = GetWallInterpolators(direction, cell);

            TriangulateWall(
                center,
                Vector3.Lerp(center, e.v1, interpolators.x),
                Vector3.Lerp(center, e.v5, interpolators.y),
                e,
                cell.HasWallThroughEdge(direction)
            );
        }
    }

    private void TriangulateWallEdge(Vector3 center, Vector3 mL, Vector3 mR)
    {
        _walls.AddTriangle(center, mL, mR);
    }

    private Vector2 GetWallInterpolators(HexDirection direction, HexCell cell)
    {
        Vector2 interpolators;

        if (cell.HasWallThroughEdge(direction))
        {
            interpolators.x = interpolators.y = 0.5f;
        }
        else
        {
            interpolators.x = cell.HasWallThroughEdge(direction.Previous()) ? 0.5f : 0.25f;
            interpolators.y = cell.HasWallThroughEdge(direction.Next()) ? 0.5f : 0.25f;
        }
        return interpolators;
    }

    public void AddCell(int index, HexCell cell)
    {
        _cells[index] = cell;
        cell.chunk = this;
        cell.transform.SetParent(transform, false);
        cell.uiRectTransform.SetParent(_gridCanvas.transform, false);
    }

    public void Refresh()
    {
        enabled = true;
    }
}
