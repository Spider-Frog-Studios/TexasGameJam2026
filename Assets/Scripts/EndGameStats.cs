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
            timeText.text = GameManager.Instance.GetGameTime().ToString();
            windowsWonText.text = GameManager.Instance.GetWindowsCompleted().ToString();
        }
    }
}
