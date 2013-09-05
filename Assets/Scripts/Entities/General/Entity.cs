using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Entity : MonoBehaviour {

    private int x;
    public int X
    {
        get
        {
            return x;
        }
        set
        {
            x = Math.Min(Math.Max(0, value), GameObject.FindGameObjectWithTag("Map").GetComponent<MapGrid>().GridWorldPositions[0].Length - 1);
        }
    }
    private int y;
    public int Y
    {
        get
        {
            return y;
        }
        set
        {
            y = Math.Min(Math.Max(0, value), GameObject.FindGameObjectWithTag("Map").GetComponent<MapGrid>().GridWorldPositions.Length - 1);
        }
    }

    public bool Visible;
    public bool Enabled;

    public List<Tuple<int, int>> LineOfSightTemplate;
    public List<Tuple<int, int>> SizeTemplate;

    public virtual void SetVisible(bool visibleValue)
    {
        Visible = visibleValue;
        GetComponent<MeshRenderer>().enabled = visibleValue;
        SetEnabled(visibleValue);
    }
    public virtual void SetEnabled(bool enabledValue)
    {
        Enabled = enabledValue;
        GetComponent<Collider>().enabled = enabledValue;
    }

    public void SetWorldPositionFromGridPosition(int x, int y)
    {
        // Update grid position
        X = x;
        Y = y;
        // Set world position according to grid position
        transform.position = transform.root.GetComponent<Game>().Map.GetComponent<MapGrid>().GetWorldPositionFromGridPosition(X, Y);
    }

    // Controls functions
    public virtual void OnTap()
    {
    }
    public virtual void OnLongTap()
    {
    }
    public virtual void OnDragStart()
    {
    }
    public virtual void OnDrag(Vector2 dragCoordinates)
    {
    }
    public virtual void OnDragFinish()
    {
    }

    public virtual void Start()
    {
        SetWorldPositionFromGridPosition(X, Y);

        // Initialise LoS
        LineOfSightTemplate = new List<Tuple<int, int>>();

        // Initialise size (occupied tiles)
        SizeTemplate = new List<Tuple<int, int>>
        {
            new Tuple<int, int>(0, 0)
        };
    }

    public virtual void Update()
    {
        // Update whether the object is visible or not
        var isVisibleToActivePlayer = IsVisibleToPlayer(transform.root.GetComponent<Game>().ActivePlayer);
        SetEnabled(isVisibleToActivePlayer);
        SetVisible(isVisibleToActivePlayer);
    }

    public bool IsVisibleToPlayer(GameObject player)
    {
        var visibilityGrid = player.GetComponent<Player>().VisibilityGrid.GetComponent<VisibilityGrid>();
        foreach (var tile in SizeTemplate)
        {
            if (visibilityGrid.GetVisibilityInTile(X + tile.First, Y + tile.Second) > 0) return true;
        }
        return false;
    }

    public void InitialiseLineOfSight(int x, int y)
    {
        if (LineOfSightTemplate.Count <= 0) return;
        var visibilityGrid = transform.parent.parent.GetComponent<Player>().VisibilityGrid.GetComponent<VisibilityGrid>();
        foreach (var lineOfSightTiles in LineOfSightTemplate)
        {
            visibilityGrid.IncreaseVisibilityTileValue(x + lineOfSightTiles.First, y + lineOfSightTiles.Second);
        }
    }

    public void UpdateLineOfSight(int prevX, int prevY, int newX, int newY)
    {
        if (LineOfSightTemplate.Count <= 0) return;
        var visibilityGrid = transform.parent.parent.GetComponent<Player>().VisibilityGrid.GetComponent<VisibilityGrid>();
        foreach (var lineOfSightTiles in LineOfSightTemplate)
        {
            visibilityGrid.DecreaseVisibilityTileValue(prevX + lineOfSightTiles.First, prevY + lineOfSightTiles.Second);
            visibilityGrid.IncreaseVisibilityTileValue(newX + lineOfSightTiles.First, newY + lineOfSightTiles.Second);
        }
    }

    public void SetNewPosition(int x, int y)
    {
        UpdateLineOfSight(X, Y, x, y);
        SetWorldPositionFromGridPosition(x, y);
    }
}
