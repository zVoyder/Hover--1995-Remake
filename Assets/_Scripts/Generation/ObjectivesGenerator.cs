using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectivesGenerator : MonoBehaviour
{
    public const int MAX_OBJECTIVES = 6;

    public Image uICounterPlayer, uICounterEnemy;
    public GameObject objective;
    [Range(1, MAX_OBJECTIVES)]public int quantity = 3;
    public List<Vector3> positions;
    
    private int _objectsInScene;

    private float _defaultWidth;


    private void Start()
    {
        _objectsInScene = 0;

        SetUpBackgroundUI();


        SpawnInRandomPosition();
    }

    

    private void SpawnInRandomPosition()
    {
        if (_objectsInScene < quantity)
        {
            _objectsInScene++;
            Generate();
        }
    }

    /// <summary>
    /// Generate the objective in a random position of the list and remove that position from the list
    /// </summary>
    /// <returns></returns>
    private void Generate()
    {
        int r = positions.Count - 1;
        
        Instantiate(objective, positions[r], Quaternion.Euler(new Vector3(0, Random.Range(0, 180), 0)));
        positions.RemoveAt(r);
    }


    private void SetUpBackgroundUI()
    {
        if(uICounterPlayer.transform.parent.TryGetComponent<Image>(out Image back))
        {

            Debug.Log(quantity / MAX_OBJECTIVES);
            back.fillAmount = (float)quantity / (float)MAX_OBJECTIVES;
        }
    }


    public void PlayerGrabbedAnObjective()
    {
        AddObjectiveToPlayerUI();
        SpawnInRandomPosition();
    }

    public void EnemyGrabbedAnObjective()
    {
        AddObjectiveToEnemyUI();
        SpawnInRandomPosition();
    }

    private void AddObjectiveToPlayerUI()
    {
        uICounterPlayer.fillAmount += 1f / (float)MAX_OBJECTIVES; 
    }

    private void AddObjectiveToEnemyUI()
    {
        uICounterEnemy.fillAmount += 1f / (float)MAX_OBJECTIVES;
    }
}