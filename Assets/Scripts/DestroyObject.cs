using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class DestroyObject : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip closeClip;
    public void ButtonPressed()
    {
        if (transform.parent != null && (transform.parent.GetComponentInChildren<MouseGrab>() == null ||
            !transform.parent.GetComponentInChildren<MouseGrab>().IsBeingDragged()))
        {
            if (closeClip != null)
            {
                Vector3 listenerPosition = Camera.main != null ? Camera.main.transform.position : transform.position;
                AudioSource.PlayClipAtPoint(closeClip, listenerPosition, audioSource != null ? audioSource.volume : 1f);
            }
            Destroy(transform.parent.gameObject);
        }
    }
}
