using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class typingWindowSubmit : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private GameObject windowRoot;
    [SerializeField] private Button submitButton;
    [SerializeField] private windowCountdownText countdown;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip closeClip;
    private SelectItemName selector;
    private Coroutine errorFlash;
    private Coroutine resumeTyping;
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
    
    private void OnEnable()
    {
        if (inputField != null)
            inputField.onSelect.AddListener(ResumeTyping);
    }

    private void ResumeTyping(string text)
    {
        if (resumeTyping != null)
            StopCoroutine(resumeTyping);
        resumeTyping = StartCoroutine(MoveCaretToEnd());
    }

    private IEnumerator MoveCaretToEnd()
    {
        // Wait for TMP's activation and pointer handling to finish positioning the caret.
        yield return null;
        if (inputField != null && inputField.isFocused)
            inputField.MoveTextEnd(false);
        resumeTyping = null;
    }

    public void SubmitAnswer()
    {
        if (completed)
            return;
        string requiredMessage = "Cancel " + selector.GetSelectedItem();

        if (string.Equals(inputField.text, requiredMessage,
                System.StringComparison.Ordinal))
        {
            if (countdown != null && !countdown.TryComplete())
                return;
            completed = true;

            GameManager.Instance.PlayWinSound();
            GameManager.Instance.CompleteWindow();
            Destroy(windowRoot);
        }
        else
        {
            if (audioSource != null && closeClip != null)
                audioSource.PlayOneShot(closeClip);
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
        if (inputField != null)
            inputField.onSelect.RemoveListener(ResumeTyping);
        if (resumeTyping != null)
        {
            StopCoroutine(resumeTyping);
            resumeTyping = null;
        }
        if (errorFlash != null)
        {
            StopCoroutine(errorFlash);
            errorFlash = null;
            if (submitButton != null)
                submitButton.colors = originalColors;
        }
    }
}
