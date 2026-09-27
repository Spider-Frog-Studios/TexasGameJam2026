using TMPro;
using UnityEngine;

public class selectName : MonoBehaviour
{
    private SelectItemName selector;
    public TextMeshProUGUI text;
    void Start()
    {
        selector = GetComponentInParent<SelectItemName>();

        string word = selector.GetSelectedItem();
        text.text = word;
    }
    
}
