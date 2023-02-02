using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioWithRigidbody : MonoBehaviour
{
    [Range(-3, 3)]
    public float minPitch = 0.5f, maxPitch = 2.0f;
    [Range(10, 100)]
    public float accelerationScale = 50f;

    private Rigidbody _rb;
    private AudioSource _sound;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _sound = GetComponent<AudioSource>();
    }

    void Update()
    {
        float speed = _rb.velocity.magnitude;
        _sound.pitch = Mathf.Lerp(minPitch, maxPitch, speed / accelerationScale);
    }
}
