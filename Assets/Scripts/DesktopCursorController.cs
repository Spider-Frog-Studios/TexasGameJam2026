using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class DesktopCursorController : MonoBehaviour
{
    private Camera mainCamera;
    private GameObject selectedWindow;
    private bool leftMousePressed;

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip clickSound;

    void Start()
    {
        mainCamera = Camera.main;
        leftMousePressed = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    void Update()
    {
        FollowMouse();

        if (!leftMousePressed && selectedWindow != null)
        {
            selectedWindow.GetComponent<MouseGrab>().StopDragging();
            selectedWindow = null;
        }
    }

    void OnClick(InputValue value)
    {
        leftMousePressed = value.isPressed;

        if (leftMousePressed)
        {
            if (audioSource != null && clickSound != null)
                audioSource.PlayOneShot(clickSound);
            Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
            Vector2 mouseWorldPos = mainCamera.ScreenToWorldPoint(mouseScreenPos);

            // 1. Get ALL colliders under the mouse instead of just the first one
            Collider2D[] hitColliders = Physics2D.OverlapPointAll(mouseWorldPos);

            if (hitColliders.Length > 0)
            {
                Collider2D bestTarget = null;
                int highestOrderFound = int.MinValue;

                // 2. Loop through all objects under the mouse to find the one closest to the front
                foreach (Collider2D col in hitColliders)
                {
                    SpriteRenderer sr = col.GetComponentInChildren<SpriteRenderer>();

                    // If this specific piece doesn't have a renderer, check its parent
                    if (sr == null && col.transform.parent != null)
                    {
                        sr = col.transform.parent.GetComponentInChildren<SpriteRenderer>();
                    }

                    int currentOrder = (sr != null) ? sr.sortingOrder : 0;

                    // Keep track of whichever object has the highest sorting order
                    if (currentOrder > highestOrderFound)
                    {
                        highestOrderFound = currentOrder;
                        bestTarget = col;
                    }
                }

                // 3. Process the click ONLY on the visual topmost object
                if (bestTarget != null)
                {
                    GameObject clickedObject = bestTarget.gameObject;

                    if (clickedObject.GetComponent<MouseGrab>() && selectedWindow == null)
                    {
                        selectedWindow = clickedObject;
                        selectedWindow.GetComponent<MouseGrab>().StartDragging(this.transform);
                        selectedWindow.GetComponent<MouseGrab>().toFront();
                    }
                    else if (clickedObject.GetComponent<DestroyObject>() && selectedWindow == null)
                    {
                        clickedObject.GetComponent<DestroyObject>().ButtonPressed();
                    } else if (clickedObject.GetComponent<ChangeScene>())
                    {
                        clickedObject.GetComponent<ChangeScene>().LoadScene();
                    }
                    else if (clickedObject.layer == 6)
                    {
                        MouseGrab grabScript = clickedObject.transform.parent.GetComponentInChildren<MouseGrab>();
                        if (grabScript != null)
                        {
                            grabScript.toFront();
                            selectedWindow = grabScript.gameObject;
                        }
                    }
                }
            }
        }
    }

    private void FollowMouse()
    {
        Vector3 targetPos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        targetPos.z = 0;
        transform.position = targetPos;
    }
}
