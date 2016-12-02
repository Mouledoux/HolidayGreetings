using UnityEngine;
using System.Collections;

public class SnowShakeOffset : MonoBehaviour
{
    private Vector3 posBase;

    void Start()
    {
        posBase = transform.localPosition;
    }

    void LateUpdate()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, posBase.y, transform.localPosition.z);
    }
}
