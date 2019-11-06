using System.Collections.Generic;
using UnityEngine;

public class HexCell : MonoBehaviour
{
    public HexCoordinates coordinates;
    public Color color;
    public HexGridChunk chunk;
    public RectTransform uiRectTransform;

    [SerializeField] private HexCell[] _neighbors;
    [SerializeField] private bool[] _walls;

    private GameObject _tempWall;
    private GameObject _wall;
    public Dictionary<HexDirection, GameObject> _cellWalls = new Dictionary<HexDirection, GameObject>();
    private Quaternion _wallQuaternion;

    public Vector3 Position => transform.localPosition;

    public HexCell GetNeighbor(HexDirection direction)
    {
        return _neighbors[(int)direction];
    }

    public void SetNeighbor(HexDirection direction, HexCell cell)
    {
        _neighbors[(int)direction] = cell;
        cell._neighbors[(int)direction.Opposite()] = this;
    }

    public Color Color
    {
        get => color;

        set
        {
            if (color == value)
            {
                return;
            }
            color = value;
            Refresh();
        }
    }

    private void Refresh()
    {
        if (!chunk)
        {
            return;
        }

        chunk.Refresh();

        for (int i = 0; i < _neighbors.Length; i++)
        {
            HexCell neighbor = _neighbors[i];
            // TODO: check if removing the null check breaks the logic, neighbor != null
            if (neighbor && neighbor.chunk != chunk)
            {
                neighbor.chunk.Refresh();
            }
        }
    }

    public bool HasWallThroughEdge(HexDirection direction)
    {
        return _walls[(int) direction];
    }

    // TODO: currently not in use
    public bool HasWalls
    {
        get
        {
            for (int i = 0; i < _walls.Length; i++)
            {
                if (_walls[i])
                {
                    return true;
                }
            }
            return false;
        }
    }

    public void RemoveWall(GameObject clickedWall)
    {  
        // TODO: check if removing the null check breaks the logic, clickedWall != null
        if (clickedWall)
        {
            HexDirection direction;

            if (clickedWall.GetComponent<WallProperties>().Coordinates.X == coordinates.X &&
                clickedWall.GetComponent<WallProperties>().Coordinates.Y == coordinates.Y &&
                clickedWall.GetComponent<WallProperties>().Coordinates.Z == coordinates.Z)
            {
                direction = clickedWall.GetComponent<WallProperties>().Direction;
            }
            else
            {
                direction = clickedWall.GetComponent<WallProperties>().Direction.Opposite();
            }

            // Since the wall in on top of two cells we need to deactivate the wall on the other cell also
            GetNeighbor(direction)._cellWalls[direction.Opposite()].SetActive(false);
            _cellWalls[direction].SetActive(false);
           
        }

        for (int i = 0; i < _neighbors.Length; i++)
        {
            if (_walls[i])
            {
                _walls[i] = false;
                SetWall(i, false);
            }
        }
    }

    //TODO no need to have 2 separate methods for adding walls
    public void AddWall(HexDirection direction, GameObject wall)
    {
        if (!_walls[(int)direction])
        {
            BuildWall(direction, wall);
            SetWall((int)direction, true);
        }
    }

    public void AddRotatingWall(HexDirection direction, GameObject wall)
    {
        if (!_walls[(int)direction])
        {
            BuildWallWithRotation(direction, wall);
            SetWall((int)direction, true);
        } 
    }
    public void ShowTempWalls(Dictionary<HexDirection, GameObject> wallsToBeBuilt)
    {
        for (HexDirection direction = HexDirection.NorthEast; direction <= HexDirection.NorthWest; direction++)
        {
            _cellWalls.TryGetValue(direction, out _tempWall);
            // TODO: check if removing the null check breaks the logic, GetNeighbor(direction) == null || (_tempWall != null
            if (!GetNeighbor(direction) || (_tempWall && _tempWall.activeInHierarchy))
            {
                wallsToBeBuilt[direction].SetActive(false);
            }
            else
            {
                wallsToBeBuilt[direction].transform.position = (transform.position + _neighbors[(int)direction].transform.position) / 2;
                wallsToBeBuilt[direction].SetActive(true);
            }
        }     
    }

    public void ShowTempWall(Dictionary<HexDirection, GameObject> wallsToBeBuilt, HexDirection direction)
    {
        wallsToBeBuilt[direction].transform.position = (transform.position + _neighbors[(int)direction].transform.position) / 2;
        wallsToBeBuilt[direction].SetActive(true);
    }

    public void ShowContainedTempWall(Dictionary<HexDirection, GameObject> wallsToBeBuilt, HexDirection direction)
    {
        wallsToBeBuilt[direction].transform.position = transform.position;
        wallsToBeBuilt[direction].SetActive(true);
    }
    
    private void BuildWall(HexDirection direction, GameObject wall)
    {
        if (_cellWalls.TryGetValue(direction, out GameObject currentWall))
        {
           if (!currentWall.activeInHierarchy)
           {
               currentWall.SetActive(true);
               return;
           }
        }
        else
        {
            _wall = Instantiate(wall);
            _wall.GetComponent<WallProperties>().Direction = direction;
            _wall.GetComponent<WallProperties>().Coordinates = coordinates;
            _wall.transform.position = (transform.position + _neighbors[(int)direction].transform.position) / 2;
            _cellWalls.Add(direction, _wall);

            // Since the wall in on top of two cells we need to add the wall to the other cell also
            GetNeighbor(direction)._cellWalls.Add(direction.Opposite(), _wall);
        }

        switch(direction)
        {
            case HexDirection.NorthEast:
                _wallQuaternion = Quaternion.Euler(0, 120f, 0);
                break;
            case HexDirection.NorthWest:
                _wallQuaternion = Quaternion.Euler(0, 60f, 0);
                break;
            case HexDirection.SouthEast:
                _wallQuaternion = Quaternion.Euler(0, 240f, 0);
                break;
            case HexDirection.SouthWest:
                _wallQuaternion = Quaternion.Euler(0, 300f, 0);
                break;
            default:
                _wallQuaternion = Quaternion.Euler(0, 0, 0);
                break;
        }

        _wall.transform.rotation = _wallQuaternion;
    }

    private void BuildWallWithRotation(HexDirection direction, GameObject wall)
    {
        if (_cellWalls.TryGetValue(direction, out GameObject currentWall))
        {
            if (!currentWall.activeInHierarchy)
            {
                currentWall.SetActive(true);
                return;
            }
        }
        else
        {
            _wall = Instantiate(wall);
            _wall.GetComponent<WallProperties>().Direction = direction;
            _wall.GetComponent<WallProperties>().Coordinates = coordinates;
            _wall.transform.position = transform.position;
            _cellWalls.Add(direction, _wall);

            // Since the wall in on top of two cells we need to add the wall to the other cell also
            GetNeighbor(direction)._cellWalls.Add(direction.Opposite(), _wall);
        }

        switch(direction)
        {
            case HexDirection.NorthEast:
                _wallQuaternion = Quaternion.Euler(0, 120f, 0);
                break;
            case HexDirection.NorthWest:
                _wallQuaternion = Quaternion.Euler(0, 60f, 0);
                break;
            case HexDirection.SouthEast:
                _wallQuaternion = Quaternion.Euler(0, 240f, 0);
                break;
            case HexDirection.SouthWest:
                _wallQuaternion = Quaternion.Euler(0, 300f, 0);
                break;
            default:
                _wallQuaternion = Quaternion.Euler(0, 0, 0);
                break;
        }
        _wall.transform.rotation = _wallQuaternion;
    }
    
    private void SetWall(int index, bool state)
    {
        _walls[index] = state;
        _neighbors[index]._walls[(int)((HexDirection)index).Opposite()] = state;
        _neighbors[index].RefreshSelfOnly();
        RefreshSelfOnly();        
    }

    private void RefreshSelfOnly()
    {
        chunk.Refresh();
    }
}
