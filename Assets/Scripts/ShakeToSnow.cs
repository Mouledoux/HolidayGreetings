using UnityEngine;
using System.Collections;

public class ShakeToSnow : MonoBehaviour
{
    bool headShoke;
    [Range(5,10)]
    [SerializeField] float shakeSensitivity;

    //[Range(0, 1)]
    float snowAmount = .5f;

    [SerializeField] ParticleSystem snow;

    // Test Variable
    //public UnityEngine.UI.Text snow;

	// Use this for initialization
	void Start ()
    {
        StartCoroutine(CheckShake());
	}

    [ContextMenu("hit snow")]
    public void Snow()
    {
        snow.Play();
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

            if(Vector3.Distance(newRot,lastRot) > shakeSensitivity)
            {
                count++;
            }
            else
            {
                if(count > 0)
                    count--;
            }

            if(count >= 3)
            {
                count = 0;
                StartCoroutine(ShakeSnow());
                Debug.Log("HeadShake");
            }


            yield return new WaitForSeconds(.01f);
        }
    }
    
    IEnumerator ShakeSnow()
    {
        float timer = 0;

        snow.Play();
        while(timer < snowAmount)
        {
            timer += Time.deltaTime;

            yield return null;
        }
        snow.Stop();
    }	
}
