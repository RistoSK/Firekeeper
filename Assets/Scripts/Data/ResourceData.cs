using UnityEngine;

public enum ResourceType
{
    Wood = 1 << 0,
    SpaceshipParts = 1 << 1,
    AttachedSpaceshipParts = 1 << 2,
}

[CreateAssetMenu(menuName = "Resource Data")]
public class ResourceData : ScriptableObject
{
    public int resourcePoints;
    public ResourceType resourceType;
    public int requiredHarvestTime;
    public Transform position;
}
