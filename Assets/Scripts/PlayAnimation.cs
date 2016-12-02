using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayAnimation : MonoBehaviour
{
    Vector3 startPos;
    Animation anim;
    List<AnimationState> animations;
    [Range(0, 5)]
    [SerializeField] float animationSpeed = 1;


    void Start()
    {
        startPos = gameObject.transform.position;
        anim = GetComponent<Animation>();
    }

    //void Update()
    //{
    //    anim[""].speed = animationSpeed;
    //    foreach(AnimationState s in animations)
    //    {
    //        s.speed = animationSpeed;
    //    }
    //}

    public void StartRun()
    {
        anim.Play();
    }

    public void BackToStart()
    {
        anim.Stop();
        transform.position = startPos;
    }
}
