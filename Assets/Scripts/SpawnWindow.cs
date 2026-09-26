using UnityEngine;

public class SpawnWindow : MonoBehaviour
{
    [SerializeField] GameObject windowPrefab;
    [SerializeField] float zOffset;

    public void CreateWindow()
    {
        if (windowPrefab != null)
        {
            Vector3 spawnLocation = transform.position;
            Instantiate(windowPrefab, new Vector3(spawnLocation.x, spawnLocation.y, zOffset), transform.rotation, null);
            zOffset -= 0.5f; // Decrease zOffset for the next window
        }
        else
        {
            Debug.LogError("Window prefab is not assigned.");
        }
    }

    public GameObject CreateWindow(Vector3 location)
    {
        GameObject newWindow = null;
        if (windowPrefab != null)
        {
            float randomR = Random.Range(.2f, .8f);
            float randomG = Random.Range(.2f, .8f);
            float randomB = Random.Range(.2f, .8f);
            newWindow = Instantiate(windowPrefab, new Vector3(location.x, location.y, zOffset), transform.rotation, null);
            newWindow.GetComponentInChildren<SpriteRenderer>().color = new Color(randomR, randomG, randomB, 1f); // Set the color to white with 50% transparency
            zOffset -= 0.5f; // Decrease zOffset for the next window
        }
        else
        {
            Debug.LogError("Window prefab is not assigned.");
        }

        return newWindow;
    }
}
