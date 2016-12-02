using UnityEngine;
using System.Collections;

public class TransitionToCamera : MonoBehaviour
{
    [SerializeField] GameObject camera;
    [SerializeField] GameObject GVRViewer;

    
	
	public void TransitionFromMenu()
    {
        gameObject.SetActive(false);
        camera.SetActive(true);
        GVRViewer.SetActive(true);
    }
}
