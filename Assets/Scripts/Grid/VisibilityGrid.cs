using System;
using UnityEngine;
using System.Collections;
using System.Linq;

public class VisibilityGrid : MonoBehaviour
{

    // -1 = Unexplored, 0 = Fogged, 1+ = Visible by 1+ units
    public int[][] VisibilityTiles;
    public Texture2D VisibilityTexture;

    public void InitialiseVisibilityGrid(Vector3[][] worldPositionsGrid)
    {
        VisibilityTiles = new int[worldPositionsGrid.Length][];
        VisibilityTexture = new Texture2D(worldPositionsGrid[0].Length, worldPositionsGrid.Length, TextureFormat.RGB24, false, false)
            {
                wrapMode = TextureWrapMode.Clamp
            };
        for (var x = 0; x < VisibilityTexture.width; x++)
            for (var y = 0; y < VisibilityTexture.height; y++)
                VisibilityTexture.SetPixel(x, y, Color.black);

        for (var y = 0; y < VisibilityTiles.Length; y++)
        {
            VisibilityTiles[y] = new int[worldPositionsGrid[y].Length];
            for (var x = 0; x < VisibilityTiles[y].Length; x++)
                VisibilityTiles[y][x] = -1;
        }
        VisibilityTexture.Apply(false);
    }

    public int GetVisibilityInTile(int x, int y)
    {
        return VisibilityTiles[y][x];
    }
    public void SetVisibilityInTile(int x, int y, int visibilityValue)
    {
        if ((x < 0) || (y < 0) || (y > VisibilityTiles.Length - 1) || (x > VisibilityTiles[y].Length - 1)) return;
        VisibilityTiles[y][x] = visibilityValue;
    }

    public void IncreaseVisibilityTileValue(int x, int y)
    {
        if ((x < 0) || (y < 0) || (y > VisibilityTiles.Length - 1) || (x > VisibilityTiles[y].Length - 1)) return;
        VisibilityTiles[y][x]++;
        VisibilityTexture.SetPixel(x, y, new Color(1.0f, 1.0f, 1.0f));
        VisibilityTexture.Apply(false);
        if (VisibilityTiles[y][x] == 0)
            VisibilityTiles[y][x]++;
    }
    public void DecreaseVisibilityTileValue(int x, int y)
    {
        if ((x < 0) || (y < 0) || (y > VisibilityTiles.Length - 1) || (x > VisibilityTiles[y].Length - 1)) return;
        if (VisibilityTiles[y][x] <= 0) return;
        VisibilityTiles[y][x]--;
        if (VisibilityTiles[y][x] == 0)
        {
            VisibilityTexture.SetPixel(x, y, new Color(0.25f, 0.25f, 0.25f));
            VisibilityTexture.Apply(false);
        }
    }

}
