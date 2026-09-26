using UnityEngine;

public class GameManager : MonoBehaviour
{

    private float gameTime = 0f;
    private int windowsCompleted;
    public static GameManager Instance;

    void Awake()
    {
        Instance =  this;
    }

    // Update is called once per frame
    void Update()
    {
        gameTime += Time.deltaTime;
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
