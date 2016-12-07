using UnityEngine;
using System.Collections;

public class DelayAnimation : MonoBehaviour
{
    public float delay = 1;
    public GameObject go;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(AnimDelay());
    }

    IEnumerator AnimDelay()
    {
        float timer = 0;

        while (timer <= delay)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        go.SetActive(true);
    }
}