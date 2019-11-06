using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HexMetrics
{
    private const float OuterToInner = 0.866025404f;
    public const float InnerToOuter = 1f / OuterToInner;

    public const float OuterRadius = 10f;
    public const float InnerRadius = OuterRadius * OuterToInner;

    private const float SolidFactor = 0.75f;
    private const float BlendFactor = 1f - SolidFactor;

    public const int ChunkSizeX = 5, ChunkSizeZ = 5;

    private static readonly Vector3[] Corners =
    {
        new Vector3(0f, 0f, OuterRadius),
        new Vector3(InnerRadius, 0f, 0.5f * OuterRadius),
        new Vector3(InnerRadius, 0f, -0.5f * OuterRadius),
        new Vector3(0f, 0f, -OuterRadius),
        new Vector3(-InnerRadius, 0f, -0.5f * OuterRadius),
        new Vector3(-InnerRadius, 0f, 0.5f * OuterRadius),
        new Vector3(0f, 0f, OuterRadius)
    };

    public static Vector3 GetFirstSolidCorner(HexDirection direction)
    {
        return Corners[(int)direction] * SolidFactor;
    }

    public static Vector3 GetSecondSolidCorner(HexDirection direction)
    {
        return Corners[(int)direction + 1] * SolidFactor;
    }
    
    public static Vector3 GetBridge(HexDirection direction)
    {
        return (Corners[(int)direction] + Corners[(int)direction + 1]) * BlendFactor;
    }
}
