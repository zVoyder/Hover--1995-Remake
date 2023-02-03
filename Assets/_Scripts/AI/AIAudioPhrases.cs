using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource), typeof(StateMachineAI))]
public class AIAudioPhrases : MonoBehaviour
{

    //Audio SFX
    [Header("Audio SFX")]
    [Tooltip("How much does it talk? In seconds."), Range(1, 60)] public float audioFrequency = 1f;
    public AudioClip[] engagings;
    public AudioClip[] founds;
    public AudioClip[] patrolings;

    private AudioClip _phraseToPlay;
    private AudioSource _audio;
    private bool hasPlayedAudio;
    private StateMachineAI _ai;

    private void Start()
    {
        _audio = GetComponent<AudioSource>();
        _ai = GetComponent<StateMachineAI>();
    }

    private void Update()
    {
        SayPhrase();
    }


    #region Audio

    private void SayPhrase()
    {
        if (!hasPlayedAudio)
        {

            _phraseToPlay = _ai.GetCurrentState switch
            {
                StateMachineAI.AIState.CHASE => engagings[Random.Range(0, engagings.Length - 1)],

                StateMachineAI.AIState.GRABOBJECTIVE => founds[Random.Range(0, founds.Length - 1)],

                StateMachineAI.AIState.PATROL => patrolings[Random.Range(0, patrolings.Length - 1)],

                _ => null
            };


            StartCoroutine(PlayAudio(_phraseToPlay));
        }
    }

    private IEnumerator PlayAudio(AudioClip clip)
    {
        _audio.clip = clip;
        _audio.Play();
        hasPlayedAudio = true;
        yield return new WaitForSeconds(audioFrequency);
        hasPlayedAudio = false;
    }

    #endregion
}
