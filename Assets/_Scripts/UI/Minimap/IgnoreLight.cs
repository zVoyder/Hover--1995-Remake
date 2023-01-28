using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class used for ignoring the visibility of the light for a camera
/// </summary>
public class IgnoreLight : MonoBehaviour
{
    Light _lightIgnored;

    public Light LIghtIgnored { set => _lightIgnored = value; }

    void OnPreCull()
    {
        if (_lightIgnored != null)
            _lightIgnored.enabled = false;
    }

    void OnPreRender()
    {
        if (_lightIgnored != null)
            _lightIgnored.enabled = false;
    }
    void OnPostRender()
    {
        if (_lightIgnored != null)
            _lightIgnored.enabled = true;
    }
}
