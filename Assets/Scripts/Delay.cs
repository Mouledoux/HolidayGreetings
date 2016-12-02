using UnityEngine;
using System.Collections;

public class Delay : MonoBehaviour
{
    public float delay = 1;
    Animator anim;

	// Use this for initialization
	void Start ()
    {
        anim = gameObject.GetComponent<Animator>();
        StartCoroutine(AnimDelay());
	}
	
    IEnumerator AnimDelay()
    {
        float timer = 0;

        while(timer<= delay)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        anim.enabled = true;
    }
}
