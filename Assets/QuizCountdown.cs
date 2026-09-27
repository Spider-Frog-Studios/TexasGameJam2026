using UnityEngine;
using TMPro;

public class QuizCountdown : MonoBehaviour
{
    [SerializeField] private TMP_Text countdownText;
    private QuizLogic quiz;
    private string template;
    private float deadline;
    private bool running;

    public bool HasExpired => running && Time.time >= deadline;

    public void Begin(QuizLogic owner)
    {
        quiz = owner;
        if (countdownText == null)
            countdownText = GetComponent<TMP_Text>();
        template = countdownText.text;
        countdownText.raycastTarget = false;
        deadline = Time.time + 15f;
        running = true;
        UpdateLabel();
    }

    public void StopCountdown()
    {
        running = false;
    }

    private void Update()
    {
        if (!running)
            return;
        UpdateLabel();
        if (HasExpired)
        {
            running = false;
            quiz.TimeExpired();
        }
    }

    private void UpdateLabel()
    {
        string seconds = Mathf.Clamp(Mathf.CeilToInt(deadline - Time.time), 0, 15).ToString();
        countdownText.text = template.Contains("xx") ? template.Replace("xx", seconds) : seconds;
    }
}
