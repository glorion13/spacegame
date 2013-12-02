using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BuildingShip : Unit
{
    // Use this for initialization
    public override void Start()
    {
        base.Start();
        SetupContextMenu();

        // Set up LoS template
        // left tile, right tile, top tile, bottom tile
        /*LineOfSightTemplate = new List<Tuple<int, int>>
        {
            new Tuple<int, int>(0, 0),
            new Tuple<int, int>(-1, 0),
            new Tuple<int, int>(0, -1),
            new Tuple<int, int>(1, 0),
            new Tuple<int, int>(0, 1)
        };*/
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

    public override void OnTap()
    {
        if (!Built) return;
        if (!IsOwnedByActivePlayer()) return;
        transform.parent.parent.GetComponentInChildren<PlayerGui>().SetContextMenuFunctions(GuiMenuFunctions);
        transform.parent.parent.GetComponentInChildren<PlayerGui>().SetContextMenuVisible(true);
        transform.parent.parent.GetComponentInChildren<PlayerGui>().SetFocusedObject(this.gameObject);
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
            { "Deploy", Deploy }
        };
    }
    #endregion

    public GameObject BuildingObject;
    public bool Deploy()
    {
        var newObject = (GameObject)Instantiate(BuildingObject, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z), Quaternion.identity);
        newObject.transform.parent = transform.parent.parent.GetComponent<Player>().Buildings.transform;
        var newBuilding = newObject.GetComponent<Building>();
        newBuilding.SetWorldPositionFromGridPosition(X, Y);
        //CreateConnection(newUnit);
        return true;
    }
}
