using UnityEngine;
using System.Collections;

public class ResetObjcts : MonoBehaviour
{
    public GameObject Sled;
    public GameObject SnowBank;
    public GameObject Sign;
	    
    public void Reset()
    {
        Sled.transform.position = Vector3.zero;
        SnowBank.SetActive(true);
        Sign.GetComponent<Collider>().enabled = true;
    }
}
