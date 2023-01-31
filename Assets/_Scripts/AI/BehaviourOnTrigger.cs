using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourOnTrigger : MonoBehaviour
{
    public string triggerTag = Constants.Tags.ENEMY;
    public StateMachineAI.AIBehaviour behaviourToSet;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(triggerTag)) {
            StateMachineAI ai = other.transform.root.GetComponentInChildren<StateMachineAI>();
            ai.SetBehaviour(behaviourToSet);
        }
    }
}


