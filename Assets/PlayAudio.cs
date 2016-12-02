using UnityEngine;
using System.Collections;

public class PlayAudio : MonoBehaviour
{
    private AudioSource source;
	void Start ()
    {
        source = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void StartAudio()
    {
        source.Play();
    }

    public void StopAudio()
    {
        source.Stop();
    }

    public void SetPitch(float newPitch)
    {
        source.pitch = newPitch;
        print(source.pitch);
    }
}
