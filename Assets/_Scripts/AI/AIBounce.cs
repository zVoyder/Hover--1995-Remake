using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// This class is a necessary class tha permits the addforce
/// on a rigidbody that works with a navmesh agent
/// </summary>

[RequireComponent( typeof(Rigidbody), typeof(NavMeshAgent), typeof(StateMachineAI) )]
public class AIBounce : MonoBehaviour
{
    [Tooltip("Look at Target when bumped")]public bool lookAtTarget = true;
    public List<string> collisionTags = new List<string>(){ Extension.Constants.Tags.PLAYER };
    [Tooltip("Bump force of this rigidbody backward"), Range(5, 50)] public float bumpForce = 10f;
    [Tooltip("Push force of the rigidbody that collides forward"), Range(5, 50)] public float pushForce = 5f;
    [Tooltip("How long before the AI start to run again?")] [Range(.5f, 5)] public float recover = 1.5f;


    private Rigidbody _rb;
    private StateMachineAI _en;
    private NavMeshAgent _agent;

    bool _hasCollide;
    Transform _look;

    private void Start()
    {
        _en = GetComponent<StateMachineAI>(); // Get the components
        _agent = GetComponent<NavMeshAgent>();
        _rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (collisionTags.Contains(other.transform.tag)) // If the collision contains one of the tag in the list
        {
            _look = other.transform;
            StartCoroutine(RenableIn(recover)); // Start the corutine for recovering after the bounce
            _rb.AddForce(-transform.forward * bumpForce, ForceMode.Impulse); //bump backward
            

            if(other.transform.TryGetComponent<Rigidbody>(out Rigidbody rb)) // Making sure the object of collision has a rigidbody
            {
                
                rb.AddForce(transform.forward * pushForce, ForceMode.Impulse); // push forward the rigidbody of other collision
            }
        }

        
    }

    private void FixedUpdate()
    {
        if (lookAtTarget 
            && _hasCollide)
        {
            transform.LookAt(_look.transform);
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
        _hasCollide = true;

        yield return new WaitForSeconds(time); // Wait before renenable

        _hasCollide = false;

        _rb.velocity = Vector3.zero; // Set velocity to zero so the navmesh will work properly again
        _en.enabled = true;
        _agent.enabled = true;
    }
}
