using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class DestroyObject : MonoBehaviour
{
    public void ButtonPressed()
    {
        if (transform.parent != null && (transform.parent.GetComponentInChildren<MouseGrab>() == null ||
            !transform.parent.GetComponentInChildren<MouseGrab>().IsBeingDragged()))
        {
            Destroy(transform.parent.gameObject);
        }
    }
}
