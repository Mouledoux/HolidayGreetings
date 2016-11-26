using UnityEngine;
using System.Collections;

public class ReactAnimation : MonoBehaviour
{
    Animation anim;

    void OnTriggerEnter()
    {
        anim.Play();
    }
}