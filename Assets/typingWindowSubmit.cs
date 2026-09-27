using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class typingWindowSubmit : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private GameObject windowRoot;
    [SerializeField] private Button submitButton;
    [SerializeField] private windowCountdownText countdown;
    private SelectItemName selector;
    private Coroutine errorFlash;
    private ColorBlock originalColors;
    private bool completed;
    
    private void Awake()
    {
        selector = GetComponentInParent<SelectItemName>();
        if (submitButton == null)
            submitButton = GetComponentInChildren<Button>();
        if (countdown == null && windowRoot != null)
            countdown = windowRoot.GetComponentInChildren<windowCountdownText>();
        if (submitButton != null)
            originalColors = submitButton.colors;
    }
    
    public void SubmitAnswer()
    {
        if (completed)
            return;
        string requiredMessage = "Abort " + selector.GetSelectedItem() + " Purchase";

        if (string.Equals(inputField.text, requiredMessage,
                System.StringComparison.Ordinal))
        {
            if (countdown != null && !countdown.TryComplete())
                return;
            completed = true;
            Debug.Log("Correct!");

            GameManager.Instance.CompleteWindow();
            Destroy(windowRoot);
        }
        else
        {
            Debug.Log("Incorrect. Try again!");
            if (submitButton != null)
            {
                if (errorFlash != null)
                    StopCoroutine(errorFlash);
                errorFlash = StartCoroutine(FlashIncorrect());
            }
            inputField.ActivateInputField();
        }
    }

    private IEnumerator FlashIncorrect()
    {
        ColorBlock redColors = originalColors;
        redColors.normalColor = Color.red;
        redColors.highlightedColor = Color.red;
        redColors.pressedColor = Color.red;
        redColors.selectedColor = Color.red;
        submitButton.colors = redColors;
        yield return new WaitForSeconds(1f);
        submitButton.colors = originalColors;
        errorFlash = null;
    }

    private void OnDisable()
    {
        if (errorFlash != null)
        {
            StopCoroutine(errorFlash);
            errorFlash = null;
            if (submitButton != null)
                submitButton.colors = originalColors;
        }
    }
}
