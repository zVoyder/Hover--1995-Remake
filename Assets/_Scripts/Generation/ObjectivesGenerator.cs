using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectivesGenerator : MonoBehaviour
{
    public GameObject objective;
    public int nObjectives;
    public List<Vector3> positions;


    public static int remainingEnabledObjects;

    public List<Vector3> usedPositions;

    private void Start()
    {
        remainingEnabledObjects = nObjectives;

        usedPositions.Add(Generate());
    }

    public void SpawnInRandomPosition(int n)
    {
        for (int i = 0; i < n; i++)
        {
            Transform t;

            
                t = Generate();
            

            usedPositions.Add(t.position);

            t.GetComponent<NextObjectiveTrigger>().objectiveNumber = remainingEnabledObjects;
        }
    }

    private Transform Generate()
    {
        Transform t; 
        int r = Random.Range(0, positions.Count - 1);

        do
        {
            return Instantiate(objective, positions[r], Random.rotation).transform;

        } while (usedPositions.Contains(t.position)) ;
    }
}
