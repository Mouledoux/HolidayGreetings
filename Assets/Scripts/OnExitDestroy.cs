using UnityEngine;
using System.Collections.Generic;

public class OnExitDestroy : MonoBehaviour
{
    public List<GameObject> Characters;

    void OnEnable()
    {
        foreach (GameObject go in Characters)
        {
            go.SetActive(false);
        }
    }

    void OnTriggerExit(Collider other)
    {
        gameObject.SetActive(false);

        foreach(GameObject go in Characters)
        {
            go.SetActive(true);
        }
    }
}
