using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectivesGenerator : MonoBehaviour
{
    public GameObject objective;
    public int quantity = 3;
    public List<Vector3> positions;
    private int _remainingEnabledObjects;

    private void Start()
    {
        _remainingEnabledObjects = quantity;
        SpawnInRandomPosition();
    }

    /// <summary>
    /// Spawn the object in a random position and descrease the remaining objects to spawn
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
    /// Generate the objective in a random position of the list and remove that position from the list
    /// </summary>
    /// <returns></returns>
    private Transform Generate()
    {
        int r = positions.Count - 1;
        
        Transform t = Instantiate(objective, positions[r], Quaternion.Euler(new Vector3(0, Random.Range(0, 180), 0))).transform;
        positions.RemoveAt(r);

        return t;
    }
}