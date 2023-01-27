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


        if (_script.quantity > _script.positions.Count && !Application.isPlaying)
        {
            for (int i = _script.positions.Count; i < _script.quantity; i++)
            {
                _script.positions.Add(Vector3.zero);
            }
        }
        
        _script.quantity = _script.quantity < 0 ? 0 : _script.quantity;
    }


}

#endif