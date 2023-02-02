using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ImmobilizePlatform : MonoBehaviour
{
    public float immobilizeDuration;
    private RBPlayerMovement player;
    private AudioSource _audio;

    private void Start()
    {
        _audio = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<RBPlayerMovement>(out player)) //when the player collides set the variable true
        {
            if (other.TryGetComponent<PlayerInventory>(out PlayerInventory pli)
    && !pli.IsShielded)
            {
                _audio.Play();
                StartCoroutine(ReEnableIn(immobilizeDuration, player.CanMove));
            }
        }
    }

    private IEnumerator ReEnableIn(float time, bool b)
    {
        player.CanMove = false;
        player.rigidBody.velocity = Vector3.zero;
        yield return new WaitForSeconds(time);
        player.CanMove = true;
    }
}

