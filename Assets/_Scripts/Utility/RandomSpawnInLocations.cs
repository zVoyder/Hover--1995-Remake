using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawnInLocations : MonoBehaviour
{
    public GameObject gameObjectToSpawn;
    public List<Vector3> spawnLocations;


    void Start()
    {

        Instantiate(gameObjectToSpawn,
            spawnLocations[(int)Random.Range(0, spawnLocations.Count - 1)],
            Quaternion.Euler(new Vector3(0, Random.Range(0f,360f), 0)));
    }
}
