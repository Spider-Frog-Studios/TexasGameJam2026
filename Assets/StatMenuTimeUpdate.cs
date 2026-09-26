using TMPro;
using UnityEngine;

public class StatMenuTimeUpdate : MonoBehaviour
{
    
    public TextMeshProUGUI text;
    GameManager gameManager;
    void Update()
    {
        text.text = GameManager.Instance.GetGameTime().ToString("F1");
    }
}
