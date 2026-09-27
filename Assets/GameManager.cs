using UnityEngine;

public class GameManager : MonoBehaviour
{

    private float gameTime = 0f;
    private int windowsCompleted;
    public static GameManager Instance;
    [SerializeField] AudioClip errorSound;

    [Header("Malware")]
    [SerializeField, Min(1f)] private float malwareFillSeconds = 180f;
    private float malwareSeconds;
    private bool malwareLimitReached;

    public event System.Action<float> MalwareAdjusted;

    public float MalwareProgress => malwareSeconds / Mathf.Max(1f, malwareFillSeconds);

    void Awake()
    {
        Instance =  this;
    }

    // Update is called once per frame
    void Update()
    {
        gameTime += Time.deltaTime;
        SetMalwareSeconds(malwareSeconds + Time.deltaTime);
    }

    public void AddMalware(float seconds)
    {
        AudioSource source = GetComponent<AudioSource>();
        if (source != null && errorSound != null)
            source.PlayOneShot(errorSound);
        AdjustMalware(Mathf.Max(0f, seconds));
    }

    public void SubtractMalware(float seconds)
    {
        AdjustMalware(-Mathf.Max(0f, seconds));
    }

    private void AdjustMalware(float seconds)
    {
        SetMalwareSeconds(malwareSeconds + seconds);
        if (seconds != 0f)
        {
            MalwareAdjusted?.Invoke(seconds);
        }
    }

    private void SetMalwareSeconds(float seconds)
    {
        float limit = Mathf.Max(1f, malwareFillSeconds);
        malwareSeconds = Mathf.Clamp(seconds, 0f, limit);
        if (malwareSeconds >= limit && !malwareLimitReached)
        {
            malwareLimitReached = true;
            endGame();
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    public float GetGameTime()
    {
        return gameTime;
    }

    public int GetWindowsCompleted()
    {
        return windowsCompleted;
    }

    public void CompleteWindow()
    {
        windowsCompleted++;
    }

    public void endGame()
    {
        Debug.Log("Go back to lobby");
    }
}
