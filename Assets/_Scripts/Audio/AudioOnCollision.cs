using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioOnCollision : MonoBehaviour
{
    public List<string> bumpableTags = new List<string>() { Extension.Constants.Tags.PLAYER };
    public AudioSFX audioEffect;

    private void OnCollisionEnter(Collision hit)
    {
        if(bumpableTags.Contains(hit.transform.tag))
        {
            Extension.Methods.Audios.PlayClipAtPoint(audioEffect, hit.contacts[0].point);
        }
    }
}
