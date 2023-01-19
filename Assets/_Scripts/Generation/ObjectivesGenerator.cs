using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectivesGenerator : MonoBehaviour
{
    public GameObject objective;
    public int nObjectives;
    public List<Vector3> positions;

    public int remainingEnabledObjects;
    public List<Vector3> usedPositions;

    private void Start()
    {
        remainingEnabledObjects = nObjectives;

        SpawnInRandomPosition(remainingEnabledObjects);
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
        int r;

        do
        {
            r = Random.Range(0, positions.Count - 1);
            t = Instantiate(objective, positions[r], Random.rotation).transform;
        } while (usedPositions.Contains(t.position));

        
        return t;
    }

    public void Reset()
    {
        remainingEnabledObjects = nObjectives;
        usedPositions.Clear();
    }
}
