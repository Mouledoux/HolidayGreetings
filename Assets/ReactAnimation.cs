using UnityEngine;
using System.Collections;

public class ReactAnimation : MonoBehaviour
{
    Animator anim;

    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    void OnTriggerEnter()
    {
        anim.SetTrigger("Look");
    }
}