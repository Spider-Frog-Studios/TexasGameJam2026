using UnityEngine;
using TMPro;

public class LaserActivation : MonoBehaviour
{

    [Header("Timer Settings")]
    [SerializeField] float activationTime = 5f; // Time in seconds before the laser activates
    private bool timerIsRunning = true;

    [Header("UI Reference")]
    [SerializeField] TextMeshProUGUI timerText; // Reference to the TextMeshProUGUI component for displaying the timer
    private GameObject laser; // Reference to the laser GameObject
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        laser.SetActive(false); // Ensure the laser is initially off
    }

    // Update is called once per frame
    void Update()
    {
        if (timerIsRunning)
        {
            if (activationTime > 0)
            {
                activationTime -= Time.deltaTime;
                DisplayTime(activationTime);
            }
            else
            {
                activationTime = 0;
                timerIsRunning = false;

                // 1. Turn the laser on
                laser.SetActive(true);

                // 2. FORCE sorting order update right after activation
                SpriteRenderer sr = laser.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    sr.sortingOrder = 100; // Overrides the default window sorting layer stack
                }

                // 3. Complete your custom canvas alignment routing
                transform.parent.GetComponentInChildren<MouseGrab>().toFront();
            }
        }
    }

    public void SetLaser(GameObject laser)
    {
        this.laser = laser;
    }

    void DisplayTime(float timeToDisplay)
    {
        if (timeToDisplay < 0) timeToDisplay = 0;

        float seconds = Mathf.CeilToInt(timeToDisplay);
        timerText.text = seconds.ToString();
    }
}
