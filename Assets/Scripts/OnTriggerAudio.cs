using UnityEngine;
using System.Collections;

public class OnTriggerAudio : MonoBehaviour
{
    public string tag = "";
    public AudioSource audioSource;

    void OnTriggerEnter(Collider other)
    {
        if(tag != "")
        {
            if(!other.gameObject.CompareTag(tag))
            {
                return;
            }
        }
        audioSource.Play();
    }
}
