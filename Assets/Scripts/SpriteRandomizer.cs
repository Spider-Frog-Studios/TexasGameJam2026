using UnityEngine;

public class SpriteRandomizer : MonoBehaviour
{
    [SerializeField] Sprite[] notepadSprites;
    [SerializeField] AudioClip uniqueSound;
    [SerializeField] bool cannonSprites = false;
    [SerializeField] bool hasUniqueAudio = false;
    [SerializeField] bool audioOneShot = false;
    [SerializeField] int uniqueIndex;
    [SerializeField] GameObject[] lasers;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (notepadSprites == null || notepadSprites.Length == 0)
            return;
        int randomIndex = Random.Range(0, notepadSprites.Length);
        GetComponent<SpriteRenderer>().sprite = notepadSprites[randomIndex];
        if(cannonSprites && lasers != null && randomIndex < lasers.Length)
        {
            foreach (GameObject candidate in lasers)
                if (candidate != null) candidate.SetActive(false);
            if(transform.parent.GetComponentInChildren<LaserActivation>() != null)
            {
                transform.parent.GetComponentInChildren<LaserActivation>().SetLaser(lasers[randomIndex]);
            }
        }

        if (hasUniqueAudio)
        {
            playUniqueSound(uniqueIndex, audioOneShot);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void playUniqueSound(int checkIndex, bool isOneShot)
    {
        AudioSource audioSource = transform.parent.GetComponentInChildren<AudioSource>();
        if(GetComponent<SpriteRenderer>().sprite == notepadSprites[checkIndex])
        {
            if (isOneShot)
            {
                audioSource.PlayOneShot(uniqueSound);
            } else
            {
                audioSource.loop = true;
                audioSource.Play();
            }
                
        }
    }
}
