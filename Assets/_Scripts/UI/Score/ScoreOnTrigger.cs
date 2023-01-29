using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This class is a simple class to add score when
/// an entity enter on trigger, calling the Singleton for the score
/// </summary>
public class ScoreOnTrigger : MonoBehaviour
{
    public string triggerTag = Constants.Tags.PLAYER;
    public int scoreToAdd;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(triggerTag))
        {
            ScoreSingleton.instance.AddScore(scoreToAdd);
        }
    }
}
