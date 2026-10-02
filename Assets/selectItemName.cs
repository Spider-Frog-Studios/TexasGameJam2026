using System.Collections.Generic;
using UnityEngine;

public class SelectItemName : MonoBehaviour
{
    private string[] list =
    {
        "SAT test",
        "A&M merch",
        "iPhone Duo",
        "Cursor plushie",
        "DDR5 memory",
        "Protein slop",
        "Fortnite V-Bucks",
        "Windows license",
        "Oracle stocks",
        "VALORANT Points",
        "500 cigarettes",
        "Reddit Gold",
        "Discord Nitro",
        "Freemium currency",
        "Krispy Kreme donut",
        "Krispy Kreme donut",
        "Cowboys Superbowl merch",
    };

    [SerializeField, Min(1)] private int startingMaxCharacters = 12;
    [SerializeField, Min(1f)] private float secondsUntilAllItemsUnlock = 250;

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
