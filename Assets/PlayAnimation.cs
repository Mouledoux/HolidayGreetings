using UnityEngine;
using System.Collections;

public class PlayAnimation : MonoBehaviour
{
    Vector3 startPos;

    void Start()
    {
        startPos = gameObject.transform.position;
    }

    public void StartRun()
    {
        GetComponent<Animation>().Play();
    }

    public void BackToStart()
    {
        transform.position = startPos;
    }
}
