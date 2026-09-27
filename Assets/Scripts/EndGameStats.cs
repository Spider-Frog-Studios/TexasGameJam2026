using UnityEngine;
using TMPro;

public class EndGameStats : MonoBehaviour
{
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text windowsWonText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (GameManager.Instance != null)
        {
            int time = (int)GameManager.Instance.GetGameTime();
            timeText.text = time.ToString() + " Secs";
            windowsWonText.text = GameManager.Instance.GetWindowsCompleted().ToString();
        }
    }
}
