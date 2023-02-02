using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Simple Class to set the multiplier of the singleton on scene startup
/// </summary>
public class SceneSetMultiplier : MonoBehaviour
{
    public float multiplier = 1.0f;

    private void Start()
    {
        ScoreSingleton.instance.Multiplier = multiplier;
    }
}
