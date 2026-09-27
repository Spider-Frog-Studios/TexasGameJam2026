using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuizLogic : MonoBehaviour
{
    [SerializeField] private TMP_Text questionText;
    [SerializeField] private Button[] answerButtons = new Button[3];
    [SerializeField] private GameObject windowRoot;
    [SerializeField] private QuizCountdown countdown;
    [SerializeField] private GameObject winPrefab;
    [SerializeField] private GameObject losePrefab;
    [SerializeField, Min(0f)] private float winMalwareSeconds = 15f;
    [SerializeField, Min(0f)] private float lossMalwareSeconds = 15f;
    [SerializeField, Range(0f, 1f)] private float capitalQuestionChance = 0.5f;

    private static readonly string[,] Capitals =
    {
        { "Alabama", "Montgomery" }, { "Alaska", "Juneau" },
        { "Arizona", "Phoenix" }, { "Arkansas", "Little Rock" },
        { "California", "Sacramento" }, { "Colorado", "Denver" },
        { "Connecticut", "Hartford" }, { "Delaware", "Dover" },
        { "Florida", "Tallahassee" }, { "Georgia", "Atlanta" },
        { "Hawaii", "Honolulu" }, { "Idaho", "Boise" },
        { "Illinois", "Springfield" }, { "Indiana", "Indianapolis" },
        { "Iowa", "Des Moines" }, { "Kansas", "Topeka" },
        { "Kentucky", "Frankfort" }, { "Louisiana", "Baton Rouge" },
        { "Maine", "Augusta" }, { "Maryland", "Annapolis" },
        { "Massachusetts", "Boston" }, { "Michigan", "Lansing" },
        { "Minnesota", "Saint Paul" }, { "Mississippi", "Jackson" },
        { "Missouri", "Jefferson City" }, { "Montana", "Helena" },
        { "Nebraska", "Lincoln" }, { "Nevada", "Carson City" },
        { "New Hampshire", "Concord" }, { "New Jersey", "Trenton" },
        { "New Mexico", "Santa Fe" }, { "New York", "Albany" },
        { "North Carolina", "Raleigh" }, { "North Dakota", "Bismarck" },
        { "Ohio", "Columbus" }, { "Oklahoma", "Oklahoma City" },
        { "Oregon", "Salem" }, { "Pennsylvania", "Harrisburg" },
        { "Rhode Island", "Providence" }, { "South Carolina", "Columbia" },
        { "South Dakota", "Pierre" }, { "Tennessee", "Nashville" },
        { "Texas", "Austin" }, { "Utah", "Salt Lake City" },
        { "Vermont", "Montpelier" }, { "Virginia", "Richmond" },
        { "Washington", "Olympia" }, { "West Virginia", "Charleston" },
        { "Wisconsin", "Madison" }, { "Wyoming", "Cheyenne" }
    };

    private int correctIndex;
    private bool completed;

    private void Start()
    {
        if (answerButtons == null || answerButtons.Length != 3 ||
            answerButtons[0] == null || answerButtons[1] == null || answerButtons[2] == null)
            answerButtons = GetComponentsInChildren<Button>(true);

        if (questionText == null)
        {
            foreach (TMP_Text label in GetComponentsInChildren<TMP_Text>(true))
            {
                if (label.GetComponentInParent<Button>() == null &&
                    label.GetComponent<QuizCountdown>() == null)
                {
                    questionText = label;
                    break;
                }
            }
        }

        if (windowRoot == null)
        {
            for (Transform parent = transform; parent != null; parent = parent.parent)
            {
                MouseGrab grab = parent.GetComponentInChildren<MouseGrab>(true);
                if (grab != null)
                {
                    windowRoot = grab.transform.parent.gameObject;
                    break;
                }
            }
        }

        if (questionText == null || answerButtons.Length != 3 || windowRoot == null ||
            answerButtons[0] == answerButtons[1] || answerButtons[0] == answerButtons[2] ||
            answerButtons[1] == answerButtons[2])
        {
            Debug.LogError("QuizLogic needs a question label, three distinct buttons, and a window root.", this);
            enabled = false;
            return;
        }

        if (countdown == null)
            countdown = windowRoot.GetComponentInChildren<QuizCountdown>();

        if (countdown == null || winPrefab == null || losePrefab == null)
        {
            Debug.LogError("QuizLogic needs a QuizCountdown and both result prefabs.", this);
            enabled = false;
            return;
        }

        var labels = new TMP_Text[3];
        for (int i = 0; i < 3; i++)
        {
            labels[i] = answerButtons[i].GetComponentInChildren<TMP_Text>(true);
            if (labels[i] == null)
            {
                Debug.LogError("Each quiz button needs a child TMP text label.", answerButtons[i]);
                enabled = false;
                return;
            }
        }

        string[] answers = Random.value < capitalQuestionChance
            ? CreateCapitalQuestion() : CreateMathQuestion();
        correctIndex = Random.Range(0, 3);
        (answers[0], answers[correctIndex]) = (answers[correctIndex], answers[0]);
        questionText.raycastTarget = false;
        for (int i = 0; i < 3; i++)
        {
            int index = i;
            labels[i].text = answers[i];
            labels[i].raycastTarget = false;
            answerButtons[i].onClick.AddListener(() => ChooseAnswer(index));
        }
        countdown.Begin(this);
    }

    private string[] CreateCapitalQuestion()
    {
        int count = Capitals.GetLength(0);
        int state = Random.Range(0, count);
        int wrongA = (state + Random.Range(1, count)) % count;
        int wrongB;
        do { wrongB = Random.Range(0, count); }
        while (wrongB == state || wrongB == wrongA);
        questionText.text = $"What is the capital of {Capitals[state, 0]}?";
        return new[] { Capitals[state, 1], Capitals[wrongA, 1], Capitals[wrongB, 1] };
    }

    private string[] CreateMathQuestion()
    {
        int minutes = Mathf.Clamp(Mathf.FloorToInt(GameManager.Instance.GetGameTime() / 60f), 0, 198);
        int minimum = 1 + minutes * 2;
        int maximum = 10 + minutes * 5;
        int a = Random.Range(minimum, maximum + 1);
        int b = Random.Range(minimum, maximum + 1);
        int operation = Random.Range(0, 3);
        if (operation == 1 && b > a)
            (a, b) = (b, a); 
        int result = operation == 0 ? a + b : operation == 1 ? a - b : a * b;
        string symbol = operation == 0 ? "+" : operation == 1 ? "-" : "×";
        questionText.text = $"What is {a} {symbol} {b}?";
        int spread = Mathf.Max(3, maximum);
        int wrongA = result + Random.Range(1, spread + 1);
        int wrongB = result >= 2 ? result - Random.Range(1, Mathf.Min(result, spread) + 1)
            : wrongA + Random.Range(1, spread + 1);
        return new[] { result.ToString(), wrongA.ToString(), wrongB.ToString() };
    }

    private void ChooseAnswer(int index)
    {
        if (completed)
            return;
        // A click at or after the deadline is a loss, even before the timer's Update.
        Resolve(!countdown.HasExpired && index == correctIndex);
    }

    public void TimeExpired()
    {
        Resolve(false);
    }

    private void Resolve(bool won)
    {
        if (completed)
            return;
        completed = true;
        countdown.StopCountdown();
        foreach (Button button in answerButtons)
            button.interactable = false;

        // Use the current position so the result follows windows that were dragged.
        Instantiate(won ? winPrefab : losePrefab,
            windowRoot.transform.position, windowRoot.transform.rotation, windowRoot.transform.parent);
        if (won)
        {
            GameManager.Instance.SubtractMalware(winMalwareSeconds);
            GameManager.Instance.CompleteWindow();
        }
        else
            GameManager.Instance.AddMalware(lossMalwareSeconds);
        Destroy(windowRoot);
    }
}
