using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourOnTrigger : MonoBehaviour
{
    public StateMachineAI.AIBehaviour behaviourToSet;
    public Color color = Color.white;

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.root.TryGetComponent<StateMachineAI>(out StateMachineAI ai))
        {
            ai.SetBehaviour(behaviourToSet, color);
        }
    }
}


