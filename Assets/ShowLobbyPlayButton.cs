using System.Collections;
using UnityEngine;

public class ShowLobbyPlayButton : MonoBehaviour
{
    [SerializeField] private GameObject objectToShow;
    [SerializeField] private GameObject objectToHide;

    private IEnumerator Start()
    {
        if (objectToShow == null)
        {
            Debug.LogError("Assign the lobby popup to Object To Show.", this);
            yield break;
        }

        objectToShow.SetActive(false);
        yield return new WaitForSeconds(6f);
        GetComponent<AudioSource>().Play();
        objectToShow.SetActive(true);
        yield return new WaitForSeconds(6f);
        if (objectToHide != null)
            objectToHide.SetActive(false);
    }
}
