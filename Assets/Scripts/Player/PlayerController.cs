using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    // Touch states
    private bool touchMoved;
    private bool touchStarted;

    // Possible entities hit by input
    private Entity firstEntityHit;
    private Entity lastEntityHit;

    // Latest mouse position
    private Vector3 latestMousePosition;

    void Update()
    {
        // *TODO Create the various distinctions between 'Tap', 'LongTap' and 'Drag', as well as Pinch and Pan
        // TouchPhase.Began
        // TouchPhase.Moved
        // TouchPhase.Stationary
        // TouchPhase.Ended

        // Tap
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
                    latestMousePosition = Input.mousePosition;
                    touchStarted = true;
                }
            }
        }


        if (Input.GetMouseButtonUp(0))
        {
            // Raycast to find if user input hit a 3D object
            touchStarted = false;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                lastEntityHit = hit.collider.gameObject.GetComponent<Entity>();
                if (touchMoved)
                {
                    firstEntityHit.OnDragFinish();
                    touchMoved = false;
                }
                else
                {
                    if (lastEntityHit == null)
                        HideContextMenu();
                    else
                    {
                        //bool entityIsOwnedByActor = entityHit.transform.parent.parent.GetComponent<NetworkView>().isMine;
                        lastEntityHit.OnTap();
                    }
                }
            }
            else
            {
                HideContextMenu();
                // Handle the case when the item is dragged out of the map limits
                if (touchMoved)
                {
                    firstEntityHit.OnDragFinish();
                    touchMoved = false;
                }
            }
        }

        if (touchStarted)
        {
            if (Input.mousePosition != latestMousePosition)
            {
                if (touchMoved)
                {
                    latestMousePosition = Input.mousePosition;
                    var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                        firstEntityHit.OnDrag(GameObject.FindGameObjectWithTag("Map").GetComponent<MapGrid>().GetGridPositionFromWorldPosition(hit.point));
                }
                else
                {
                    latestMousePosition = Input.mousePosition;
                    {
                        touchMoved = true;
                        firstEntityHit.OnDragStart();
                    }
                }
            }
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
