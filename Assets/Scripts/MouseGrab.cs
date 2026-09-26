using UnityEngine;
using UnityEngine.InputSystem;

public class MouseGrab : MonoBehaviour
{
    private bool isBeingDragged = false;
    private Vector3 grabOffset;
    private Transform currentDragger;
    private static GameObject currentHighestWindow;
    private static int currentHighestOrder = 10;

    void Update()
    {
        if (isBeingDragged && currentDragger != null)
        {
            Vector3 draggerPos = currentDragger.position;
            transform.parent.position = new Vector3(draggerPos.x + grabOffset.x, draggerPos.y + grabOffset.y, transform.parent.position.z);
        }
    }

    public void StartDragging(Transform draggerTransform)
    {
        if (!isBeingDragged)
        {
            isBeingDragged = true;
            currentDragger = draggerTransform;
            grabOffset = (Vector3)transform.parent.position - (Vector3)draggerTransform.position;
            grabOffset.z = 0;
        }
    }

    public void StopDragging()
    {
        if (isBeingDragged)
        {
            isBeingDragged = false;
            currentDragger = null;
        }
    }

    public void toFront()
    {
        Transform rootWindow = transform.parent != null ? transform.parent : transform;
        SpriteRenderer[] renderers = rootWindow.GetComponentsInChildren<SpriteRenderer>();
        GameObject currWindow = rootWindow.gameObject;

        if (currWindow != null && currWindow != currentHighestWindow && renderers.Length > 0)
        {
            // FIX 2: Give the entire window a fresh base block of sorting order layers.
            // Spacing them out by 10 ensures child elements keep their relative depths
            currentHighestOrder += 10;

            foreach (SpriteRenderer renderer in renderers)
            {
                // Re-calculates internal layered spacing (e.g. text stays on top of panels)
                renderer.sortingOrder = currentHighestOrder + (renderer.sortingOrder % 10);
            }
            currentHighestWindow = currWindow;
        }
    }
}
