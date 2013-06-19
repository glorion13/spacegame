using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public abstract class InstanceBehaviour : MonoBehaviour, IVisible {

    #region Public
    // Monitors the build progress of the current building (value between 0 and 100)
    public int completionProgress;
    public float secondsToComplete;

    public int x;
    public int y;

    /* TODO: Unit tile size */

    protected GameObject Map;

    public void SetWorldPositionFromGridPosition(int x, int y)
    {
        if (!((x == this.x) && (y == this.y)))
        {
            this.x = x;
            this.y = y;
        }
        this.transform.position = Map.GetComponentInChildren<MapGridBehaviour>().GetWorldPositionFromGridPosition(x, y);
    }

    #endregion
    #region Protected
    // Current player object
    protected GameObject player;
    // A dictionary mapping context-menu buttons to functions
    protected Dictionary<string, Func<bool>> guiMenuFunctions;
    // GUI behaviours script of the player
    protected GuiBehaviour guiBehaviour;
    // Set the interface to be enabled or disabled
    protected bool IsFinishedBuilding;
    #endregion

    #region Game-related logic
    // Build progress of self
    protected void StartBuildingProgress()
    {
        // Start gradually creating the building
        if (completionProgress < 100)
        {
            InvokeRepeating("ProgressCompletion", 0, secondsToComplete / 100);
        }
        else
        {
            IsFinishedBuilding = true;
        }
    }

    public bool IsVisible(FogOfWarGridBehaviour visibilityGrid)
    {
        if (visibilityGrid.GetVisibilityInTile(x, y) > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    protected void ProgressCompletion()
    {
        completionProgress++;
        if (completionProgress == 100)
        {
            IsFinishedBuilding = true;
            CancelInvoke("ProgressCompletion");
        }
    }
    #endregion
}
