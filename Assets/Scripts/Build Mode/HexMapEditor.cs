using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HexMapEditor : MonoBehaviour
{
    [SerializeField] private Color[] _colors;
    [SerializeField] private HexGrid _hexGrid;
    [SerializeField] private Material _gridMaterial;

    private Dictionary<HexDirection, GameObject> _tempWalls = new Dictionary<HexDirection, GameObject>();
    private Dictionary<HexDirection, GameObject> _containedTempWalls = new Dictionary<HexDirection, GameObject>();

    private Color _activeColor;

    private void Awake()
    {
        SelectColor(0);
    }

    private void Start()
    {
        Quaternion quaternion = new Quaternion();

        for (HexDirection direction = HexDirection.NorthEast; direction <= HexDirection.NorthWest; direction++)
        {
            GameObject wall = Instantiate(BuildModeSettings.TempWall);

            switch (direction)
            {
                case HexDirection.NorthEast:
                    quaternion = Quaternion.Euler(0, 120f, 0);
                    wall.GetComponent<WallProperties>().Direction = HexDirection.NorthEast;
                    break;
                case HexDirection.SouthEast:
                    quaternion = Quaternion.Euler(0, 240f, 0);
                    wall.GetComponent<WallProperties>().Direction = HexDirection.SouthEast;
                    break;
                case HexDirection.SouthWest:
                    quaternion = Quaternion.Euler(0, 300f, 0);
                    wall.GetComponent<WallProperties>().Direction = HexDirection.SouthWest;
                    break;
                case HexDirection.NorthWest:
                    quaternion = Quaternion.Euler(0, 60f, 0);
                    wall.GetComponent<WallProperties>().Direction = HexDirection.NorthWest;
                    break;
                case HexDirection.West:
                    quaternion = Quaternion.Euler(0, 0, 0);
                    wall.GetComponent<WallProperties>().Direction = HexDirection.West;
                    break;
                case HexDirection.East:
                    quaternion = Quaternion.Euler(0, 0, 0);
                    wall.GetComponent<WallProperties>().Direction = HexDirection.East;
                    break;      
            }

            wall.transform.rotation = quaternion;
            wall.SetActive(false);

            _tempWalls.Add(direction, wall);
            
            _gridMaterial.EnableKeyword("GRID_ON");
        }

        var states = new Dictionary<Type, BuildingBaseState>()
        {
            { typeof(DefaultState), new DefaultState()},
            { typeof(DrawingWallState), new DrawingWallState()},
            { typeof(RemoveWallState), new RemoveWallState()},
            { typeof(SingleWallState), new SingleWallState(_tempWalls)},
            { typeof(BuildWallState), new BuildWallState()},
            { typeof(PrepareToBuildWallState), new PrepareToBuildWallState(_tempWalls)},
            { typeof(RotatingWallState), new RotatingWallState()},
            { typeof(PrepareToBuildRotatingWallState), new PrepareToBuildRotatingWallState(_tempWalls)}
        };

        GetComponent<BuildingStateMachine>().SetState(states);
        
    }

    public void SelectColor(int index)
    {
        _activeColor = _colors[index];
    }
}
