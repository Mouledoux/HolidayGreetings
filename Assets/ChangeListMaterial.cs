using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ChangeListMaterial : MonoBehaviour
{
    public Material mat;
    public List<GameObject> listToChange;

    [ContextMenu("Change Materials")]
    public void ChangeMaterials()
    {
        foreach(GameObject g in listToChange)
        {
            g.GetComponent<Renderer>().material = mat;
        }
    }
}
