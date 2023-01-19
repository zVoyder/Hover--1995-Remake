using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// SimpleMove class used for testing
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class SimpleMove : MonoBehaviour
{
    public float forceAmount = 10.0f;
    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 forceDirection = new Vector3(horizontal, 0, vertical);
        _rb.AddForce(forceDirection * forceAmount);
    }
}
