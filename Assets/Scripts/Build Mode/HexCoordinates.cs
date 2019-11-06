using UnityEngine;

[System.Serializable]
public struct HexCoordinates
{
    [SerializeField] private int _x, _z;

    public int X => _x;

    public int Z => _z;

    public int Y => -X - Z;

    public HexCoordinates(int x, int z)
    {
        _x = x;
        _z = z;
    }

    public static HexCoordinates FromOffsetCoordinates(int x, int z)
    {
        return new HexCoordinates(x - z / 2, z);
    }

    public static HexCoordinates WorldPositionToHexCoordinates(Vector3 position)
    {
        // get x,y coordinates when z coordinate is 0
        // divide x by the horizontal width of a hexagon and y coordinate is a mirror of the x coordinate
        float x = position.x / (HexMetrics.InnerRadius * 2f);
        float y = -x;

       // from the z value we calculate the offset
       // every two rows we should shift an entire unit to the left
        float offset = position.z / (HexMetrics.OuterRadius * 3f);
        x -= offset;
        y -= offset;

        // x,y values end up as whole numbers. By rounding them to integers we get the coordinates
        int iX = Mathf.RoundToInt(x);
        int iY = Mathf.RoundToInt(y);
        int iZ = Mathf.RoundToInt(-x - y);

        // the coordinates sum should always be 0
        // when clicking near the edge of the hex we might end up with a different sum
        // the solution then is to discard the coordinate with the largest rounding delta, and reconstruct it from the other two
        if (iX + iY + iZ != 0)
        {
            float dX = Mathf.Abs(x - iX);
            float dY = Mathf.Abs(y - iY);
            float dZ = Mathf.Abs(-x - y - iZ);

            if (dX > dY && dX > dZ)
            {
                iX = -iY - iZ;
            }
            else if (dZ > dY)
            {
                iZ = -iX - iY;
            }
        }
       
        return new HexCoordinates(iX, iZ);
    }

    public override string ToString()
    {
        return "(" + X.ToString() + ", " + Y.ToString() + ", " +  Z.ToString() + ")";
    }

    public string ToStringOnSeparateLines()
    {
        return X.ToString() + "\n"+ Y.ToString() + "\n" + Z.ToString();
    }
}