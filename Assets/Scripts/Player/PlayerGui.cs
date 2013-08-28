using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PlayerGui : MonoBehaviour {

    private Dictionary<string, Func<bool>> contextMenuFunctions;
    public void SetContextMenuFunctions(Dictionary<string, Func<bool>> contextMenuFunctions)
    {
        this.contextMenuFunctions = contextMenuFunctions;
    }
	
	private bool contextMenuVisible;
	public void SetContextMenuVisible(bool contextMenuVisible)
	{
		this.contextMenuVisible = contextMenuVisible;
	}
	public bool GetContextMenuVisible()
	{
		return contextMenuVisible;
	}
	
	private GameObject focusedObject;
	public void SetFocusedObject(GameObject focusedObject)
	{
		this.focusedObject = focusedObject;
	}
	public GameObject GetFocusedObject()
	{
		return focusedObject;
	}

    private Rect contextMenuBoundingBox;
    public Rect GetContextMenuBoundingBox()
    {
        return contextMenuBoundingBox;
    }
	
	void OnGUI()
	{
	    if ((!contextMenuVisible) || (focusedObject == null)) return;

	    // Get the position of the selected 3D object in screen coordinates
	    var screenPoint = this.gameObject.camera.WorldToScreenPoint(focusedObject.transform.position);
	    screenPoint.Set(screenPoint.x, Screen.height - screenPoint.y, screenPoint.z);
			
	    // Display context menu on these screen coordinates
	    contextMenuBoundingBox = new Rect(screenPoint.x, screenPoint.y, 100, 90);
	    GUI.Box(contextMenuBoundingBox, "");

	    // Loop through dictionary of available actions from the selected object and display them
	    var spacing = 0;
	    foreach (var guiMenuFunction in contextMenuFunctions)
	    {
	        if (GUI.Button(new Rect(screenPoint.x, screenPoint.y + spacing, 100, 20), guiMenuFunction.Key))
	        {
	            // Perform actions
	            guiMenuFunction.Value();
	            contextMenuVisible = false;
	        }
	        spacing += 20;
	    }
	}
	
}
