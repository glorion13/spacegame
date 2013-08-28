using System;
using UnityEngine;
using System.Collections;

public class HexagonGridCreator : MonoBehaviour {

    private Vector3[][] gridWorldPositions;
    public Vector3[][] GetWorldPositionsGrid()
    {
        return gridWorldPositions;
    }

    private float hexWidth;
    private float hexHeight;
    private float hexSize;
    private float groundWidth;
    private float groundHeight;

    public void SetSizes(float size, float groundW, float groundH)
    {
        hexSize = size;
        hexHeight = size*2;
        hexWidth = (float) (hexHeight * (Math.Sqrt(3) / 2));
        groundWidth = groundW;
        groundHeight = groundH;
    }

    Vector2 CalculateGridSize()
    {
        var nrOfSides = (int) (groundHeight / hexSize);
        var gridHeightInHexes = nrOfSides * 2 / 3;
        // When the number of hexes is even the tip of the last hex in the offset column might stick up.
        // The number of hexes in that case is reduced.
        if (gridHeightInHexes % 2 == 0 && (nrOfSides + 0.5f) * hexSize > groundHeight)
            gridHeightInHexes--;
        // gridWidth in hexes is calculated by simply dividing ground width by hex width
        var gridWidthInHexes = (int) (groundWidth / hexWidth);
        return new Vector2(gridWidthInHexes, gridHeightInHexes);
    }

    Vector3 CalculateInitialPosition()
    {
        //The center of the hex grid is (0,0,0)
        var initPos = new Vector3(-groundWidth / 2 + hexWidth / 2, 0, groundHeight / 2 - hexWidth / 2);
        return initPos;
    }

    Vector3 CalculateWorldCoordinates(Vector2 gridPos)
    {
        Vector3 initPos = CalculateInitialPosition();
        float offset = 0;
        if (Math.Abs(gridPos.y % 2 - 0) > Math.E)
            offset = hexWidth / 2;

        var x = initPos.x + offset + gridPos.x * hexWidth;
        var z = initPos.z - gridPos.y * hexHeight * 0.75f;
        //If the ground is not a plane but a cube, can set the y coordinate to sth like groundDepth/2 + hexDepth/2
        return new Vector3(x, 0, z);
    }

    public void InitialiseGrid()
    {
        Vector2 gridSize = CalculateGridSize();

        gridWorldPositions = new Vector3[(int) gridSize.y][];

        for (int y = 0; y < gridSize.y; y++)
        {
            float sizeX = gridSize.x;
            //if the offset row sticks up, reduce the number of hexes in a row
            if ((Math.Abs(y % 2 - 0) > Math.E) && (gridSize.x + 0.5) * hexWidth > groundWidth)
                sizeX--;
            gridWorldPositions[y] = new Vector3[(int) sizeX];
            for (int x = 0; x < sizeX; x++)
            {
                var gridPos = new Vector2(x, y);
                gridWorldPositions[y][x] = CalculateWorldCoordinates(gridPos);
            }
        }
    }
}
