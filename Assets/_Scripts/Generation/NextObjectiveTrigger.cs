using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This class is used for the objectives in game
/// when enter on trigger is triggered spawn an other objective in the scene
/// </summary>
public class NextObjectiveTrigger : MonoBehaviour
{
    [Tooltip("Who can grab this objective?")]public string triggerTag = Constants.Tags.PLAYER; // Tag of the entity that can grab this objective

    private ObjectivesGenerator _gen; //The ObjectivesGenerator


    private void Start()
    {
        _gen = FindObjectOfType<ObjectivesGenerator>(); // Find the object of type ObjectiveGenerator in the Scene
    }

    /// <summary>
    /// OnTriggerEnter Event for triggering the next spawn of the objective
    /// and detroying the current one
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(triggerTag))
        {
            _gen.PlayerGrabbedAnObjective();
            Destroy(gameObject);
        }
    }

}
