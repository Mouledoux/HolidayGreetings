///////////////////////////////////////////////////////////////////////////
// Author:  Zac King            ///////////////////////////////////////////
// Contact: ZacKingx@Gmail.com  ///////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////
// Usage:                                                                //
//          Add component to the object to act as raycast                //
//          (Ray cast is from the objects forward)                       //
// //////                                                                //
// Notes:                                                                //
//          Do not have Clickable objects colliders overlapping          //
//          When Component is added it will require <RadialLoad>         //
///////////////////////////////////////////////////////////////////////////

using UnityEngine;

[RequireComponent(typeof(RadialLoad))]
public class RaySelect : MonoBehaviour
{
    #region Variables
    private RaycastHit rayCursor;   // Ray cast from camera
    private RadialLoad loader;      // Reference to the RadialLoad (On the same Game Object)
    public LayerMask mask;
    #endregion

    #region Functions
    void Awake()    // MonoBehaviour
    {
        loader = gameObject.GetComponent<RadialLoad>();     // Store Loader
    }

    void Update()   // Called every Physics step
    {
        if (Physics.Raycast(transform.position, transform.forward, out rayCursor, 10000, mask))  // TeleportNode layer
        {
            if (!loader.isLoading)  // makes sure not to trigger a load if one is not  already in progress
            { 
                if (rayCursor.collider.gameObject.GetComponent<RaySelectionVolume>() != null)
                    OnSelectionHit();   // RayCast on a valid Selection Volume
                else
                    OnSelectionExit();
            }
        }
        else
        {
            OnSelectionExit();  // RayCast is no longer on a Selection Volume
        }
    }

    // Functions ///////////////////////////////////////////////////////////////////////////////////////
    void OnSelectionHit()   // Start Load
    {
        loader.LoadTarget(rayCursor.collider.gameObject);  // Start loading image
    }

    void OnSelectionExit()  // Stop Load
    {
        loader.StopLoad();  // Stop loading image
    }
    #endregion
}
