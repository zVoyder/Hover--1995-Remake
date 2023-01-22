using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// Simple script that plays an audio on collision if the
/// impact is greater than a specified force
/// </summary>
public class AudioOnCollision : MonoBehaviour
{
    [SerializeField]private float _forceToTrigger = 50f; //Impact force needed to trigger the audio

    public List<string> bumpableTags = new List<string>() { Extension.Constants.Tags.PLAYER };
    public AudioSFX audioEffect;

    private void OnCollisionEnter(Collision hit)
    {
        if (bumpableTags.Contains(hit.transform.tag))
        {
            // the magnitude of the impulse applied divided for fixedDeltaTime so it scales with the delta of FixedUpdate Time
            float vel = hit.impulse.magnitude / Time.fixedDeltaTime;
            vel /= 10f;

            if (vel > _forceToTrigger)
                Extension.Methods.Audios.PlayClipAtPoint(audioEffect, hit.contacts[0].point);
        }
    }
}
