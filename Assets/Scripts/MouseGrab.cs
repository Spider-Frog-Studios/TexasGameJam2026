using TMPro;
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
        if(isBeingDragged && currentDragger != null)
        {
            Vector3 draggerPos = currentDragger.position;
            transform.parent.position = new Vector3(draggerPos.x + grabOffset.x, draggerPos.y + grabOffset.y, transform.parent.position.z);
        }
    }

    public void StartDragging(Transform draggerTransform)
    {
        if(!isBeingDragged)
        {
            isBeingDragged = true;
            currentDragger = draggerTransform;
            grabOffset = (Vector3)transform.parent.position - (Vector3)draggerTransform.position;
            grabOffset.z = 0; 
            
            // FIX 1: Automatically call your front logic when the drag starts
            toFront();
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
        GameObject currWindow = rootWindow.gameObject;

        if (currWindow != null && currWindow != currentHighestWindow)
        {
            // Give the entire window group a fresh base block of sorting layers
            currentHighestOrder += 10;

            // 1. Update all standard Sprite Renderers
            SpriteRenderer[] renderers = rootWindow.GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer renderer in renderers)
            {
                if (renderer != null)
                {
                    renderer.sortingOrder = currentHighestOrder + (renderer.sortingOrder % 10);
                }
            }

            // 2. Find the text objects and safely shift their Mesh Renderers
            TMP_Text[] textComponents = rootWindow.GetComponentsInChildren<TMP_Text>();
            foreach (TMP_Text txt in textComponents)
            {
                if (txt != null)
                {
                    Renderer textRenderer = txt.GetComponent<Renderer>();
                    if (textRenderer != null)
                    {
                        textRenderer.sortingOrder = currentHighestOrder + (textRenderer.sortingOrder % 10);
                    }
                }
            }

            // 3. NEW: Find any World Space Canvases and shift their sorting order!
            Canvas[] windowCanvases = rootWindow.GetComponentsInChildren<Canvas>();
            foreach (Canvas canvas in windowCanvases)
            {
                if (canvas != null)
                {
                    canvas.sortingOrder = currentHighestOrder + 5;
                }
            }

            currentHighestWindow = currWindow;
        }
    }
}
