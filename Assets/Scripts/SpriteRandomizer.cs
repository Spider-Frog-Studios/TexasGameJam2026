using UnityEngine;

public class SpriteRandomizer : MonoBehaviour
{
    [SerializeField] Sprite[] notepadSprites;
    [SerializeField] bool cannonSprites = false;
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
