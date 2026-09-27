using UnityEngine;

public class SpawnWindow : MonoBehaviour
{
    [SerializeField] GameObject[] windowPrefabs;
    [SerializeField] float zOffset;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] laserVoicelines;
    [SerializeField] AudioClip[] inputVoicelines;
    [SerializeField] AudioClip[] notepadVoicelines;
    [SerializeField] AudioClip[] corruptedVoicelines;
    [SerializeField] AudioClip[] paintVoicelines;
    [SerializeField] AudioClip[] lookAtMeVoicelines;



    public GameObject CreateWindow(Vector3 location)
    {
        if (windowPrefabs == null || windowPrefabs.Length == 0)
            return null;

        GameObject newWindow = null;
        int randomIndex = Random.Range(0, windowPrefabs.Length);
        GameObject currPrefab = windowPrefabs[randomIndex];

        if(audioSource != null)
        {
            if (randomIndex == 0)
            {
                int randomVoiceline = Random.Range(0, laserVoicelines.Length);
                audioSource.PlayOneShot(laserVoicelines[randomVoiceline]);
            }
            else if (randomIndex == 1)
            {
                int randomVoiceline = Random.Range(0, inputVoicelines.Length);
                audioSource.PlayOneShot(inputVoicelines[randomVoiceline]);
            }
            else if (randomIndex == 2)
            {
                int randomVoiceline = Random.Range(0, notepadVoicelines.Length);
                audioSource.PlayOneShot(notepadVoicelines[randomVoiceline]);
            }
            else if (randomIndex == 3)
            {
                int randomVoiceline = Random.Range(0, corruptedVoicelines.Length);
                audioSource.PlayOneShot(corruptedVoicelines[randomVoiceline]);
            }
            else if (randomIndex == 4)
            {
                int randomVoiceline = Random.Range(0, paintVoicelines.Length);
                audioSource.PlayOneShot(paintVoicelines[randomVoiceline]);
            }
            else if (randomIndex == 5)
            {
                int randomVoiceline = Random.Range(0, lookAtMeVoicelines.Length);
                audioSource.PlayOneShot(lookAtMeVoicelines[randomVoiceline]);
            }
        }

        if (currPrefab != null)
        {
            newWindow = Instantiate(currPrefab, new Vector3(location.x, location.y, zOffset), transform.rotation, null);
            SpriteRenderer renderer = newWindow.GetComponentInChildren<SpriteRenderer>();
            zOffset -= 0.5f; // Decrease zOffset for the next window
        }

        return newWindow;
    }
}
