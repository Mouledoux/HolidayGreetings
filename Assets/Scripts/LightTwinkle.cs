using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LightTwinkle : MonoBehaviour
{
    public List<SpriteRenderer> lights = new List<SpriteRenderer>(); // asdadsasd
    //public List<Color> colors;

    [Range(0, 255)]
    public int alpha = 255;

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

    [ContextMenu("Alpha-ize")]
    public void Colorize()
    {
        //int i = 0;
        float a = (float)alpha / 255;
        foreach (SpriteRenderer l in lights)
        {
            l.color = new Color(l.color.r, l.color.g, l.color.b, a);
            //l.color = colors[i];
            //i++;
            //if (i > 3)
            //    i = 0;
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
