using UnityEngine;
using System.Collections;

public class ShakeToSnow : MonoBehaviour
{
    bool headShoke;
    

	// Use this for initialization
	void Start ()
    {
        StartCoroutine(CheckShake());
	}

    IEnumerator CheckShake()
    {

        Vector3 newRot  = transform.localRotation.eulerAngles;
        Vector3 lastRot;

        int count = 0;

        while (true)
        {
            lastRot = newRot;
            newRot = transform.localRotation.eulerAngles;
            if(Vector3.Distance(newRot,lastRot) > 15)
            {
                count++;
            }

            if(count >= 3)
            {
                count = 0;
                Debug.Log("HeadShake");
            }


            yield return new WaitForSeconds(.1f);
        }
    }	
}
