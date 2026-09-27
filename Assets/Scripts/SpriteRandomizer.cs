using UnityEngine;

public class SpriteRandomizer : MonoBehaviour
{
    [SerializeField] Sprite[] notepadSprites;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = notepadSprites[Random.Range(0, notepadSprites.Length)];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
