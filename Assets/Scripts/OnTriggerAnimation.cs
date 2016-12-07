using UnityEngine;
using System.Collections;

public class OnTriggerAnimation : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        GetComponent<Animation>().Play();
    }
}