using UnityEngine;

public class PipeChildCollision : MonoBehaviour
{
    private MovingPipes parentPipeScript;

    void Awake()
    {
        // Automatically find the MovingPipes script on the parent object
        parentPipeScript = GetComponentInParent<MovingPipes>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Forward the collision up to the parent script
        if (parentPipeScript != null)
        {
            parentPipeScript.NotifyPipeCollision(collision);
        }
    }
}
