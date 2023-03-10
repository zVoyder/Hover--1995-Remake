using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR


using UnityEditor;
[CustomEditor(typeof(ObjectivesGenerator))]
public class ObjectivesGenerationEditor : Editor
{
    ObjectivesGenerator _script;

    private void OnEnable()
    {
        _script = (ObjectivesGenerator)target;
    }

    public override void OnInspectorGUI() //Override the method OnInspectorGUI
    {
        base.OnInspectorGUI();
        //Set the target of the editor to the script StateMachineAI by casting it.
        int qnt = _script.repetitions.Total();

        if (qnt > _script.positionsPool.Count && !Application.isPlaying)
        {
            for (int i = _script.positionsPool.Count; i < qnt; i++)
            {
                _script.positionsPool.Add(Vector3.zero);
            }
        }


    }


}

#endif