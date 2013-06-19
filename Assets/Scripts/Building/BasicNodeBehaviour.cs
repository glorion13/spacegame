using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BasicNodeBehaviour : NodeBehaviour, IVisible, IContextMenu {

    void Start()
    {
        IsFinishedBuilding = false;
        connectedNodes = new List<GameObject>();

        player = this.gameObject.transform.root.gameObject;
        guiBehaviour = player.GetComponentInChildren<GuiBehaviour>();

        Map = GameObject.FindGameObjectWithTag("Map").gameObject;

        SetupContextMenu();
        StartBuildingProgress();

        SetWorldPositionFromGridPosition(x, y);
    }

    void Update()
    {
        // Set unit's position
        SetWorldPositionFromGridPosition(x, y);

        FogOfWarGridBehaviour visibilityGrid = player.GetComponentInChildren<FogOfWarGridBehaviour>();

        // Make visible/invisible based on Fog of War visibility
        if (IsVisible(visibilityGrid))
        {
            this.renderer.enabled = true;
        }
        else
        {
            this.renderer.enabled = false;
        }
    }

    #region GUI-related logic

    public void SetupContextMenu()
    {
        guiMenuFunctions = new Dictionary<string, Func<bool>>()
        {
            { "Light fighter", BuildLightFighter },
            { "Node", BuildNode }
        };
    }

    #endregion

    #region Game-related logic

    public GameObject lightFighter;
    public bool BuildLightFighter()
    {
        GameObject newObject = Build(lightFighter);
        if (player.GetComponentInChildren<PlayerBehaviour>() != null)
        {
            newObject.transform.parent = player.GetComponentInChildren<PlayerBehaviour>().Units.transform;
        }
        return true;
    }

    public GameObject node;
    public bool BuildNode()
    {
        GameObject newObject = Build(node);
        if (player.GetComponentInChildren<PlayerBehaviour>() != null)
        {
            newObject.transform.parent = player.GetComponentInChildren<PlayerBehaviour>().Buildings.transform;
        }
        newObject.GetComponent<NodeBehaviour>().completionProgress = 0;
        newObject.GetComponent<NodeBehaviour>().x = 1;
        newObject.GetComponent<NodeBehaviour>().y = 1;
        return true;
    }

    #endregion
}
