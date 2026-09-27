using UnityEngine;
using System.Collections;


public class showTerminal : MonoBehaviour
{
    [SerializeField] private GameObject objectToShow;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(9f);
        objectToShow.SetActive(false);
    }
}
