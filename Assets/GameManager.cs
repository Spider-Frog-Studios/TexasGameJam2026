using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private float gameTime = 0f;
    private int windowsCompleted;
    public static GameManager Instance;
    [SerializeField] AudioClip errorSound;
    [SerializeField] AudioClip winSound;

    [Header("Malware")]
    [SerializeField, Min(1f)] private float malwareFillSeconds = 180f;
    private float malwareSeconds;
    private bool malwareLimitReached;
    private bool gameIsRunning;

    public event System.Action<float> MalwareAdjusted;

    public float MalwareProgress => malwareSeconds / Mathf.Max(1f, malwareFillSeconds);

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
        gameIsRunning = SceneManager.GetActiveScene().name == "SampleScene";
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameIsRunning)
            return;
        gameTime += Time.deltaTime;
        SetMalwareSeconds(malwareSeconds + Time.deltaTime);
        
        if (Keyboard.current.slashKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene("ProLobbyScene");
        }

    }

    public void AddMalware(float seconds)
    {
        if (!gameIsRunning)
            return;
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
        if (!gameIsRunning)
            return;
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

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        gameIsRunning = scene.name == "SampleScene";
        if (gameIsRunning)
            ResetGame();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            Instance = null;
        }
    }

    public float GetGameTime()
    {
        return gameTime;
    }

    public void ResetGame()
    {
        gameTime = 0;
        malwareSeconds = 0f;
        malwareLimitReached = false;
        windowsCompleted = 0;
    }

    public int GetWindowsCompleted()
    {
        return windowsCompleted;
    }

    public void CompleteWindow()
    {
        if (gameIsRunning)
            windowsCompleted++;
    }

    public void PlayWinSound()
    {
        //TODO add win sound!
        AudioSource source = GetComponent<AudioSource>();
        if (source != null && errorSound != null)
            source.PlayOneShot(winSound);
    }

    public void endGame()
    {
        if (!gameIsRunning)
            return;
        gameIsRunning = false;
        SceneManager.LoadScene("LoseScreen");
    }
}
