using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddScoreLevelClear : Action
{
    public int scoreToAdd = 2000;

    public override void SetAction()
    {
        ScoreSingleton.instance.AddScore(scoreToAdd);
    }
}
