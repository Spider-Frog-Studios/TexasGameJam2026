using Unity.VisualScripting;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    public void ButtonPressed()
    {
        if(transform.parent != null)
        {
            Destroy(transform.parent.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
