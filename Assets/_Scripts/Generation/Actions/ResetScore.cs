using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetScore : Action
{
    public override void SetAction()
    {
        ScoreSingleton.instance.ResetScore();
    }
}
