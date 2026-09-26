using UnityEngine;
using UnityEngine.InputSystem;

public class Window : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnClick(InputValue value)
    {
        if (value.isPressed)
        {
            // Handle click event
            Debug.Log("Window clicked!");
        }
    }

    void FollowObject(Transform target)
    {
        this.transform.SetParent(target);
    }

    void UnfollowObject()
    {
        this.transform.SetParent(null);
    }
}
