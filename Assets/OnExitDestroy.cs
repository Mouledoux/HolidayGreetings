using UnityEngine;
using System.Collections;

public class OnExitDestroy : MonoBehaviour
{
    void OnTriggerExit(Collider other)
    {
        gameObject.SetActive(false);
    }
}
