using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ChangeListMaterial : MonoBehaviour
{
    public Material mat;
    public List<GameObject> listToChange;

    public string grabName;

    [ContextMenu("Change Materials")]
    public void ChangeMaterials()
    {
        foreach(GameObject g in listToChange)
        {
            g.GetComponent<Renderer>().material = mat;
        }
    }

    [ContextMenu("GrabAll")]
    public void grabAll()
    {
        var v = GameObject.FindObjectsOfType<GameObject>();

        foreach(GameObject g in v)
        {
            if (g.name == grabName)
            {
                listToChange.Add(g);
            }
        }
    }
}
