using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLobbyScene : MonoBehaviour

{
    [SerializeField] private Animator fadeAnimator;
    private bool isLoading;

    public void loadLobby()
    {
        if (isLoading)
            return;

        if (fadeAnimator == null)
        {
            Debug.LogError("Assign the fade Image's Animator to Fade Animator.", this);
            return;
        }

        isLoading = true;
        fadeAnimator.Play("Base Layer.FadeBlack", 0, 0f);
        Invoke(nameof(OpenLobby), 1.5f);
    }
    
    private void OpenLobby()
    {
        SceneManager.LoadScene("LobbyScene");
    }
}
