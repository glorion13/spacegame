using UnityEngine;
using System.Collections;
using System.Linq;

public class FogOfWarGridBehaviour : MonoBehaviour
{

    // -1 = Unexplored, 0 = Fogged, 1+ = Visible by 1+ units
    public int[][] VisibilityTiles;

    void InitialiseVisibilityTiles()
    {
        GameObject Map = GameObject.FindGameObjectWithTag("Map").gameObject;
        VisibilityTiles = new int[Map.GetComponentInChildren<MapGridBehaviour>().GetWorldPositionsGrid().Length][];
        for (int y = 0; y < VisibilityTiles.Length; y++)
        {
            VisibilityTiles[y] = new int[Map.GetComponentInChildren<MapGridBehaviour>().GetWorldPositionsGrid()[y].Length];
            for (int x = 0; x < VisibilityTiles[y].Length; x++)
            {
                VisibilityTiles[y][x] = -1;
            }
        }
    }

    void Start() {
        InitialiseVisibilityTiles();
	}

    public int GetVisibilityInTile(int x, int y)
    {
        return VisibilityTiles[y][x];
    }

    public void IncreaseVisibilityTileValue(int x, int y)
    {
        VisibilityTiles[y][x]++;
        if (VisibilityTiles[y][x] == 0)
        {
            VisibilityTiles[y][x]++; ;
        }
        // Get the position of the selected 3D object in screen coordinates
        GameObject Map = GameObject.FindGameObjectWithTag("Map").gameObject;
        Vector3 screenPoint = this.gameObject.transform.root.GetComponentInChildren<Camera>().camera.WorldToScreenPoint(Map.GetComponentInChildren<MapGridBehaviour>().GetWorldPositionFromGridPosition(x, y));

        screenPoint.Set(screenPoint.x, Screen.height - screenPoint.y, screenPoint.z);
    }
    public void DecreaseVisibilityTileValue(int x, int y)
    {
        VisibilityTiles[y][x]--;
    }

}
