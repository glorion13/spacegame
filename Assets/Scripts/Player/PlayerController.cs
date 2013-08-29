using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    // 
    private Entity entityHit;

    // Touch states
    private bool moved;
    private bool stationary;
    private bool touchStarted;

    private Entity firstEntityHit;
    private Entity lastEntityHit;

	void Update () {
        // *TODO Create the various distinctions between 'Tap', 'LongTap' and 'Drag', as well as Pinch and Pan
        // TouchPhase.Began
        // TouchPhase.Moved
        // TouchPhase.Stationary
        // TouchPhase.Ended

        // Tap
        if (touchStarted)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.collider.gameObject);
                Debug.Log(GameObject.FindGameObjectWithTag("Map").GetComponent<MapGrid>().GetGridPositionFromWorldPosition(hit.collider.gameObject.transform.position));
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            // Raycast to find if user input hit a 3D object
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                firstEntityHit = hit.collider.gameObject.GetComponent<Entity>();
                if (firstEntityHit != null)
                {
                    touchStarted = true;
                    Debug.Log("start touch");
                    Debug.Log(firstEntityHit);
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("end touch");
            touchStarted = false;
            // Raycast to find if user input hit a 3D object
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                lastEntityHit = hit.collider.gameObject.GetComponent<Entity>();
                if (lastEntityHit == null)
                    HideContextMenu();
                else
                {
                    //bool inputIsFromOwner = entityHit.transform.parent.parent.GetComponent<NetworkView>().isMine;
                    lastEntityHit.OnTap();
                }
            }
            else
                HideContextMenu();
        }
	}

    void HideContextMenu()
    {
        if (!(IsPointInRectangle(Input.mousePosition, this.transform.parent.GetComponentInChildren<PlayerGui>().GetContextMenuBoundingBox())))
            // Clear GUI context menus
            this.transform.parent.GetComponentInChildren<PlayerGui>().SetContextMenuVisible(false);
    }

    bool IsPointInRectangle(Vector3 position, Rect rectangle)
    {
        position.Set(position.x, Screen.height - position.y, position.z);
        return (position.x >= rectangle.xMin) && (position.y >= rectangle.yMin) && (position.x <= rectangle.xMax) && (position.y <= rectangle.yMax);
    }

}
