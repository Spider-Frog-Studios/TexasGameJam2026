using UnityEngine;

public class SpawnWindow : MonoBehaviour
{
    GameObject windowPrefab;
    [SerializeField] float zOffset;

    public void CreateWindow()
    {
        if (windowPrefab != null)
        {
            Vector3 spawnLocation = transform.position;
            spawnLocation.z = zOffset;
            Instantiate(windowPrefab, spawnLocation, transform.rotation, null);
            zOffset -= 0.1f; // Decrease zOffset for the next window
        }
        else
        {
            Debug.LogError("Window prefab is not assigned.");
        }
    }

    public void CreateWindow(Vector3 location)
    {
        if (windowPrefab != null)
        {
            location.z = zOffset;
            Instantiate(windowPrefab, location, transform.rotation, null);
            zOffset -= 0.1f; // Decrease zOffset for the next window
        }
        else
        {
            Debug.LogError("Window prefab is not assigned.");
        }
    }
}
