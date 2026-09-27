using UnityEngine;

public class SpawnPopUp : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameObject[] popUpPrefabs;

    public GameObject CreatePopUp()
    {
        if (popUpPrefabs == null || popUpPrefabs.Length == 0)
            return null;

        GameObject newPopUp = null;
        GameObject currPrefab = popUpPrefabs[Random.Range(0, popUpPrefabs.Length)];
        if (currPrefab != null)
        {
            newPopUp = Instantiate(currPrefab, new Vector3(0f, 0f, 0f), transform.rotation, null);
        }

        return newPopUp;
    }
}
