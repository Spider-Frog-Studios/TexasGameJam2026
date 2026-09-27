using UnityEngine;
using UnityEngine.InputSystem;

public class ParrySkillCheck : MonoBehaviour
{
    [Header("Object Assignments")]
    [Tooltip("Drag the parent parryCursor here.")]
    [SerializeField] private Transform playerCursor;

    [Tooltip("Drag the parent parryTarget here.")]
    [SerializeField] private Transform targetZone;

    [Header("Speed Settings")]
    [Tooltip("Rotation speed in degrees per second.")]
    [SerializeField] private float minRotationSpeed = 150f;
    [SerializeField] private float maxRotationSpeed = 300f;
    private float rotationSpeed;

    [Tooltip("True = Clockwise, False = Counter-Clockwise")]
    [SerializeField] private bool clockwise;

    [Header("Spawn Settings")]
    [Tooltip("How long the pop-in animation takes in seconds.")]
    [SerializeField] private float spawnDuration = 0.3f;

    [Tooltip("The targeted full size scale of your window.")]
    [SerializeField] private Vector3 fullScale;

    [Header("Malware (seconds)")]
    [SerializeField, Min(0f)] private float successRewardSeconds = 15f;
    [SerializeField, Min(0f)] private float failurePenaltySeconds = 15f;

    private float currentZAngle = 0f;
    private bool gameIsRunning = false;

    private Collider2D cursorColliderChild;
    private Collider2D targetColliderChild;

    void Awake()
    {
        // Find colliders early so they are ready before animations fire
        cursorColliderChild = playerCursor.GetComponentInChildren<Collider2D>();
        targetColliderChild = targetZone.GetComponentInChildren<Collider2D>();
        rotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed); // Randomize rotation speed for variety
        int randomDirection = Random.Range(0, 2); // 0 or 1
        clockwise = randomDirection == 0;
    }

    void OnEnable()
    {
        // 1. Force the window to be invisible/tiny the instant it wakes up
        transform.parent.gameObject.transform.localScale = Vector3.zero;

        // 2. Trigger a fresh parry placement layout
        SetupNewParry();

        // 3. Play the spawn pop-in animation
        StartCoroutine(GrowAndSpawn(transform.parent.gameObject, spawnDuration));
    }

    void Update()
    {
        if (!gameIsRunning) return;

        // Rotate the parent cursor smoothly over time
        float directionFactor = clockwise ? -1f : 1f;
        currentZAngle += directionFactor * rotationSpeed * Time.deltaTime;
        currentZAngle = Mathf.Repeat(currentZAngle, 360f);
        playerCursor.localRotation = Quaternion.Euler(0f, 0f, currentZAngle);

        // Check for Input (Left Mouse Button / M1)
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            CheckParryResult();
        }
    }

    public void SetupNewParry()
    {
        float randomTargetAngle = Random.Range(0f, 360f);
        targetZone.localRotation = Quaternion.Euler(0f, 0f, randomTargetAngle);

        currentZAngle = 0f;
        playerCursor.localRotation = Quaternion.Euler(0f, 0f, currentZAngle);
    }

    private void CheckParryResult()
    {
        gameIsRunning = false;

        if (cursorColliderChild != null && targetColliderChild != null)
        {
            // Evaluate the positions drawn this frame, before the next physics tick.
            Physics2D.SyncTransforms();
            if (cursorColliderChild.Distance(targetColliderChild).isOverlapped)
            {
                ResolveSuccess();
            }
            else
            {
                ResolveFailure();
            }
        }
    }

    private void ResolveSuccess()
    {
        Debug.Log("PERFECT PARRY: Colliders overlapped successfully!");
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SubtractMalware(successRewardSeconds);
            GameManager.Instance.CompleteWindow();
        }
        StartCoroutine(ShrinkAndDestroy(transform.parent.gameObject, 0.4f));
    }

    private void ResolveFailure()
    {
        Debug.Log("FAILED: Colliders were not overlapping!");
        if (GameManager.Instance != null)
            GameManager.Instance.AddMalware(failurePenaltySeconds);
        StartCoroutine(ShrinkAndDestroy(transform.parent.gameObject, 0.4f));
    }

    // --- ANIMATION COROUTINES ---

    private System.Collections.IEnumerator GrowAndSpawn(GameObject targetWindow, float duration)
    {
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            float t = timeElapsed / duration;

            // Smooth-step interpolation formula for a satisfying ease-out pop
            t = t * t * (3f - 2f * t);

            targetWindow.transform.localScale = Vector3.Lerp(Vector3.zero, fullScale, t);
            yield return null;
        }

        targetWindow.transform.localScale = fullScale;

        // Only start moving the cursor needle AFTER the pop-in animation is fully complete
        gameIsRunning = true;
    }

    private System.Collections.IEnumerator ShrinkAndDestroy(GameObject targetWindow, float duration)
    {
        Vector3 initialScale = targetWindow.transform.localScale;
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            float t = timeElapsed / duration;
            t = t * t * (3f - 2f * t);

            targetWindow.transform.localScale = Vector3.Lerp(initialScale, Vector3.zero, t);
            yield return null;
        }

        targetWindow.transform.localScale = Vector3.zero;
        Destroy(targetWindow);
    }
}
