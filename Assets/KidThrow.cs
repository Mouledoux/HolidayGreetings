using UnityEngine;
using System.Collections;

public class KidThrow : MonoBehaviour
{
    Animator anim;
    public Animation animation;

    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        anim.SetTrigger("Throw");
    }

    [ContextMenu("throw")]
    public void AnimateSnowBall()
    {
        animation.Play();
    }
}
