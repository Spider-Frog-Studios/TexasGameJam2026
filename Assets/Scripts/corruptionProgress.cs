using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class CorruptionProgress : MonoBehaviour
{
    public Image progressBar;
    public TMP_Text percentageText;

    public float totalTime = 180f;
    private float currentTime = 0f;

    void Update()
    {
        if (currentTime >= totalTime)
        {
            Debug.Log("LOAD GAME OVER SCENE!");
        }
        currentTime += Time.deltaTime;
        currentTime = Mathf.Clamp(currentTime, 0f, totalTime);

        UpdateUI();
    }

    private void UpdateUI()
    {
        float progress = currentTime / totalTime;

        progressBar.fillAmount = progress;

        percentageText.text =
            Mathf.RoundToInt(progress * 100f) + "%";
    }

    public float GetProgress()
    {
        return currentTime / totalTime;
    }

    public void AddTime(float amount)
    {
        currentTime += amount;
        currentTime = Mathf.Clamp(currentTime, 0f, totalTime);
        StartCoroutine(FlashColor(Color.red));
    }

    public void SubtractTime(float amount)
    {
        currentTime -= amount;
        currentTime = Mathf.Clamp(currentTime, 0f, totalTime);
        StartCoroutine(FlashColor(Color.blue));
        UpdateUI();
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
        Color original = progressBar.color;
        progressBar.color = color;
        yield return new WaitForSeconds(0.5f);
        progressBar.color = original;
        yield return new WaitForSeconds(0.15f);
        progressBar.color = color;
        yield return new WaitForSeconds(0.3f);
        progressBar.color = original;;
    }
}