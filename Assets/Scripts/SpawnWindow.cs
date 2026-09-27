using UnityEngine;

public class SpawnWindow : MonoBehaviour
{
    [SerializeField] GameObject[] windowPrefabs;
    [SerializeField] float zOffset;

    public GameObject CreateWindow(Vector3 location)
    {
        if (windowPrefabs == null || windowPrefabs.Length == 0)
            return null;

        GameObject newWindow = null;
        GameObject currPrefab = windowPrefabs[Random.Range(0, windowPrefabs.Length)];
        if (currPrefab != null)
        {
            float randomR = Random.Range(.2f, .8f);
            float randomG = Random.Range(.2f, .8f);
            float randomB = Random.Range(.2f, .8f);
            newWindow = Instantiate(currPrefab, new Vector3(location.x, location.y, zOffset), transform.rotation, null);
            SpriteRenderer renderer = newWindow.GetComponentInChildren<SpriteRenderer>();
            if (renderer != null)
                renderer.color = new Color(randomR, randomG, randomB, 1f);
            zOffset -= 0.5f; // Decrease zOffset for the next window
        }

        return newWindow;
    }
}
