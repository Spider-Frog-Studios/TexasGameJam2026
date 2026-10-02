using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CaptchaLogic : MonoBehaviour
{
    [SerializeField] private Button buttonR0C0;
    [SerializeField] private Button buttonR0C1;
    [SerializeField] private Button buttonR0C2;
    [SerializeField] private Button buttonR1C0;
    [SerializeField] private Button buttonR1C1;
    [SerializeField] private Button buttonR1C2;
    [SerializeField] private Button buttonR2C0;
    [SerializeField] private Button buttonR2C1;
    [SerializeField] private Button buttonR2C2;
    [SerializeField] private Sprite[] captchaImages;
    [SerializeField] private Sprite[] fakeImages;
    [SerializeField] private Color selectedColor = new Color(0.45f, 1f, 0.45f, 1f);
    [SerializeField] private GameObject windowRoot;
    [SerializeField] private TMP_Text countdownText;
    private int[,] captcha = new int[3,3];
    private int[,] inputAnswers = new int[3,3];
    private Button[,] buttons;
    private SpriteRenderer[,] tileImages = new SpriteRenderer[3,3];
    private Color[,] originalColors = new Color[3,3];
    private bool ready;
    private bool submitted;
    private float deadline;
    private int displayedSeconds = -1;
    private string countdownTemplate;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        buttons = new Button[3, 3]
        {
            { buttonR0C0, buttonR0C1, buttonR0C2 },
            { buttonR1C0, buttonR1C1, buttonR1C2 },
            { buttonR2C0, buttonR2C1, buttonR2C2 }
        };

        // This script lives on the Canvas inside the minigame window.
        if (windowRoot == null)
            windowRoot = transform.parent != null ? transform.parent.gameObject : gameObject;

        if (captchaImages == null || captchaImages.Length == 0 ||
            fakeImages == null || fakeImages.Length == 0 ||
            System.Array.Exists(captchaImages, sprite => sprite == null) ||
            System.Array.Exists(fakeImages, sprite => sprite == null))
        {
            Debug.LogError("CaptchaLogic needs both sprite lists filled with valid sprites.", this);
            return;
        }

        foreach (Button button in buttons)
        {
            if (button == null ||
                (button.GetComponentInChildren<SpriteRenderer>(true) == null && button.image == null))
            {
                Debug.LogError("Each captcha cell needs a button and a SpriteRenderer or UI Image.", this);
                return;
            }
        }

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                int randomInt = UnityEngine.Random.Range(0, 2);
                captcha[i, j] = randomInt;
                // 0 = selected, 1 = unselected, matching the captcha answer grid.
                inputAnswers[i, j] = 1;
                Sprite[] images = randomInt == 0 ? captchaImages : fakeImages;
                Button button = buttons[i, j];
                Sprite sprite = images[UnityEngine.Random.Range(0, images.Length)];
                SpriteRenderer tileImage = button.GetComponentInChildren<SpriteRenderer>(true);
                tileImages[i, j] = tileImage;
                if (tileImage != null)
                {
                    tileImage.sprite = sprite;
                    originalColors[i, j] = tileImage.color;
                    FitHitboxToSprite(button, tileImage);
                }
                else if (button.image != null)
                {
                    button.image.sprite = sprite;
                    originalColors[i, j] = button.image.color;
                }
            }
        }
        deadline = Time.time + 20f;
        if (countdownText == null)
        {
            Transform label = transform.Find("CountdownText");
            if (label != null)
                countdownText = label.GetComponent<TMP_Text>();
        }
        if (countdownText != null)
        {
            countdownTemplate = countdownText.text;
            countdownText.raycastTarget = false;
        }
        ready = true;
        UpdateCountdownText();
    }

    void Update()
    {
        if (!ready || submitted)
            return;

        UpdateCountdownText();
        if (Time.time >= deadline)
            ResolveCaptcha(false);
    }

    private void UpdateCountdownText()
    {
        int seconds = Mathf.Clamp(Mathf.CeilToInt(deadline - Time.time), 0, 20);
        if (seconds == displayedSeconds)
            return;

        displayedSeconds = seconds;
        if (countdownText != null)
            countdownText.text = countdownTemplate.Contains("xx")
                ? countdownTemplate.Replace("xx", seconds.ToString()) : seconds.ToString();
    }

    private void FitHitboxToSprite(Button button, SpriteRenderer tileImage)
    {
        // The original button Image is larger than its child sprite and can
        // intercept clicks intended for neighboring rows.
        foreach (Graphic graphic in button.GetComponentsInChildren<Graphic>(true))
            graphic.raycastTarget = false;

        // A transparent UI Image receives clicks and bubbles them to the Button.
        // Parenting it to the artwork keeps its bounds aligned when moved/scaled.
        GameObject hitbox = new GameObject("Captcha Hitbox", typeof(RectTransform), typeof(Image));
        hitbox.layer = button.gameObject.layer;
        RectTransform rect = hitbox.GetComponent<RectTransform>();
        rect.SetParent(tileImage.transform, false);
        rect.anchorMin = rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        Bounds bounds = tileImage.localBounds;
        rect.sizeDelta = new Vector2(bounds.size.x, bounds.size.y);
        rect.localPosition = bounds.center;

        Image hitImage = hitbox.GetComponent<Image>();
        hitImage.color = Color.clear;
        hitImage.raycastTarget = true;
    }
    
    public void selectCaptchaCell(int index)
    {
        if (index < 0 || index >= 9)
            return;
        updateUserCaptchaInput(index / 3, index % 3);
    }

    public void updateUserCaptchaInput(int row, int col)
    {
        if (!ready || submitted || row < 0 || row >= 3 || col < 0 || col >= 3)
            return;

        if (inputAnswers[row, col] == 0)
        {
            inputAnswers[row, col] = 1;
        }
        else
        {
            inputAnswers[row, col] = 0;
        }

        Color tint = originalColors[row, col];
        if (inputAnswers[row, col] == 0)
        {
            tint *= selectedColor;
            tint.a = originalColors[row, col].a;
        }

        if (tileImages[row, col] != null)
            tileImages[row, col].color = tint;
        else
            buttons[row, col].image.color = tint;
    }

    public void submitCaptcha()
    {
        // A submission at the deadline is a timeout, even before Update runs.
        ResolveCaptcha(Time.time < deadline && isCaptchaSame(captcha, inputAnswers));
    }

    private void ResolveCaptcha(bool won)
    {
        if (!ready || submitted)
            return;
        submitted = true;
        if (GameManager.Instance == null)
        {
            Debug.LogError("CaptchaLogic needs a GameManager in the scene to submit.", this);
        }
        else if (won)
        {
            GameManager.Instance.SubtractMalware(15f);
            GameManager.Instance.CompleteWindow();
            GameManager.Instance.PlayWinSound();
        }
        else
        {
            GameManager.Instance.AddMalware(15f);
        }
        Destroy(windowRoot);
    }

    private bool isCaptchaSame(int[,] captcha, int[,] inputAnswers)
    {
        for (int i = 0; i < captcha.GetLength(0); i++)
        {
            for (int j = 0; j < captcha.GetLength(1); j++)
            {
                if (captcha[i, j] != inputAnswers[i, j])
                {
                    return false;
                }
            }
        }
        
        return true;
    }
}
