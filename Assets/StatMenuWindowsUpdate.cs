using TMPro;
using UnityEngine;

public class StatMenuWindowsUpdate : MonoBehaviour
{
    public TextMeshProUGUI text;
    GameManager gameManager;
    void Update()
    {
        text.text = GameManager.Instance.GetWindowsCompleted().ToString();
    }
}
