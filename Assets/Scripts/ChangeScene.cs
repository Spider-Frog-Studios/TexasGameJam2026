using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    [SerializeField] string nextScene;


    // Load a scene by its exact name
    public void LoadScene()
    {
        SceneManager.LoadScene(nextScene);
        GameManager.Instance.ResetGame();
    }
}