using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Extension.Data;


/// <summary>
/// ObjectviesGenerator is used to generate objectives in the scene to pick up
/// </summary>
public class ObjectivesGenerator : MonoBehaviour
{
    public Image uICounterPlayer; //UI Image that works as a counter with the fill.amount
    public GameObject objective;
    [Tooltip("Who can grab this objective?")] public string triggerTag = Constants.Tags.PLAYER;
    public Reps repetitions; // How many series of objectives to generate
    [Range(0, 20)]public int maxObjectives = 6; // Max objectives that can spawn
    public List<Vector3> positions; // List of all positions the objective can spawn

    private int _totalQuantity, _grabbedQuantity; //total quantity to spawn and how many to grab left


    private void Start()
    {
        _totalQuantity = repetitions.Total();
        _grabbedQuantity = 0;

        SetUpBackgroundUI(); // Set up the backgorund based on maxobjectives

        Spawn(); //Start generate
    }

    
    private void Spawn()
    {
        if (repetitions.series > 0) //if there are series left to do
        {
            repetitions.series--;

            for (int s = repetitions.steps; s > 0; s--) // do a series of S steps
            {
                InstantiateObjective();
            }
        }
        else
        {
            Completed();
        }
    }

    private void Completed()
    {
#if DEBUG
        Debug.Log(triggerTag + " has Completed");
#endif
        BroadcastMessage("SetAction");
    }

    /// <summary>
    /// Grab an objective and add one to the counter Grabbed Objectives,
    /// Also check if the trigger has collected all the objectives to grab before
    /// spawn a new series
    /// </summary>
    public void GrabObjective()
    {
        AddObjectiveToUI(); // Update UI

        _grabbedQuantity++;
        if (_grabbedQuantity == repetitions.steps) // if has collected all the objectives
        {
            _grabbedQuantity = 0; // A series is gone and trigger must collect a new one.
            Spawn();
        }    
    }

    /// <summary>
    /// Generate the objective in a random position of the list and remove that position from the list
    /// Adds a component NextObjectiveTrigger with the reference to this script.
    /// </summary>
    /// <returns></returns>
    private void InstantiateObjective()
    {
        int randomIndex = Random.Range(0, positions.Count - 1);
        
        GameObject go = Instantiate(objective, positions[randomIndex], Quaternion.Euler(new Vector3(0, Random.Range(0, 180), 0)));
        NextObjectiveTrigger next = go.AddComponent<NextObjectiveTrigger>();
        next.GeneratorReference = this;
        next.TriggerTag = triggerTag;

        positions.RemoveAt(randomIndex);
    }

    /// <summary>
    /// Set up the backgorund of the UI Image counter to match the total quantity
    /// to grab in this scene.
    /// </summary>
    private void SetUpBackgroundUI()
    {
        if(uICounterPlayer.transform.parent.TryGetComponent<Image>(out Image back))
        {
            back.fillAmount = (float)_totalQuantity / (float)maxObjectives;
        }
    }

    /// <summary>
    /// Update the UI by adding one point in proportion of Max Objectives
    /// </summary>
    private void AddObjectiveToUI()
    {
        uICounterPlayer.fillAmount += 1f / (float)maxObjectives; 
    }
}