using TMPro;
using UnityEngine;

public class selectItemNameAbort : MonoBehaviour
{
    private SelectItemName selector;
    public TextMeshProUGUI text;
    void Start()
    {
        selector = GetComponentInParent<SelectItemName>();

        string word = selector.GetSelectedItem();
        text.text = "\"Cancel " + word + "\"";
    }
}
