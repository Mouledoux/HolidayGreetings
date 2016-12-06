using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Description:
/// Used to smoothly transistion between LOOPING audio.
/// </summary>
public class SmoothMusicTransistion : MonoBehaviour
{
    /// <summary>
    /// Ensures there is an audio source on the object, and that looping is set to FALSE
    /// </summary>
    void Awake()
    {
        if(!audioSource)
        {
            audioSource = GetComponent<AudioSource>() ?
                GetComponent<AudioSource>() : gameObject.AddComponent<AudioSource>() as AudioSource;
        }

        audioSource.loop = false;

        m_currentTrack = m_nextTrack = audioTracks[0];

        StartCoroutine(PlayAudioClips());
    }

    public void SetNextTrack(int trackIndex)
    {
        m_nextTrack = audioTracks[trackIndex];
    }

    //public void SetNextTrack(AudioClip newClip)
    //{
    //    int trackIndex;

    //    if (!audioTracks.Contains(newClip))
    //    {
    //        audioTracks.Add(newClip);
    //    }

    //    trackIndex = audioTracks.IndexOf(newClip);
    //    SetNextTrack(trackIndex);
    //}

    public IEnumerator PlayAudioClips()
    {
        while (true)
        {
            audioSource.clip = m_currentTrack;
            audioSource.Play();
            yield return new WaitWhile(() => audioSource.isPlaying);
            m_currentTrack = m_nextTrack;
        }
    }

    public AudioSource audioSource;
    public List<AudioClip> audioTracks = new List<AudioClip>();

    private AudioClip m_currentTrack;
    private AudioClip m_nextTrack;
}
