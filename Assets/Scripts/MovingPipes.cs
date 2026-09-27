using UnityEngine;

public class MovingPipes : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Height Variance")]
    [Tooltip("The maximum distance up or down the pipe can randomly shift on spawn.")]
    [SerializeField] private float heightOffsetRange = 3f;

    private bool isGameOver = false;

    void Start()
    {
        // Randomize the height exactly once right when the game starts
        float initialYPosition = transform.position.y;
        float randomY = initialYPosition + Random.Range(-heightOffsetRange, heightOffsetRange);

        transform.position = new Vector3(transform.position.x, randomY, transform.position.z);
    }

    void Update()
    {
        if (isGameOver) return;

        // Move the pipe continuously to the left
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime, Space.World);
    }

    /// <summary>
    /// This is called automatically by your PipeChildCollision script.
    /// </summary>
    public void NotifyPipeCollision(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player hit a pipe! Game Over.");
            isGameOver = true;
        }
    }
}
