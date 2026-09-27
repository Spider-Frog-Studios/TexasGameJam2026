using System.Collections.Generic;
using UnityEngine;

public class SelectItemName : MonoBehaviour
{
    private string[] list =
    {
        "Texas A&M Merch",
        "iPhone Duo",
        "Cursor Plushie",
        "DDR5 Memory",
        "Protein Slop",
        "Fortnite V-Bucks",
        "VALORANT Points",
        "500 Cigarettes",
        "Reddit Gold",
        "2003 Toyota Corolla",
        "Sandpaper-Like Toilet Paper",
        "Big Ticket with Football",
        "55-Inch LG OLED TV",
        "Fridge With NO Cooling System",
        "An All-Expense-Paid Trip to Hawaii",
        "2023 Mercedes-Benz Maybach GLS 600",
        "Raising Cane's with NO Cane's Sauce",
        "A PlayStation 5 Pro with NO Games",
        "The Entire United States of America's Debt",
        "A Life-Sized Body Pillow of a Discord Moderator"
    };

    [SerializeField, Min(1)] private int startingMaxCharacters = 15;
    [SerializeField, Min(1f)] private float secondsUntilAllItemsUnlock = 300f;

    private string selectedItem;
    private string SelectWord()
    {
        float gameTime = GameManager.Instance.GetGameTime();
        float difficulty = Mathf.Clamp01(gameTime / Mathf.Max(1f, secondsUntilAllItemsUnlock));

        int shortestLength = int.MaxValue;
        int longestLength = 0;
        foreach (string item in list)
        {
            shortestLength = Mathf.Min(shortestLength, item.Length);
            longestLength = Mathf.Max(longestLength, item.Length);
        }

        int initialLimit = Mathf.Clamp(startingMaxCharacters, shortestLength, longestLength);
        int characterLimit = Mathf.FloorToInt(Mathf.Lerp(initialLimit, longestLength, difficulty));
        var eligibleItems = new List<string>();
        foreach (string item in list)
        {
            if (item.Length <= characterLimit)
            {
                eligibleItems.Add(item);
            }
        }

        return eligibleItems[Random.Range(0, eligibleItems.Count)];
    }

    public string GetSelectedItem()
    {
        if (selectedItem == null)
        {
            selectedItem = SelectWord();
        }
        
        return selectedItem;
    }
}
