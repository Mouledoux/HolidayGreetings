using UnityEngine;
using System.Collections;

public class PlayAnimation : MonoBehaviour
{
    Vector3 startPos;
    Animation anim;

    void Start()
    {
        startPos = gameObject.transform.position;
        anim = GetComponent<Animation>();
    }

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
