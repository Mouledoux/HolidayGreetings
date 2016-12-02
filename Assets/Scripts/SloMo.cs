using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SloMo : MonoBehaviour
{
    [Range(0, 5)]
    public float targetSpeed;

    float timeScale = 1;

    public List<AudioSource> allAudio;

    void Start()
    {

        foreach(AudioSource a in FindObjectsOfType<AudioSource>())
        {
            allAudio.Add(a);
        }
    }

    bool timeSlowed = false;
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !timeSlowed)
        {
            timeSlowed = true;
            StartCoroutine(BulletTime(targetSpeed));
        }
        else
        {
            StopCoroutine("BulletTime");
            StartCoroutine(BulletTime(targetSpeed));
        }

        Time.timeScale = timeScale; // Ze Worrrldo

        foreach(AudioSource a in allAudio)
        {
            a.pitch = timeScale;
        }
	}

    

    IEnumerator BulletTime(float targetScale)
    {

        while(Input.GetKey(KeyCode.Mouse0))
        {
            if (timeScale > targetScale)
                timeScale -= 0.01f;
            if (timeScale < targetScale)
                timeScale = targetScale;

            yield return null;
        }

        while (timeScale < 1)
        {
            timeScale += 0.02f;
            yield return null;
        }

        timeScale = 1;
        timeSlowed = false;
    }
}
