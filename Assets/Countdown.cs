using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;

public class Countdown : MonoBehaviour
{
    Text text;
    RectTransform rect;

    public UnityEvent loadedAction;

    void Start()
    {
        text = gameObject.GetComponent<Text>();
        rect = gameObject.GetComponent<RectTransform>();
    }

    [ContextMenu("Do the thing")]
    public void StartCount()
    {
        StartCoroutine(StartCountDown());
    }

    IEnumerator StartCountDown()
    {
        float timer = 4;

        while(timer > 0)
        {
            timer -= Time.deltaTime;

            rect.localScale = new Vector3(timer % 1 * 5, timer % 1 * 5, 1);

            if (timer > 1)
                text.text = ((int)timer).ToString();
            else
                text.text = "GO!";
            yield return null;
        }

        loadedAction.Invoke();
    }
}
