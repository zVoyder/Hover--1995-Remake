using System.Collections;
using System.Collections.Generic;
using UnityEditor;


/// <summary>
/// Custom editor fot the script StateMachineAI
/// </summary>
[CustomEditor(typeof(StateMachineAI))]
public class StateMachineAIEditor : Editor //Extend the class editor
{
    public override void OnInspectorGUI() //Override the method OnInspectorGUI
    {
        base.OnInspectorGUI();

        StateMachineAI script = (StateMachineAI)target; //Set the target of the editor to the script StateMachineAI by casting it.

        script.floorType = (StateMachineAI.WorldOriginGenerationType)EditorGUILayout.EnumPopup("Floor Type", script.floorType); // Setting the enum pop up


        //Switch for
        switch (script.floorType) // Switch for each type of the enum
        {
            case StateMachineAI.WorldOriginGenerationType.CUSTOM:

                script.size = EditorGUILayout.Vector2Field("Size", script.size);


                break;

            case StateMachineAI.WorldOriginGenerationType.TERRAIN:

                //ObjectField method needs the casting to the object i want to use (in this case TerrainData).
                script.terrainData = (UnityEngine.TerrainData)EditorGUILayout.ObjectField("Terrain", script.terrainData, typeof(UnityEngine.TerrainData), true);

                break;

            case StateMachineAI.WorldOriginGenerationType.PLANE:
                
                script.plane = (UnityEngine.Transform)EditorGUILayout.ObjectField("Plane", script.plane, typeof(UnityEngine.Transform), true);

                break;
        }
    }

}
