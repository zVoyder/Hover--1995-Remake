using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Extension;

/// <summary>
/// This class is a necessary class tha permits the addforce
/// on a rigidbody that works with a navmesh agent
/// </summary>

[RequireComponent( typeof(Rigidbody), typeof(NavMeshAgent), typeof(StateMachineAI) )]
public class AIBounce : MonoBehaviour
{
    public string collisionTag = Extension.Constants.Tags.PLAYER;
    [Range(5, 50)] public float bumpForce = 10f, pushForce = 5f;
    [Tooltip("How long before the AI start to run again?")] [Range(.5f, 5)] public float recover = 1.5f;

    private Rigidbody _rb;
    private StateMachineAI _en;
    private NavMeshAgent _agent;

    private void Start()
    {
        _en = GetComponent<StateMachineAI>(); // Get the components
        _agent = GetComponent<NavMeshAgent>();
        _rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag(collisionTag)) // If the collision is with the specific tag
        {
            StartCoroutine(RenableIn(recover)); // Start the corutine for recovering after the bounce
            _rb.AddForce(-transform.forward * bumpForce, ForceMode.Impulse); //bump backward

            if(collision.transform.TryGetComponent<Rigidbody>(out Rigidbody rb)){
                rb.AddForce(-rb.transform.forward * pushForce, ForceMode.Impulse); // push the player backward
            }
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
