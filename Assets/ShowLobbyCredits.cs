using UnityEngine;

public class ShowLobbyCredits : MonoBehaviour
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

    public void quitApplication()
    {
        Application.Quit();
    }
}
