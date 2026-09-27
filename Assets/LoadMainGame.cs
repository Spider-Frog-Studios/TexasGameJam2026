using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class LoadMainGame : MonoBehaviour
{
    
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(11.5f);
        SceneManager.LoadScene("SampleScene");
    }
}
