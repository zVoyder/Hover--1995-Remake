using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyStateMachine))]
public class EnemyBounce : MonoBehaviour
{
    [Range(10, 50)] public float force = 10f;
    [Range(.5f, 5)] public float recover = 1.5f;

    private Rigidbody _rb;
    private EnemyStateMachine _en;
    private NavMeshAgent _agent;

    private void Start()
    {
        _en = GetComponent<EnemyStateMachine>();
        _agent = GetComponent<NavMeshAgent>();
        _rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            StartCoroutine(RenableIn(2));
            _rb.AddForce(-transform.forward * force, ForceMode.Impulse);
        }
    }

    private IEnumerator RenableIn(float time)
    {
        _agent.enabled = false;
        _en.enabled = false;

        yield return new WaitForSeconds(time);
        
        _en.enabled = true;
        _agent.enabled = true;
    }
}
