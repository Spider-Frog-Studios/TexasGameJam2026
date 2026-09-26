using UnityEngine;
using TMPro;
using System.Collections;

public class threeDotAnimation : MonoBehaviour
{
    public TMP_Text loadingText;
    
    private string text= "CORRUPTING DATA";
    private CorruptionProgress corruptionProgress;
    
    void Start()
    {
        StartCoroutine(TypeText());
        corruptionProgress = GetComponentInParent<CorruptionProgress>();
    }
    
    IEnumerator TypeText()
    {
        loadingText.text = "";
        for (int i = 1; i <= text.Length; i++)
        {
            loadingText.text = text.Substring(0, i);

            yield return new WaitForSeconds(0.25f);
        }

        while (true)
        {
            if (corruptionProgress.GetProgress() >= 0.75)
            {
                loadingText.text = "C0RRUPTING D4TA";
            }
            else
            {
                loadingText.text = text;
            }
            yield return new WaitForSeconds(0.5f);
            
            if (corruptionProgress.GetProgress() >= 0.75)
            {
                loadingText.text = "CoRRUPT1NG DAT4.";
            }
            else
            {
                loadingText.text = text + ".";
            }
            yield return new WaitForSeconds(0.5f);
            
            if (corruptionProgress.GetProgress() >= 0.75)
            {
                loadingText.text = "¢0RRUPT1NG DATA..";
            }
            else
            {
                loadingText.text = text + "..";
            }
            yield return new WaitForSeconds(0.5f);

            if (corruptionProgress.GetProgress() >= 0.75)
            {
                loadingText.text = "C0RRUPT1N6 D4T4...";
            }
            else
            {
                loadingText.text = text + "...";
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
}
