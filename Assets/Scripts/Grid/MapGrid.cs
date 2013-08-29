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

    public Vector2 GetGridPositionFromWorldPosition(Vector3 position)
    {
        float hexHeight = GameObject.FindGameObjectWithTag("MapMesh").renderer.bounds.size.z / GridWorldPositions.Length;
        float hexWidth = GameObject.FindGameObjectWithTag("MapMesh").renderer.bounds.size.x / GridWorldPositions[0].Length;
        float xOffset = GameObject.FindGameObjectWithTag("MapMesh").renderer.bounds.max.x;
        float zOffset = GameObject.FindGameObjectWithTag("MapMesh").renderer.bounds.max.z;
        return new Vector2((float)Math.Floor((position.x + GameObject.FindGameObjectWithTag("MapMesh").renderer.bounds.max.x) / hexWidth), (float)Math.Floor((-position.z + GameObject.FindGameObjectWithTag("MapMesh").renderer.bounds.max.z) / hexHeight));
    }

}
