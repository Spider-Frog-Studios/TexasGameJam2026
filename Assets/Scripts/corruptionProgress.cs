using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class CorruptionProgress : MonoBehaviour
{
    public Image progressBar;
    public TMP_Text percentageText;

    private GameManager manager;
    private Coroutine flash;
    private Color originalColor;

    private void Awake()
    {
        if (progressBar != null)
            originalColor = progressBar.color;
    }

    private void LateUpdate()
    {
        // Bind after all Awake methods, and reconnect when the taskbar is re-enabled.
        if (manager == null && GameManager.Instance != null)
        {
            manager = GameManager.Instance;
            manager.MalwareAdjusted += OnMalwareAdjusted;
        }
        UpdateUI();
    }

    private void UpdateUI()
    {
        float progress = GetProgress();
        if (progressBar != null)
            progressBar.fillAmount = progress;
        if (percentageText != null)
            percentageText.text = Mathf.RoundToInt(progress * 100f) + "%";
    }

    public float GetProgress()
    {
        return GameManager.Instance != null ? GameManager.Instance.MalwareProgress : 0f;
    }

    public void AddTime(float amount)
    {
        GameManager.Instance.AddMalware(amount);
    }

    public void SubtractTime(float amount)
    {
        GameManager.Instance.SubtractMalware(amount);
    }

    [ContextMenu("Add Progress")]
    private void Add10Seconds()
    {
        AddTime(30f);
    }

    [ContextMenu("Subtract Progress")]
    private void Subtract10Seconds()
    {
        SubtractTime(30f);
    }

    private IEnumerator FlashColor(Color color)
    {
        progressBar.color = color;
        yield return new WaitForSeconds(0.5f);
        progressBar.color = originalColor;
        yield return new WaitForSeconds(0.15f);
        progressBar.color = color;
        yield return new WaitForSeconds(0.3f);
        progressBar.color = originalColor;
        flash = null;
    }

    private void OnMalwareAdjusted(float seconds)
    {
        UpdateUI();
        if (progressBar == null)
            return;
        if (flash != null)
            StopCoroutine(flash);
        flash = StartCoroutine(FlashColor(seconds > 0f ? Color.red : Color.blue));
    }

    private void OnDisable()
    {
        if (manager != null)
            manager.MalwareAdjusted -= OnMalwareAdjusted;
        manager = null;
        if (flash != null)
            StopCoroutine(flash);
        flash = null;
        if (progressBar != null)
            progressBar.color = originalColor;
    }
}
