using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This class is used for the objectives in game
/// when enter on trigger is triggered spawn an other objective in the scene
/// </summary>
public class NextObjectiveTrigger : MonoBehaviour
{
    [Tooltip("Who can grab this objective?")]public string triggerTag = Extension.Constants.Tags.PLAYER; // Tag of the entity that can grab this objective
    
    private ObjectivesGenerator _gen; //The ObjectivesGenerator
    internal int objectiveNumber; // How many objectives i need to spawn then, Internal so I can access it only via script

    private void Start()
    {
        _gen = FindObjectOfType<ObjectivesGenerator>(); // Find the object of type ObjectiveGenerator in the Scene
    }

    /// <summary>
    /// OnTriggerEnter Event for triggering the next spawn of the objectives
    /// and detroying the current one
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(triggerTag))
        {
            if (_gen.remainingEnabledObjects > 0)
            {
                _gen.SpawnInRandomPosition(--_gen.remainingEnabledObjects);
            }
            else
            {
                _gen.Reset();
                _gen.SpawnInRandomPosition(_gen.nObjectives);
            }

            Destroy(gameObject);
        }
    }

}
