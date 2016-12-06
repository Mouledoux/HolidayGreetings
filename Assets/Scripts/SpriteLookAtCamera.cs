using UnityEngine;
using System.Collections;

public class SpriteLookAtCamera : MonoBehaviour
{
    [SerializeField] GameObject Camera;
	
	// Update is called once per frame
	void Update ()
    {
        transform.LookAt(Camera.transform);
	}
}
