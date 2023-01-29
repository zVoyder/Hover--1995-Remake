using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ScriptableObject for storing the score
/// </summary>
[CreateAssetMenu(menuName = "Score")]
public class SO_Score : ScriptableObject
{
    public int score;
    public float multiplier;
}
