using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Extension.Data;
using System.Linq;

/// <summary>
/// ObjectviesGenerator is used to generate objectives in the scene to pick up
/// </summary>
public class ObjectivesGenerator : MonoBehaviour
{
    public Image uICounterPlayer; //UI Image that works as a counter with the fill.amount
    public GameObject objective;
    [Tooltip("Who can grab this objective?")] public string triggerTag = Constants.Tags.PLAYER;
    public Reps repetitions; // How many series of objectives to generate
    [Range(0, 20)]public int maxObjectivesOnUI = 6; // Max objectives that can spawn
    public List<Vector3> positionsPool; // List of all positions the objective can spawn

    private List<Vector3> _positionToUse;
    private int _totalQuantity, _stepsDoneQuantity, _grabbedQuantity; //total quantity to spawn, how many to grab left and total grabbed quantity

    private int QuantityInGame { get => GameObject.FindGameObjectsWithTag("PlayerFlag").Length; }

    private void Start()
    {
        _positionToUse = new List<Vector3>(positionsPool);
        _totalQuantity = repetitions.Total();
        _stepsDoneQuantity = 0;
        _grabbedQuantity = 0;

        SetUpBackgroundUI(); // Set up the backgorund based on maxobjectives

        nextSeries(); //Start generate
    }

    
    private bool CheckWin()
    {
        if (_grabbedQuantity == _totalQuantity)
        {
            Completed();
            return true;
        }

        return false;
    }

    private void nextSeries()
    {
        if (!CheckWin())
        {
            if (QuantityInGame == 1 || QuantityInGame == 0)
            {
                for (int s = repetitions.steps; s > 0; s--) // do a series of S steps
                {
                    InstantiateObjective();
                }
            }
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
    /// Also check if the trigger has collected all the objectives to grab
    /// </summary>
    /// <returns>True if there are no more objectives to grab, otherwise False</returns>
    public void AddObjective()
    {
        AddObjectiveToUI();
        _grabbedQuantity++;
        _stepsDoneQuantity++;

        if (_stepsDoneQuantity == repetitions.steps) // if has collected all the objectives
        {
            _stepsDoneQuantity = 0; // A series is gone and trigger must collect a new one.
            nextSeries();
        }

        CheckWin(); // I have to it, so it check also if the stepsdone are zero
    }

    public void RemoveObjective()
    {
        if (_grabbedQuantity > 0)
        {
            _grabbedQuantity--;

            _stepsDoneQuantity = _stepsDoneQuantity == 0 ? 0 : _stepsDoneQuantity - 1;

            RemoveObjectiveToUI();

            _positionToUse.AddRange(positionsPool);

            _positionToUse = _positionToUse.Distinct().ToList();

            InstantiateObjective();
        }
    }

    /// <summary>
    /// Generate the objective in a random position of the list and remove that position from the list
    /// Adds a component NextObjectiveTrigger with the reference to this script.
    /// </summary>
    /// <returns></returns>
    private void InstantiateObjective()
    {
        int randomIndex = Random.Range(0, _positionToUse.Count - 1);
        
        GameObject go = Instantiate(objective, _positionToUse[randomIndex], Quaternion.Euler(new Vector3(0, Random.Range(0, 180), 0)));
        NextObjectiveTrigger next = go.AddComponent<NextObjectiveTrigger>();
        next.GeneratorReference = this;
        next.TriggerTag = triggerTag;
        
        _positionToUse.RemoveAt(randomIndex);
    }

    /// <summary>
    /// Set up the backgorund of the UI Image counter to match the total quantity
    /// to grab in this scene.
    /// </summary>
    private void SetUpBackgroundUI()
    {
        if(uICounterPlayer.transform.parent.TryGetComponent<Image>(out Image back))
        {
            back.fillAmount = (float)_totalQuantity / (float)maxObjectivesOnUI;
        }
    }

    /// <summary>
    /// Update the UI by adding one point in proportion of Max Objectives
    /// </summary>
    private void AddObjectiveToUI()
    {
        uICounterPlayer.fillAmount += 1f / (float)maxObjectivesOnUI; 
    }

    private void RemoveObjectiveToUI()
    {
        uICounterPlayer.fillAmount -= 1f / (float)maxObjectivesOnUI;
    }
}