using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddScoreByRemainingGameObjects : Action
{
    public ObjectivesGenerator objectivesGeneratorReference;
    public string objectiveTag = Constants.Tags.ENEMY_FLAG;
    public int scorePerObjective = 2000;
    

    public override void SetAction()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag(objectiveTag);

        int toAdd = objectivesGeneratorReference.repetitions.Total() + gos.Length;

        ScoreSingleton.instance.
            AddScore(toAdd * scorePerObjective);
    }

    private void Update()
    {
        
    }
}
