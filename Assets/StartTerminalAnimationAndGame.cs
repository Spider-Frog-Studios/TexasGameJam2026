using System.Collections;
using UnityEngine;

public class StartTerminalAnimationAndGame : MonoBehaviour
{
    [SerializeField] private GameObject objectToHide;
    [SerializeField] private GameObject terminalPrefab;
    [SerializeField] private Vector3 terminalPosition = new Vector3(-1.35874f, -0.16046f, 0f);

    private bool transitionStarted;

    public void beginTransition()
    {
        if (transitionStarted)
            return;

        if (objectToHide == null || terminalPrefab == null)
        {
            Debug.LogError("Assign the popup and terminal prefab before starting the lobby transition.", this);
            return;
        }

        transitionStarted = true;
        StartCoroutine(ShowTerminalAfterDelay());
    }

    private IEnumerator ShowTerminalAfterDelay()
    {
        objectToHide.SetActive(false);
        yield return new WaitForSeconds(2f);

        GameObject terminal = Instantiate(terminalPrefab, terminalPosition, Quaternion.identity);
        terminal.transform.localScale = Vector3.one;
        terminal.SetActive(true);
    }
}
