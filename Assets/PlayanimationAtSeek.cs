using UnityEngine;
using System.Collections;

public class PlayanimationAtSeek : MonoBehaviour
{
    [SerializeField] float seekTime = 0;
    [SerializeField] string AnimationName = "";
    Animation anim;

    void Start()
    {
        anim = GetComponent<Animation>();
    }
    	
	public void StartAtSeektime()
    {
        anim.Play(AnimationName);
        anim[AnimationName].time = seekTime;
    }
}
