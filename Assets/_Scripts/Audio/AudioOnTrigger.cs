using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioOnTrigger : MonoBehaviour
{
    public List<string> bumpableTags = new List<string>() { Extension.Constants.Tags.PLAYER };
    public AudioSFX audioEffect;

    private void OnTriggerEnter(Collider hit)
    {
        if (bumpableTags.Contains(hit.transform.tag))
        {
            Extension.Methods.Audios.PlayClipAtPoint(audioEffect, hit.ClosestPoint(transform.position));
        }
    }
}
