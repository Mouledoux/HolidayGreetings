using UnityEngine;
using System.Collections;

public class ResetObjcts : MonoBehaviour
{
    public GameObject Sled;
    public GameObject SnowBank;
    public GameObject Sign;
    public GameObject[] Penguins;
	    
    public void Reset()
    {
        Sled.transform.localPosition = Vector3.zero;
        Sled.transform.localRotation = Quaternion.identity;

        SnowBank.SetActive(true);

        Sign.GetComponent<Collider>().enabled = true;

        foreach(GameObject p in Penguins)
        {
            p.transform.localPosition = Vector3.zero;
            p.transform.localRotation = Quaternion.identity;
        }
    }
}
