using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class MapGridBehaviour : AbstractHexagonGridFromSizeCreator
{
    public float HexagonWidth;
    public float HexagonHeight;

    public List<GameObject>[][] UnitTiles;

    void Start()
    {
        SetSizes(HexagonWidth, HexagonHeight, GameObject.FindGameObjectWithTag("MapMesh").renderer.bounds.size.x, GameObject.FindGameObjectWithTag("MapMesh").renderer.bounds.size.z);
        InitialiseGrid();
        InitialiseUnitTiles();
    }

    public Vector3 GetWorldPositionFromGridPosition(int x, int y)
    {
        // Inverse y and x to match the grid row-column priorities
        return GetWorldPositionsGrid()[y][x];
    }

    void InitialiseUnitTiles()
    {
        UnitTiles = new List<GameObject>[GetWorldPositionsGrid().Length][];
        for (int y = 0; y < UnitTiles.Length; y++)
        {
            UnitTiles[y] = new List<GameObject>[GetWorldPositionsGrid()[y].Length];
            for (int x = 0; x < UnitTiles[y].Length; x++)
            {
                UnitTiles[y][x] = new List<GameObject>();
            }
        }
    }
}
