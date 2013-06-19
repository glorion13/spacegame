using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class NodeBehaviour : InstanceBehaviour
{
    // store the nodes which are linked to each other
    protected List<GameObject> connectedNodes;

    public void PerformOnSelect()
    {
        if (IsFinishedBuilding)
        {
            guiBehaviour.SetFocusedObject(this.gameObject);
            guiBehaviour.SetContextMenuFunctions(guiMenuFunctions);
            guiBehaviour.SetContextMenuVisible(true);
        }
        else
        {
        }
    }

    #region Game-related logic

    // Network-related logic
    public void CreateConnection(GameObject node1, GameObject node2)
    {
        node1.GetComponent<NodeBehaviour>().connectedNodes.Add(node2);
        node2.GetComponent<NodeBehaviour>().connectedNodes.Add(node1);
    }
    public void BreakConnection(GameObject node1, GameObject node2)
    {
        node1.GetComponent<NodeBehaviour>().connectedNodes.Remove(node2);
        node2.GetComponent<NodeBehaviour>().connectedNodes.Remove(node1);
    }
    public void DisconnectNodeFromAll(GameObject node)
    {
        foreach (GameObject otherNode in node.GetComponent<NodeBehaviour>().connectedNodes)
        {
            BreakConnection(node, otherNode);
        }
    }

    // Building units
    protected GameObject Build(GameObject objectToBuilt)
    {
        GameObject newObject = (GameObject) Instantiate(objectToBuilt, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z), Quaternion.identity);
        return newObject;
    }

#endregion
}
