using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Building : BuildableEntity
{

    public float EnergyGatheringRadius;
    public float BuildingLinkRadius;
    public int GatheringRate;

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        SetupContextMenu();

        // Set up LoS template
        // left tile, right tile, top tile, bottom tile
        LineOfSightTemplate = new List<Tuple<int, int>>
        {
            new Tuple<int, int>(0, 0),
            new Tuple<int, int>(-1, 0),
            new Tuple<int, int>(0, -1),
            new Tuple<int, int>(1, 0),
            new Tuple<int, int>(0, 1),
            new Tuple<int, int>(-2, 0),
            new Tuple<int, int>(0, -2),
            new Tuple<int, int>(2, 0),
            new Tuple<int, int>(0, 2),
            new Tuple<int, int>(-1, -1),
            new Tuple<int, int>(-1, -2),
            new Tuple<int, int>(-2, -1),
            new Tuple<int, int>(1, 1),
            new Tuple<int, int>(1, 2),
            new Tuple<int, int>(2, 1),
            new Tuple<int, int>(1, -1),
            new Tuple<int, int>(1, -2),
            new Tuple<int, int>(2, -1),
            new Tuple<int, int>(-1, 1),
            new Tuple<int, int>(-1, 2),
            new Tuple<int, int>(-2, 1),
        };
        InitialiseLineOfSight(X, Y);
    }

    public override void FinishBuilding()
    {
        base.FinishBuilding();
        var allBuildings = transform.parent.parent.GetComponent<Player>().Buildings.transform.GetComponentsInChildren<Building>();
        foreach (var building in allBuildings)
        {
            if (building.gameObject != this.gameObject)
            {
                float distance = (float)Math.Pow((X - building.X), 2) + (float)Math.Pow((Y - building.Y), 2);
                Debug.Log(distance);
                if (BuildingLinkRadius >= distance)
                    building.CreateConnection(this);
            }
        }
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        GatherAvailableEnergy(); // Set it to work with actual time rather than Update() time
    }

    public void GatherAvailableEnergy()
    {
        var allEnergyPatches = transform.root.GetComponentsInChildren<EnergyEntity>();
        foreach (var energyPatch in allEnergyPatches)
            GatherEnergyFromPatch(energyPatch);
    }

    public void GatherEnergyFromPatch(EnergyEntity patch)
    {
        if (patch.EnergyValue == 0) return;
        patch.EnergyValue--;
        GetOwner().GetComponent<Player>().Energy++;
    }

    #region Context-menu
    public Dictionary<string, Func<bool>> GuiMenuFunctions { get; set; }
    public void SetupContextMenu()
    {
        GuiMenuFunctions = new Dictionary<string, Func<bool>>()
        {
            // Add key-value pairs for context menu
            // Left-hand side: string to appear on the menu button
            // Right-hand side: Function to execute on button tap
            { "Fighter", BuildFighter },
            { "Node", BuildBuildingShip },
        };
    }
    #endregion

    #region Controls logic
    public override void OnTap()
    {
        if (!Built) return;
        if (!IsOwnedByActivePlayer()) return;
        transform.parent.parent.GetComponentInChildren<PlayerGui>().SetContextMenuFunctions(GuiMenuFunctions);
        transform.parent.parent.GetComponentInChildren<PlayerGui>().SetContextMenuVisible(true);
        transform.parent.parent.GetComponentInChildren<PlayerGui>().SetFocusedObject(this.gameObject);
    }
    #endregion

    #region Network-related logic

    public List<Building> ConnectedBuildings;
    public List<GameObject> BuildingLinks;
    public GameObject LinkPrefab;

    public void CreateConnection(Building otherBuilding)
    {
        ConnectedBuildings.Add(otherBuilding);
        otherBuilding.ConnectedBuildings.Add(this);
        GameObject linkCylinder = VisualiseConnection(otherBuilding);
        BuildingLinks.Add(linkCylinder);
        otherBuilding.BuildingLinks.Add(linkCylinder);
    }
    private GameObject VisualiseConnection(Building otherBuilding)
    {
        var linkCylinder = (GameObject)Instantiate(LinkPrefab);
        linkCylinder.transform.parent = transform.parent;
        linkCylinder.transform.localScale = new Vector3(linkCylinder.transform.localScale.x, linkCylinder.transform.localScale.y, Vector2.Distance(transform.position, otherBuilding.transform.position));
        linkCylinder.transform.position = new Vector3((transform.position.x + otherBuilding.transform.position.x) / 2, (transform.position.y + otherBuilding.transform.position.y) / 2, (transform.position.z + otherBuilding.transform.position.z) / 2);
        linkCylinder.transform.LookAt(otherBuilding.transform);
        return linkCylinder;
    }
    public void BreakConnection(Building otherBuilding)
    {
        int connectionIndex = ConnectedBuildings.IndexOf(otherBuilding);
        int otherConnectionIndex = otherBuilding.ConnectedBuildings.IndexOf(otherBuilding);
        BuildingLinks.RemoveAt(connectionIndex);
        otherBuilding.BuildingLinks.RemoveAt(otherConnectionIndex);

        ConnectedBuildings.Remove(otherBuilding);
        otherBuilding.ConnectedBuildings.Remove(this);
    }
    public void DisconnectFromAll()
    {
        foreach (Building otherBuilding in ConnectedBuildings)
        {
            BreakConnection(otherBuilding);
        }
    }

    #endregion

    #region Building units/buildings logic

    protected GameObject Build(GameObject objectToBuilt)
    {
        var newObject = (GameObject)Instantiate(objectToBuilt, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z), Quaternion.identity);
        return newObject;
    }

    public GameObject FighterObject;
    public GameObject BuildingShipObject;

    public bool BuildFighter()
    {
        GameObject newObject = Build(FighterObject);
        newObject.transform.parent = transform.parent.parent.GetComponent<Player>().Units.transform;
        var newUnit = newObject.GetComponent<Unit>();
        newUnit.X = X + 5;
        newUnit.Y = Y + 5;
        return true;
    }
    public bool BuildBuildingShip()
    {
        GameObject newObject = Build(BuildingShipObject);
        newObject.transform.parent = transform.parent.parent.GetComponent<Player>().Units.transform;
        var newUnit = newObject.GetComponent<BuildingShip>();
        newUnit.X = X + 5;
        newUnit.Y = Y + 5;
        return true;
    }

    #endregion
}
