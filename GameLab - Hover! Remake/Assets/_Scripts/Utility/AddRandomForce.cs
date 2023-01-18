using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AddRandomForce : MonoBehaviour
{
    public ForceMode forceMode = ForceMode.Impulse;
    [Range(1, 5)] public float cooldown = 5;
    public float maxForce, minForce;

    private bool _impulse = false;
    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!_impulse)
        {
            StartCoroutine(AddForceAndWait());
        }
    }

    private IEnumerator AddForceAndWait()
    {
        _impulse = true;
        _rb.AddForce(Random.insideUnitSphere * Random.Range(minForce, maxForce), forceMode);
        yield return new WaitForSeconds(cooldown);
        _impulse = false;
    }
}