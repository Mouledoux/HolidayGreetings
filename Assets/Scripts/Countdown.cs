using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;

public class Countdown : MonoBehaviour
{
    Text text;
    RectTransform rect;

    [SerializeField] AudioClip numBeep;
    [SerializeField] AudioClip goBeep;

    AudioSource audio;

    public UnityEvent loadedAction;

    void Start()
    {
        text = gameObject.GetComponent<Text>();
        rect = gameObject.GetComponent<RectTransform>();
        audio = gameObject.GetComponent<AudioSource>();
    }

    [ContextMenu("Do the thing")]
    public void StartCount()
    {
        StartCoroutine(StartCountDown());
    }

    IEnumerator StartCountDown()
    {
        float timer = 4;

        int trackNum = 0;
        bool playGo = false;
        bool sameNum = false;

        loadedAction.Invoke();

        while(timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
                timer = 0;
            rect.localScale = new Vector3((1 - (timer % 1)) * 50, (1 - (timer % 1)) * 50, 1);

            if (timer > 1)
            {
                text.text = ((int)timer).ToString();
                sameNum = (trackNum == (int)timer);
                if (!sameNum)
                {
                    audio.PlayOneShot(numBeep);
                }

                trackNum = (int)timer;
            }
            else
            {
                text.text = "GO!";
                if (!playGo)
                {
                    playGo = true;
                    audio.PlayOneShot(goBeep);
                }
            }
            
            yield return null;
        }
        text.text = "";
    }
}
