using UnityEngine;
using UnityEngine.UI;

public class QuizResultWindow : MonoBehaviour
{
    [SerializeField] private Button dismissButton;

    private void Awake()
    {
        if (dismissButton == null)
            dismissButton = GetComponentInChildren<Button>(true);
        dismissButton.onClick.AddListener(Dismiss);
    }

    public void Dismiss()
    {
        Destroy(gameObject);
    }
}
