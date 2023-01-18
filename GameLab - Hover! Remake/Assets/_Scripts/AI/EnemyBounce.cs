using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// This class is a necessary class tha permits the addforce
/// on a rigidbody that works with a navmesh agent
/// </summary>

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyStateMachine))]
public class EnemyBounce : MonoBehaviour
{
    public string collisionTag = "Player";
    [Range(10, 50)] public float force = 10f;
    [Tooltip("How long before the AI start to run again?")] [Range(.5f, 5)] public float recover = 1.5f;

    private Rigidbody _rb;
    private EnemyStateMachine _en;
    private NavMeshAgent _agent;

    private void Start()
    {
        _en = GetComponent<EnemyStateMachine>(); // Get the components
        _agent = GetComponent<NavMeshAgent>();
        _rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag(collisionTag)) // If the collision is with the specific tag
        {
            StartCoroutine(RenableIn(recover)); // Start the corutine for recovering after the bounce
            _rb.AddForce(-transform.forward * force, ForceMode.Impulse);
        }
    }

    /// <summary>
    /// Disable the Rigidbody and the EnemyStateMachine
    /// <param name="time"></param>
    /// <returns>WaitForSeconds</returns>
    private IEnumerator RenableIn(float time)
    {
        _agent.enabled = false; 
        _en.enabled = false;

        yield return new WaitForSeconds(time); // Wait before renenable

        _rb.velocity = Vector3.zero; // Set velocity to zero so the navmesh will work properly again
        _en.enabled = true;
        _agent.enabled = true;
    }
}
