using UnityEngine;
using System.Collections;

public class AbstractHexagonGridFromSizeCreator : MonoBehaviour {

    private Vector3[][] gridWorldPositions;
    public Vector3[][] GetWorldPositionsGrid()
    {
        return gridWorldPositions;
    }

    private float hexWidth;
    private float hexHeight;
    private float groundWidth;
    private float groundHeight;

    public void SetSizes(float hexW, float hexH, float groundW, float groundH)
    {
        hexWidth = hexW;
        hexHeight = hexH;
        groundWidth = groundW;
        groundHeight = groundH;
    }

    //The method used to calculate the number hexagons in a row and number of rows
    //Vector2.x is gridWidthInHexes and Vector2.y is gridHeightInHexes
    Vector2 CalcGridSize()
    {
        float sideLength = hexHeight / 2;
        //the number of whole hex sides that fit inside inside ground height
        int nrOfSides = (int)(groundHeight / sideLength);
        int gridHeightInHexes = (int)(nrOfSides * 2 / 3);
        //When the number of hexes is even the tip of the last hex in the offset column might stick up.
        //The number of hexes in that case is reduced.
        if (gridHeightInHexes % 2 == 0
            && (nrOfSides + 0.5f) * sideLength > groundHeight)
            gridHeightInHexes--;
        //gridWidth in hexes is calculated by simply dividing ground width by hex width
        return new Vector2((int)(groundWidth / hexWidth), gridHeightInHexes);
    }
    //Method to calculate the position of the first hexagon tile
    //The center of the hex grid is (0,0,0)
    Vector3 CalcInitPos()
    {
        Vector3 initPos;
        initPos = new Vector3(-groundWidth / 2 + hexWidth / 2, 0,
            groundHeight / 2 - hexWidth / 2);

        return initPos;
    }

    Vector3 CalcWorldCoord(Vector2 gridPos)
    {
        Vector3 initPos = CalcInitPos();
        float offset = 0;
        if (gridPos.y % 2 != 0)
            offset = hexWidth / 2;

        float x = initPos.x + offset + gridPos.x * hexWidth;
        float z = initPos.z - gridPos.y * hexHeight * 0.75f;
        //If your ground is not a plane but a cube you might set the y coordinate to sth like groundDepth/2 + hexDepth/2
        return new Vector3(x, 0, z);
    }

    public void InitialiseGrid()
    {
        Vector2 gridSize = CalcGridSize();

        gridWorldPositions = new Vector3[(int)gridSize.y][];

        for (float y = 0; y < gridSize.y; y++)
        {
            float sizeX = gridSize.x;
            //if the offset row sticks up, reduce the number of hexes in a row
            if (y % 2 != 0 && (gridSize.x + 0.5) * hexWidth > groundWidth)
                sizeX--;
            gridWorldPositions[(int)y] = new Vector3[(int) sizeX];
            for (float x = 0; x < sizeX; x++)
            {
                Vector2 gridPos = new Vector2(x, y);
                gridWorldPositions[(int)y][(int)x] = CalcWorldCoord(gridPos);
            }
        }
    }
}
