using UnityEngine;
using System.Collections;

public class SloMo : MonoBehaviour
{
    [Range(0, 5)]
    public float timeScale = 1;

    //public AudioSource aud;

    bool timeSlowed = false;
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !timeSlowed)
        {
            timeSlowed = true;
            StartCoroutine(BulletTime(0.15f));
        }

            Time.timeScale = timeScale; // Ze Worrrldo
        //aud.pitch = timeScale;  // We have to make the slow affect the audio, and it works well
	}

    

    IEnumerator BulletTime(float targetScale)
    {

        while(Input.GetKey(KeyCode.Mouse0))
        {
            if (timeScale > targetScale)
                timeScale -= 0.01f;
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
