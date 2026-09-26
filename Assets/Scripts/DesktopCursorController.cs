using UnityEngine;
using UnityEngine.InputSystem;

public class DesktopCursorController : MonoBehaviour
{

    private Camera mainCamera;
    private GameObject selectedWindow;
    private bool leftMousePressed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = Camera.main;
        leftMousePressed = false;
    }

    // Update is called once per frame
    void Update()
    {
        FollowMouse();

        if (leftMousePressed)
        {
            Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
            Vector2 mouseWorldPos = mainCamera.ScreenToWorldPoint(mouseScreenPos);

            Collider2D hitCollider = Physics2D.OverlapPoint(mouseWorldPos);

            if (hitCollider != null)
            {
                GameObject clickedObject = hitCollider.gameObject;

                if (clickedObject.GetComponent<MouseGrab>())
                {
                    selectedWindow = clickedObject;
                    Debug.Log("Window clicked!");
                    selectedWindow.GetComponent<MouseGrab>().StartDragging(this.transform);
                } else if(clickedObject.GetComponent<DestroyObject>() && selectedWindow == null)
                {
                    clickedObject.GetComponent<DestroyObject>().ButtonPressed();
                }
                else
                {
                    Debug.Log("Clicked object is not a window.");
                }
            } else
            {
                Debug.Log("No object clicked.");
            }
        } else if(selectedWindow != null)
        {
            selectedWindow.GetComponent<MouseGrab>().StopDragging();
            selectedWindow = null;
        }
    }

    void OnClick(InputValue value)
    {
        leftMousePressed = value.isPressed;
    }

    private void FollowMouse()
    {
        Vector3 targetPos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        targetPos.z = 0;
        transform.position = targetPos;
    }


}
