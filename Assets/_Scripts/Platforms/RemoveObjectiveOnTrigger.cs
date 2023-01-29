using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class RemoveObjectiveOnTrigger : MonoBehaviour
{
    public ObjectivesGenerator objectivesGeneratorReference;
    public string triggerTag = Constants.Tags.PLAYER;
    public int scoreToRemove = 2000;

    private AudioSource _audio;

    private void Start()
    {
        _audio = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(triggerTag))
        {
            _audio.Play();
            objectivesGeneratorReference.RemoveObjective();
            ScoreSingleton.instance.RemoveScore(scoreToRemove);
        }
    }
}
