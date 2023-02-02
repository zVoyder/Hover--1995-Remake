using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class RemoveObjectiveOnTrigger : MonoBehaviour
{
    public ObjectivesGenerator objectivesGeneratorReference;
    public int scoreToRemove = 2000;

    private AudioSource _audio;

    private void Start()
    {
        _audio = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerInventory>(out PlayerInventory pli)
        && !pli.IsShielded)
        {
            _audio.Play();
            objectivesGeneratorReference.RemoveObjective();
            ScoreSingleton.instance.RemoveScore(scoreToRemove);
        }
    }
}
