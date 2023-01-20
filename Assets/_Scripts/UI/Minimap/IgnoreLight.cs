using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class used for ignoring the visibility of the light for a camera
/// </summary>
public class IgnoreLight : MonoBehaviour
{
    Light limelight;

    public Light Limelight { set => limelight = value; }

    void OnPreCull()
    {
        if (limelight != null)
            limelight.enabled = false;
    }

    void OnPreRender()
    {
        if (limelight != null)
            limelight.enabled = false;
    }
    void OnPostRender()
    {
        if (limelight != null)
            limelight.enabled = true;
    }
}
