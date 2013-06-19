using UnityEngine;
using System.Collections;

public class ControllerBehaviour : MonoBehaviour {
	
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            // Raycast to find if user input hit a 3D object
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                // If it hit a building
                if (hit.collider.gameObject.tag == "Building")
                {
                    hit.collider.gameObject.GetComponent<NodeBehaviour>().PerformOnSelect();
                }
                // If it hit a unit
                else if (hit.collider.gameObject.tag == "Unit")
                {
                }
                // If none of the above
                else
                {
                    Debug.Log(hit.collider.gameObject);
                    if (!(IsPointInRectangle(Input.mousePosition, this.gameObject.GetComponent<GuiBehaviour>().GetContextMenuBoundingBox())))
                        // Clear GUI context menus
                        this.gameObject.GetComponent<GuiBehaviour>().SetContextMenuVisible(false);
                }
            }
        }
	}

    bool IsPointInRectangle(Vector3 position, Rect rectangle)
    {
        position.Set(position.x, Screen.height - position.y, position.z);
        if ((position.x >= rectangle.xMin) && (position.y >= rectangle.yMin) && (position.x <= rectangle.xMax) && (position.y <= rectangle.yMax))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
