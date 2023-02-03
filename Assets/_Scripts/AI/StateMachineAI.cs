using Extension;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// This script is a state machine for an enemy AI in a Unity game.
/// It uses the NavMeshAgent component to navigate the game world.
/// The AI has three states: CHASE, GRABOBJECTIVE, and PATROL.
/// The AI will chase the player if they are within a certain range,
/// grab an objective if one is within range, and patrol randomly if neither of those conditions are met.
/// The script also includes methods for checking if the AI can reach a location and for setting a
/// random destination for the AI to move to.
/// </summary>

[RequireComponent(typeof(NavMeshAgent))]
public class StateMachineAI : MonoBehaviour
{
    public enum AIBehaviour
    {
        HUNTER, // Only chase the player
        SEEKER, // Only grab the flags
        FLEX // Do all
    };

    // Tags
    [Header("Objectives")]

    [Tooltip("The tag of the GameObject the AI must find and grab")]
    public string objectiveTag = Constants.Tags.ENEMY_FLAG;

    [Tooltip("The tag of the GameObject the AI must chase")]
    public string toChaseTag = Constants.Tags.PLAYER;

    // Declarations of the navigation variables
    [Header("Navigation")]
    public AIBehaviour behaviour = AIBehaviour.FLEX;

    [Tooltip("Path Range of the AI"), Range(1, 100f)]
    public float pathRange = 100f;

    [Tooltip("The detection range of the AI"), Range(10, 100)]
    public float detectionRange = 20f;

    [Tooltip("The speed of the AI"), Range(1, 100)]
    public float speed = 3.5f;

    [Tooltip("The accelearation of the AI"), Range(1, 50)] 
    public float acceleration = 8f;

    [Tooltip("Set the eyes height")]
    public float heightEyesOffset = 0f;

    private float _stoppingDistance = 1f;
    private Vector3 _destination;
    private Transform _toChase, _closestObjective;
    private NavMeshAgent _agent;

    public enum AIState { CHASE, GRABOBJECTIVE, PATROL };
    private AIState _currentState = AIState.PATROL;


    public AIState GetCurrentState { get => _currentState; }

    private bool _targetIsVisible; // Checking if the target has activated the invisibility

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>(); // Get the agent and set up my variables

        _agent.speed = speed;
        _agent.acceleration = acceleration;
        _agent.stoppingDistance = _stoppingDistance;

        _toChase = GameObject.FindGameObjectWithTag(toChaseTag).transform; // find the player in the scene

        GoToLocation(RandomPoint()); // Go to a random location
    }

    private void Update()
    {
        // log the current state of the AI in the console (only enabled in DEBUG mode)
#if DEBUG
        //Debug.Log(behaviour + " is " + _currentState);
#endif

        if(_toChase.transform.TryGetComponent<PlayerInventory>(out PlayerInventory pli)){
            _targetIsVisible = !pli.IsInvisible;
        }

        bool canSeeTarget = CanSeeLocation(transform.position, _toChase.position, heightEyesOffset, detectionRange) && _targetIsVisible;
        bool canReachObjective = Finder.TryGetClosestGameObjectWithTag(transform, objectiveTag, out GameObject objectiveGameObject)
            && CanSeeLocation(transform.position, objectiveGameObject.transform.position, heightEyesOffset, detectionRange);

        switch (behaviour)
        {

            case AIBehaviour.HUNTER:

                // Check if the player is within the detection range
                if (canSeeTarget)
                {
                    _currentState = AIState.CHASE; // If so, set the state to CHASE
                }
                else if (canReachObjective) // Check if there is an objective within range
                {

                    // If so, set the closest objective as the target and set the state to GRABOBJECTIVE
                    _closestObjective = objectiveGameObject.transform;
                    _currentState = AIState.GRABOBJECTIVE;
                }
                else
                {
                    _currentState = AIState.PATROL; // If neither condition is met, set the state to PATROL
                }
            break;

            case AIBehaviour.SEEKER:

                if (canReachObjective)
                {
                    _closestObjective = objectiveGameObject.transform;
                    _currentState = AIState.GRABOBJECTIVE;
                }
                else
                    _currentState = AIState.PATROL;

            break;

            case AIBehaviour.FLEX:

                if (canReachObjective)
                {
                    // If so, set the closest objective as the target and set the state to GRABOBJECTIVE
                    _closestObjective = objectiveGameObject.transform;
                    _currentState = AIState.GRABOBJECTIVE;
                }
                else if (canSeeTarget)
                    _currentState = AIState.CHASE;// If so, set the state to CHASE
                else
                    _currentState = AIState.PATROL;
 

            break;
        }

        // Switch statement to perform the appropriate task based on the current state
        switch (_currentState)
        {
            case AIState.CHASE:
                Chase();
                break;
            case AIState.GRABOBJECTIVE:
                GrabObjective();
                break;
            default:
                Patrol();
                break;
        }

    }


    #region Tasks

    /// <summary>
    /// Check if the AI is close to its destination
    /// If so, set a new random destination for the AI
    /// </summary>
    private void Patrol()
    {
        // Check if the AI is close to its destination
        if (Vector3.Distance(transform.position, _destination) < _agent.stoppingDistance + 1f)
        {
            // If so, set a new random destination for the AI
            GoToLocation(RandomPoint());
        }
    }

    /// <summary>
    /// Set the destination to the position of the closest objective
    /// </summary>
    private void GrabObjective()
    {
        GoToLocation(_closestObjective.position);
    }

    /// <summary>
    /// Set the destination to the position of the player
    /// </summary>
    private void Chase()
    {
        GoToLocation(_toChase.position);
    }

    public void Stay()
    {
        GoToLocation(transform.position);
    }

    #endregion


    #region Conditions

    /// <summary>
    /// Check if the agent can see the location
    /// </summary>
    /// <param name="from">transform of the origin raycast source</param>
    /// <param name="to">transform of destination source</param>
    /// <param name="range">range within can check</param>
    /// <returns></returns>
    private bool CanSeeLocation(Vector3 from, Vector3 to, float offset, float range)
    {
        bool r = Vector3.Distance(from, to) <= range
            && !NavMesh.Raycast(new Vector3(from.x, from.y + offset, from.z), to, out NavMeshHit hit, NavMesh.AllAreas);

#if DEBUG
        Color color = r ? Color.green : Color.red;
        Debug.DrawLine(new Vector3(from.x, from.y + heightEyesOffset, from.z), to, color);
#endif

        return r;
    }


    /// <summary>
    /// Check if the agent can calculate a path to reach the destination position
    /// </summary>
    /// <param name="point">point i want to reach</param>
    /// <param name="finalDestination">final destination</param>
    /// <returns>True if it can reach the point, False if not</returns>
    private bool CanReachLocation(Vector3 point, out Vector3 finalDestination)
    {
        NavMeshPath path = new NavMeshPath();

        if (NavMesh.SamplePosition(point, out NavMeshHit hit, 1.0f, NavMesh.AllAreas) // If the destination point is on the navmesh
            && _agent.CalculatePath(hit.position, path)
            && path.status == NavMeshPathStatus.PathComplete) // And if the path with the point is found and Complete
        {
            finalDestination = hit.position;
            return true;
        }

        finalDestination = Vector3.zero;
        return false;
    }

    #endregion


    #region Behaviour

    /// <summary>
    /// Set the behaviour of the AI
    /// </summary>
    /// <param name="behaviour"></param>
    public void SetBehaviour(AIBehaviour behaviour)
    {
        this.behaviour = behaviour;
    }

    #endregion


    #region Position

    /// <summary>
    /// Tell the NavMeshAgent to move to the new destination
    /// </summary>
    /// <param name="position">the position i want to set for _destination</param>
    private void GoToLocation(Vector3 position)
    {
        _agent.SetDestination(position);
        _destination = position;
    }

    /// <summary>
    /// Get a random point on the NavMesh
    /// </summary>
    /// <returns>A random point on the NavMesh</returns>
    private Vector3 RandomPoint()
    {
        // I set the randompoint from the transform origin, then i add a random point in a sphere
        // and i mulitply it by path range
        Vector3 randomPoint = transform.position + Random.insideUnitSphere * pathRange;

        if (CanReachLocation(randomPoint, out Vector3 dest)) // if the path with the point is found and Complete
            return dest;

        return transform.position; // otherwise stay there
    }

    #endregion



}