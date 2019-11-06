using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildModeSettings : MonoBehaviour
{
    [SerializeField] private GameObject _wall;
    [SerializeField] private GameObject _tempWall;
    [SerializeField] private Color _singleColor;
    [SerializeField] private Color _drawColor;
    [SerializeField] private Color _removeColor;
    [SerializeField] private LayerMask _tempWallsLayerMask;
    [SerializeField] private LayerMask _wallLayerMask;
    [SerializeField] private LayerMask _cellLayerMask;

    private static BuildModeSettings BuildModeSettingsInstance { get; set; }

    public static GameObject Wall => BuildModeSettingsInstance._wall;

    public static GameObject TempWall => BuildModeSettingsInstance._tempWall;

    public static LayerMask TempWallsLayerMask => BuildModeSettingsInstance._tempWallsLayerMask;

    public static LayerMask WallLayerMask => BuildModeSettingsInstance._wallLayerMask;

    public static LayerMask CellLayerMask => BuildModeSettingsInstance._cellLayerMask;

    public static Color SingleColor => BuildModeSettingsInstance._singleColor;

    public static Color DrawColor => BuildModeSettingsInstance._drawColor;

    public static Color RemoveColor => BuildModeSettingsInstance._removeColor;

    private void Awake()
    {
        // TODO: check if removing the null check breaks the logic, BuildModeSettingsInstance != null
        if (BuildModeSettingsInstance)
        {
            Destroy(gameObject);
        }
        else
        {
            BuildModeSettingsInstance = this;
        }
    }
}
