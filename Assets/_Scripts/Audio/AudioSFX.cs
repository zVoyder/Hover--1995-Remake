using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class AudioSFX
{
    public AudioClip clip;
    [Range(0, 1)] public float volume = 1;
    [Range(-3, 3)] public float pitch = 1;
    [Range(0, 1)] public float spatialBlend;
    public AudioMixerGroup mixerGroup;
}
