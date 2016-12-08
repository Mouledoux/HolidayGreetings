using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LightTwinkle : MonoBehaviour
{
    public List<SpriteRenderer> lights = new List<SpriteRenderer>(); // asdadsasd
    public List<Color> colors;

    void Start()
    {
        StartCoroutine(LightBlink());
    }

    [ContextMenu("Add lights")]
    public void AddLights()
    {
        foreach (Transform t in gameObject.transform)
        {
            lights.Add(t.gameObject.GetComponent<SpriteRenderer>());
        }
    }

    [ContextMenu("Colorize")]
    public void Colorize()
    {
        int i = 0;
        foreach (SpriteRenderer l in lights)
        {
            l.color = colors[i];
            i++;
            if (i > 3)
                i = 0;
        }
    }

    IEnumerator LightBlink()
    {
        while(true)
        {
            foreach (SpriteRenderer l in lights)
            {
                int i = Random.Range(0, 3);
                l.enabled = (i == 1);  
            }

            yield return new WaitForSeconds(2);
        }
    }
}
