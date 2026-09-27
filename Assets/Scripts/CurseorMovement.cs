using UnityEngine;

public class CurseorMovement : MonoBehaviour
{
    private float smoothTime = 0.5f;
    private float speedMultiplier = 1f;
    private float currentSpeed = 3f;
    private float speedIncreasePerSec = 0.1f;
    private float arrivalDistance = 0.5f;
    private Vector2 currentTargetWorld;
    private Vector2 velocity = Vector2.zero;
    private Camera mainCamera;
    private bool grabbingNewWindow = false;
    private bool grabbedWindow = false;
    [SerializeField] Transform[] windowSpawnLocations;
    [SerializeField] GameObject windowSpawner;
    [SerializeField] GameObject popUpSpawner;
    private GameObject currentPopUp;
    private GameObject currentWindow;
    private Transform spawnLocation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = Camera.main;
        PickRandomViewportTarget();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.SmoothDamp(transform.position, currentTargetWorld, ref velocity, smoothTime, currentSpeed * speedMultiplier);
        if(Vector2.Distance(transform.position, currentTargetWorld) < arrivalDistance)
        {
            if (grabbingNewWindow)
            {
                GrabNewWindow();
            } else
            {
                if(grabbedWindow && currentWindow != null)
                {
                    currentWindow.GetComponentInChildren<MouseGrab>().StopDragging();
                    currentWindow = null;
                    grabbedWindow = false;
                }
                PickRandomViewportTarget();
            }
        }
        currentSpeed += speedIncreasePerSec * Time.deltaTime;
    }

    void PickRandomViewportTarget()
    {
        if (mainCamera == null)
        {
            return;
        }

        if(currentWindow == null)
        {
            grabbedWindow = false;
        }

        float randomX = Random.Range(.2f, .8f);
        float randomY = Random.Range(.2f, .8f);
        Vector3 worldPoint = mainCamera.ViewportToWorldPoint(new Vector3(randomX, randomY, mainCamera.nearClipPlane));
        currentTargetWorld = new Vector2(worldPoint.x, worldPoint.y);

        int randomIndex = Random.Range(0, 10);

        if(popUpSpawner != null && randomIndex <= 0 && currentPopUp == null)
        {
            currentPopUp = popUpSpawner.GetComponent<SpawnPopUp>().CreatePopUp();
        }

        if (!grabbedWindow && randomIndex <= 4 && windowSpawnLocations.Length > 0)
        {
            SetUpNextTargetAsWindow();
        }
    }

    void SetUpNextTargetAsWindow()
    {
        grabbingNewWindow = true;
        int randomIndex = Random.Range(0, windowSpawnLocations.Length);
        spawnLocation = windowSpawnLocations[randomIndex];

        if(spawnLocation != null)
        {
            currentTargetWorld = new Vector2(spawnLocation.position.x, spawnLocation.position.y);
        }
    }

    void GrabNewWindow()
    {
        if (spawnLocation != null && windowSpawner != null && currentWindow == null)
        {
            currentWindow = windowSpawner.GetComponent<SpawnWindow>().CreateWindow(spawnLocation.position);
            if(currentWindow != null)
            {
                currentWindow.GetComponentInChildren<MouseGrab>().StartDragging(this.transform);
                grabbedWindow = true;
            }
        }

        grabbingNewWindow = false;
        PickRandomViewportTarget();
    }
}
