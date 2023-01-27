using System.Collections;
using System.Collections.Generic;
using UnityEditor;

#if UNITY_EDITOR
using UnityEngine;

/// <summary>
/// Custom editor fot the script StateMachineAI
/// </summary>
[CustomEditor(typeof(StateMachineAI))]
public class StateMachineAIEditor : Editor //Extend the class editor
{
    StateMachineAI _script;


    private void OnEnable()
    {

        _script = (StateMachineAI)target;
    }

    public override void OnInspectorGUI() //Override the method OnInspectorGUI
    {
        base.OnInspectorGUI();
        //Set the target of the editor to the script StateMachineAI by casting it.

        _script.heightEyesOffset = EditorGUILayout.Slider("Height Eyes Offset", _script.heightEyesOffset, 0, _script.transform.localScale.y);
        _script.floorType = (StateMachineAI.WorldOriginGenerationType)EditorGUILayout.EnumPopup("Floor Type", _script.floorType); // Setting the enum pop up


        //Switch for
        switch (_script.floorType) // Switch for each type of the enum
        {
            case StateMachineAI.WorldOriginGenerationType.CUSTOM:

                _script.size = EditorGUILayout.Vector2Field("Size", _script.size);


                break;

            case StateMachineAI.WorldOriginGenerationType.TERRAIN:

                //ObjectField method needs the casting to the object i want to use (in this case TerrainData).
                _script.terrainData = (UnityEngine.TerrainData)EditorGUILayout.ObjectField("Terrain", _script.terrainData, typeof(UnityEngine.TerrainData), true);

                break;

            case StateMachineAI.WorldOriginGenerationType.PLANE:
                
                _script.plane = (UnityEngine.Transform)EditorGUILayout.ObjectField("Plane", _script.plane, typeof(UnityEngine.Transform), true);

                break;
        }
    }

}

#endif