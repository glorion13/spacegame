using UnityEngine;
using System.Collections;

public class UnitBehaviour : InstanceBehaviour {

    void Start()
    {
        IsFinishedBuilding = false;

        player = this.gameObject.transform.root.gameObject;
        guiBehaviour = player.GetComponentInChildren<GuiBehaviour>();

        Map = GameObject.FindGameObjectWithTag("Map").gameObject;

        //SetupContextMenu();
        StartBuildingProgress();

        //SetWorldPositionFromGridPosition(x, y);
    }

    void Update()
    {
        // Set unit's position
        SetWorldPositionFromGridPosition(x, y);

        // Update LoS (normally we don't do this on update but only on every move)
        FogOfWarGridBehaviour visibilityGrid = player.GetComponentInChildren<FogOfWarGridBehaviour>();
        visibilityGrid.IncreaseVisibilityTileValue(x, y);

        // Make visible/invisible based on Fog of War visibility
        if (IsVisible(player.GetComponentInChildren<FogOfWarGridBehaviour>()))
        {
            this.renderer.enabled = true;
        }
        else
        {
            this.renderer.enabled = false;
        }
    }
}
