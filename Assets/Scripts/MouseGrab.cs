using UnityEngine;
using UnityEngine.InputSystem;

public class MouseGrab : MonoBehaviour
{
    private bool isBeingDragged = false;
    private Vector3 grabOffset;
    private Transform currentDragger;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        if(isBeingDragged && currentDragger != null)
        {
            Vector3 draggerPos = currentDragger.position;
            transform.parent.position = draggerPos + grabOffset;
        }
    }

    public void StartDragging(Transform draggerTransform)
    {
        if(!isBeingDragged)
        {
            isBeingDragged = true;
            currentDragger = draggerTransform;
            grabOffset = (Vector3)transform.parent.position - (Vector3)draggerTransform.position;
            grabOffset.z = 0; // Ensure the offset is only in the XY plane
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
}
