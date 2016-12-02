using UnityEngine;
using System.Collections;

public class CameraAnimation : MonoBehaviour
{
    Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void PlayMenuCamera(bool vr)
    {
        anim.enabled = true;

        anim.SetBool("VR", vr);
    }
}
