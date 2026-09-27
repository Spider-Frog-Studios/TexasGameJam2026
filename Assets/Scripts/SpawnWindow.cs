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
    [SerializeField] AudioClip[] quizVoicelines;
    [SerializeField] AudioClip[] KojimaVoicelines;

    public GameObject CreateWindow(Vector3 location)
    {
        if (windowPrefabs == null || windowPrefabs.Length == 0)
            return null;

        GameObject newWindow = null;
        int randomIndex = Random.Range(0, windowPrefabs.Length);
        GameObject currPrefab = windowPrefabs[randomIndex];

        if (currPrefab != null)
        {
            newWindow = Instantiate(currPrefab, new Vector3(location.x, location.y, zOffset), transform.rotation, null);
            PlayVoiceLine(randomIndex);
            zOffset -= 0.5f; // Decrease zOffset for the next window
        }

        return newWindow;
    }
    private void PlayVoiceLine(int prefabIndex)
    {
        // Matches the prefab's six voiced window types; additional types may be silent.
        AudioClip[] clips = prefabIndex switch
        {
            0 => laserVoicelines,
            1 => inputVoicelines,
            2 => notepadVoicelines,
            3 => corruptedVoicelines,
            4 => paintVoicelines,
            5 => lookAtMeVoicelines,
            6 => quizVoicelines,
            7 => KojimaVoicelines,
            _ => null
        };
        if (audioSource == null || clips == null || clips.Length == 0)
            return;
        AudioClip clip = clips[Random.Range(0, clips.Length)];
        if (clip != null)
            audioSource.PlayOneShot(clip);
    }
}
