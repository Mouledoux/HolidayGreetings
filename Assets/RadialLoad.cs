///////////////////////////////////////////////////////////////////////////
// Author:  Zac King            ///////////////////////////////////////////
// Contact: ZacKingx@Gmail.com  ///////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////
// Usage:                                                                //
// //////                                                                //
// Notes:                                                                //
///////////////////////////////////////////////////////////////////////////

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RadialLoad : MonoBehaviour
{
    #region Variables
    [SerializeField] private Image progressCircle;  // Load image
    [SerializeField] private Gradient loadGradient; // Gradient over time for load
    [SerializeField] private float timeToPlay = 3f; // Seconds to complete load
    public bool isLoading = false;                  // Bool for current state of loading
    #endregion

    // MonoBehaviour ///////////////////////////////////////////////////////////////////////////////////
    [ContextMenu("Set the Type to Fill")]
    void Awake()    // If not already set up, it will be fixed on awake
    {
        progressCircle.type = Image.Type.Filled;    // Amount visible dictacted by value
        progressCircle.fillAmount = 0;              // Zero it out
        progressCircle.fillOrigin = 2;              // Fill Origin = top
        progressCircle.fillClockwise = false;       // FIll Counter clockwise
    }

    // Functions ///////////////////////////////////////////////////////////////////////////////////////
    public void StopLoad()  // Public function allowing asessing obj halt the Load Coroutine 
    {
        isLoading = false;
    }

    public void LoadTarget(GameObject selection)  //
    {
        isLoading = true;
        StartCoroutine(LoadCircle(selection));
    }

    IEnumerator LoadCircle(GameObject selection)    // Load coroutine
    {
        bool startLoad = false;
        float timer = 0;
        while (timer <= timeToPlay && isLoading)
        {
            progressCircle.fillAmount = timer / timeToPlay;
            progressCircle.color = loadGradient.Evaluate(timer / timeToPlay);
            timer += Time.deltaTime;

            if (timer >= (timeToPlay / 2) && !startLoad)
            {
                startLoad = true;
            }

            yield return null;
        }

        if (isLoading)
        {
            selection.GetComponent<RaySelectionVolume>().loadedAction.Invoke();
        }

        progressCircle.fillAmount = 0;   // Reset
    }
}
