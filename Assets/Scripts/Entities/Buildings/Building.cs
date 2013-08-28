using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Building : BuildableEntity
{

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
            new Tuple<int, int>(0, 1)
        };
        UpdateLineOfSight(X, Y, X, Y);
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        //SetNewPosition(X, Y); // only for debugging
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
            { "Fighter", BuildUnit },
            { "Node", BuildBuilding },
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
        var linkCylinder = (GameObject) Instantiate(LinkPrefab);
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
        var newObject = (GameObject) Instantiate(objectToBuilt, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z), Quaternion.identity);
        return newObject;
    }

    public GameObject UnitObject;
    public bool BuildUnit()
    {
        GameObject newObject = Build(UnitObject);
        newObject.transform.parent = transform.parent.parent.GetComponent<Player>().Units.transform;
        return true;
    }

    public GameObject BuildingObject;
    public bool BuildBuilding()
    {
        GameObject newObject = Build(BuildingObject);
        var newBuilding = newObject.GetComponent<Building>();
        newObject.transform.parent = transform.parent.parent.GetComponent<Player>().Buildings.transform;
        newBuilding.X = X + 20;
        newBuilding.Y = Y + 20;
        CreateConnection(newBuilding);
        return true;
    }

    #endregion
}
