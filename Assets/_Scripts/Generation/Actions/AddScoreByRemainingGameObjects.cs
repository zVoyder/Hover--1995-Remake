using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddScoreByRemainingGameObjects : Action
{
    public ObjectivesGenerator objectivesGeneratorReference;
    public int scorePerObjective = 2000;

    public override void SetAction()
    {
        int qnt = objectivesGeneratorReference.repetitions.Total() - objectivesGeneratorReference.GrabbedQuantity;

        Debug.Log(qnt);

        ScoreSingleton.instance.
            AddScore(qnt * scorePerObjective);
    }

}
