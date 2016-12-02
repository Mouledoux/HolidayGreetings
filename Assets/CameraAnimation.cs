using UnityEngine;
using System.Collections;

public class CameraAnimation : MonoBehaviour
{
    Animation anim;
    public AnimationClip vr;
    public AnimationClip full;

    void Start()
    {
        anim = gameObject.GetComponent<Animation>();
    }

    public void PlayVR()
    {

        
    }

    public void PlayFull()
    {
        anim.Play(full.name);
    }
}
