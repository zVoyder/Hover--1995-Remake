using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// This class is used for the objectives in game
/// when enter on trigger is triggered spawn an other objective in the scene
/// </summary>
public class NextObjectiveTrigger : MonoBehaviour
{
    private ObjectivesGenerator _generatorReference; //The ObjectivesGenerator
    private string _triggerTag; // Tag of the entity that can grab this objective

    public string TriggerTag { get => _triggerTag; set => _triggerTag = value; }
    public ObjectivesGenerator GeneratorReference { get => _generatorReference; set => _generatorReference = value; }

    /// <summary>
    /// OnTriggerEnter Event for triggering the next spawn of the objective
    /// and detroying the current one
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TriggerTag))
        {
            GeneratorReference.GrabObjective();
            Destroy(gameObject);
        }
    }

}
