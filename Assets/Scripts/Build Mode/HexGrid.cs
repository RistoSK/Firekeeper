using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour
{
    [SerializeField] private Color _defaultColor = Color.white;

    [SerializeField] private HexCell _cellPrefab;
    [SerializeField] private TextMeshProUGUI _cellLabelPrefab;

    [SerializeField] private HexGridChunk _chunkPrefab;
    [SerializeField] private int _chunkCountX = 4;
    [SerializeField] private int _chunkCountZ = 3;

    private HexCell[] _cells;
    private HexGridChunk[] _chunks;

    private int _cellCountX, _cellCountZ;

    public static HexGrid Instance { get; private set; }

    private void Awake()
    {
        // TODO: check if removing the null check breaks the logic, Instance != null
        if (Instance)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        _cellCountX = _chunkCountX * HexMetrics.ChunkSizeX;
        _cellCountZ = _chunkCountZ * HexMetrics.ChunkSizeZ;

        CreateChunks();
        CreateCells();
    }

    public int ChunkCountX => _chunkCountX;
    public int ChunkCountZ => _chunkCountZ;

    public HexCell GetCell(Vector3 position)
    {
        position = transform.InverseTransformPoint(position);
        HexCoordinates coordinates = HexCoordinates.WorldPositionToHexCoordinates(position);

        int index = coordinates.X + coordinates.Z * _cellCountX + coordinates.Z / 2;
        return _cells[index];
    }

    private void CreateCell(int x, int z, int i)
    {
        Vector3 position;
        position.x = (x + z * 0.5f - z / 2) * (HexMetrics.InnerRadius * 2f);
        position.y = 0.1f;
        position.z = z * (HexMetrics.OuterRadius * 1.5f);

        HexCell cell = _cells[i] = Instantiate(_cellPrefab);

        cell.transform.localPosition = position;
        cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
        cell.color = _defaultColor;

        if (x > 0)
        {
            cell.SetNeighbor(HexDirection.West, _cells[i - 1]);
        }

        if (z > 0)
        {
            if ((z & 1) == 0)
            {
                cell.SetNeighbor(HexDirection.SouthEast, _cells[i - _cellCountX]);

                if (x > 0)
                {
                    cell.SetNeighbor(HexDirection.SouthWest, _cells[i - _cellCountX - 1]);
                }
            }
            else
            {
                cell.SetNeighbor(HexDirection.SouthWest, _cells[i - _cellCountX]);

                if (x < _cellCountX - 1)
                {
                    cell.SetNeighbor(HexDirection.SouthEast, _cells[i - _cellCountX + 1]);
                }
            }
        }

        TextMeshProUGUI label = Instantiate(_cellLabelPrefab);
        label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);
        label.text = cell.coordinates.ToStringOnSeparateLines();

        cell.uiRectTransform = label.rectTransform;

        AddCellToChunk(x, z, cell);
    }

    private void CreateCells()
    {
        _cells = new HexCell[_cellCountZ * _cellCountX];

        for (int z = 0, i = 0; z < _cellCountZ; z++)
        {
            for (int x = 0; x < _cellCountX; x++)
            {
                CreateCell(x, z, i++);
            }
        }
    }

    private void CreateChunks()
    {
        _chunks = new HexGridChunk[_chunkCountX * _chunkCountZ];

        for (int z = 0, i = 0; z < _chunkCountZ; z++)
        {
            for (int x = 0; x < _chunkCountX; x++)
            {
                HexGridChunk chunk = _chunks[i++] = Instantiate(_chunkPrefab);
                chunk.transform.SetParent(transform);
            }
        }
    }

    private void AddCellToChunk(int x, int z, HexCell cell)
    {
        int chunkX = x / HexMetrics.ChunkSizeX;
        int chunkZ = z / HexMetrics.ChunkSizeZ;

        HexGridChunk chunk = _chunks[chunkX + chunkZ * _chunkCountX];

        int localX = x - chunkX * HexMetrics.ChunkSizeX;
        int localZ = z - chunkZ * HexMetrics.ChunkSizeZ;

        chunk.AddCell(localX + localZ * HexMetrics.ChunkSizeX, cell);
    }
}