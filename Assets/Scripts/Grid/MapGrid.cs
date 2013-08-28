using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class MapGrid : MonoBehaviour
{
    public float HexagonSize;
    // Note that for correct hexagonal geometry, the height to width ratio must be
    // equal to sqrt(3) / 2.

    public Vector3[][] GridWorldPositions;

    public Vector3 GetWorldPositionFromGridPosition(int x, int y)
    {
        // Inverse y and x to match the grid row-column priorities
        return GridWorldPositions[y][x];
    }

}
