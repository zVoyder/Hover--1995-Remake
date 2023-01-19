using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectivesGenerator : MonoBehaviour
{
    public GameObject objective;
    public List<Vector3> positions;

    private int _remainingEnabledObjects;

    private void Start()
    {
        _remainingEnabledObjects = positions.Count;
        SpawnInRandomPosition();
    }

    /// <summary>
    /// Spawn the object in a random position
    /// </summary>
    public void SpawnInRandomPosition()
    {
        if (_remainingEnabledObjects > 0)
        {
            Transform t;
            t = Generate();

            _remainingEnabledObjects--;
        }
    }

    /// <summary>
    /// Generate a random position of the list and remove it from the list
    /// </summary>
    /// <returns></returns>
    private Transform Generate()
    {
        int r = positions.Count-1;
        Debug.Log(r);
        
        Transform t = Instantiate(objective, positions[r], Random.rotation).transform;
        positions.RemoveAt(r);

        return t;
    }
}
