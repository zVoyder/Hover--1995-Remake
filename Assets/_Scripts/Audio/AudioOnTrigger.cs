using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Simple script that plays an audio on trigger enter
/// </summary>
public class AudioOnTrigger : MonoBehaviour
{
    public List<string> bumpableTags = new List<string>() { Constants.Tags.PLAYER };
    public AudioSFX audioEffect;

    private void OnTriggerEnter(Collider hit)
    {
        if (bumpableTags.Contains(hit.transform.tag))
        {
            Extension.Audios.PlayClipAtPoint(audioEffect, hit.ClosestPoint(transform.position));
        }
    }
}
