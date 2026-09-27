using UnityEngine;
using TMPro;

public class windowCountdownText : MonoBehaviour
{
    [SerializeField] private TMP_Text countdownText;
    [SerializeField] private GameObject windowRoot;
    [SerializeField, Min(0f)] private float timeoutMalwareSeconds = 15f;

    private string textTemplate;
    private float deadline;
    private bool finished;

    private void Start()
    {
        if (countdownText == null)
            countdownText = GetComponent<TMP_Text>();
        textTemplate = countdownText.text;

        int elapsedMinutes = Mathf.FloorToInt(GameManager.Instance.GetGameTime() / 60f);
        int duration = Mathf.Max(15, 30 - elapsedMinutes * 3);
        deadline = Time.time + duration;
        UpdateLabel();
    }

    private void Update()
    {
        if (finished)
            return;
        UpdateLabel();
        if (Time.time >= deadline)
            Expire();
    }

    private void UpdateLabel()
    {
        string seconds = Mathf.Clamp(Mathf.CeilToInt(deadline - Time.time), 0, 99).ToString();
        countdownText.text = textTemplate.Contains("xx")
            ? textTemplate.Replace("xx", seconds)
            : seconds;
    }

    // Resolve the deadline here too, so a click cannot win after time has expired.
    public bool TryComplete()
    {
        if (finished)
            return false;
        if (Time.time >= deadline)
        {
            Expire();
            return false;
        }
        finished = true;
        return true;
    }

    private void Expire()
    {
        if (finished)
            return;
        finished = true;
        GameManager.Instance.AddMalware(timeoutMalwareSeconds);
        Destroy(windowRoot);
    }
}
