using UnityEngine;
using System.Collections;

public class KidThrow : MonoBehaviour
{
    Animator anim;

    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        anim.SetTrigger("Throw");
    }

    //[ContextMenu("throw")]
    //public void AnimateSnowBall()
    //{
    //    animation.Play();
    //}
}
