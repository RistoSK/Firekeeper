using UnityEngine;

public class WallProperties : MonoBehaviour
{
    public GameObject Wall() {  return gameObject; }

    public HexDirection Direction { get; set; }

    public HexCoordinates Coordinates { get; set; }
}
