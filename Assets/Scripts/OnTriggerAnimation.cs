using UnityEngine;
using System.Collections;

public class OnTriggerAnimation : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
            GetComponent<Animation>().Play();
    }
}