using System;
using UnityEngine;

public class showStatPanel : MonoBehaviour
{
    bool IsShown = false;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void ToggleStatsPanel()
    {
        if (IsShown)
        {
            gameObject.SetActive(false);
            IsShown = false;
        }
        else
        {
            gameObject.SetActive(true);
            IsShown = true;
        }
        
    }
}
