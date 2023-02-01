using UnityEngine;
using UnityEngine.AI;
using Extension;

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
    public enum WorldOriginGenerationType
    {
        TERRAIN,
        PLANE,
        CUSTOM
    };

    public enum AIBehaviour
    {
        HUNTER, // Only chase the player
        SEEKER, // Only grab the flags
        FLEX // Do all
    };

    // Tags
    [Header("Objectives")]
    [Tooltip("The tag of the GameObject the AI must find and grab")] public string objectiveTag = Constants.Tags.ENEMY_FLAG;
    [Tooltip("The tag of the GameObject the AI must chase")] public string toChaseTag = Constants.Tags.PLAYER;

    // Declarations of the navigation variables
    [Header("Navigation")]
    public AIBehaviour behaviour = AIBehaviour.FLEX;
    [Tooltip("The detection range of the AI")] [Range(10, 100)] public float detectionRange = 20f;
    /*[Tooltip("The speed of the AI")] [Range(1, 50)]*/ public float speed = 3.5f;
    [Tooltip("The accelearation of the AI")] [Range(1, 50)] public float acceleration = 8f;

    [HideInInspector, Tooltip("Set the eyes height")] public float heightEyesOffset = 0f;
    [HideInInspector] public Vector2 size;
    [HideInInspector] public WorldOriginGenerationType floorType;
    [HideInInspector] public Transform plane;
    [HideInInspector] public TerrainData terrainData;

    
    private float _stoppingDistance = 1f;
    private Vector3 _originPosition, _destination; 
    private Transform _toChase, _closestObjective;
    private NavMeshAgent _agent;
    private float _navmeshRange;

    public enum AIState { CHASE, GRABOBJECTIVE, PATROL };
    private AIState _currentState = AIState.PATROL;
    

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>(); // Get the agent and set up my variables

        _agent.speed = speed;
        _agent.acceleration = acceleration;
        _agent.stoppingDistance = _stoppingDistance;

        _toChase = GameObject.FindGameObjectWithTag(toChaseTag).transform; // find the player in the scene

        _originPosition = SetWorldOrigin(out float range); // Setting the world origin position and getting the range (radius) of the map
        _navmeshRange = range; 

        GoToLocation(RandomPoint()); // Go to a random location
    }

    private void Update()
    {
        // log the current state of the AI in the console (only enabled in DEBUG mode)
#if DEBUG
        Debug.Log(behaviour + " is " + _currentState);
#endif

        switch (behaviour)
        {

            case AIBehaviour.HUNTER:

                // Check if the player is within the detection range
                if (CanSeeLocation(transform.position, _toChase.position, heightEyesOffset, detectionRange))
                {
                    // If so, set the state to CHASE
                    _currentState = AIState.CHASE;
                }
                else
                {
                    // Check if there is an objective within range
                    if (Finder.TryGetClosestGameObjectWithTag(transform, objectiveTag, out GameObject alfaGO)
                        && CanSeeLocation(transform.position, alfaGO.transform.position, heightEyesOffset, detectionRange))
                    {
                        // If so, set the closest objective as the target and set the state to GRABOBJECTIVE
                        _closestObjective = alfaGO.transform;
                        _currentState = AIState.GRABOBJECTIVE;
                    }
                    else
                    {
                        // If neither condition is met, set the state to PATROL
                        _currentState = AIState.PATROL;
                    }
                }

                break;

            case AIBehaviour.SEEKER:

                if (Finder.TryGetClosestGameObjectWithTag(transform, objectiveTag, out GameObject omegaGO)
                    && CanSeeLocation(transform.position, omegaGO.transform.position, heightEyesOffset, detectionRange))
                {
                    _closestObjective = omegaGO.transform;
                    _currentState = AIState.GRABOBJECTIVE;
                }
                else
                {
                    // If neither condition is met, set the state to PATROL
                    _currentState = AIState.PATROL;
                }

                break;

            case AIBehaviour.FLEX:

                if (Finder.TryGetClosestGameObjectWithTag(transform, objectiveTag, out GameObject epsilonGO)
                        && CanSeeLocation(transform.position, epsilonGO.transform.position, heightEyesOffset, detectionRange))
                {
                    // If so, set the closest objective as the target and set the state to GRABOBJECTIVE
                    _closestObjective = epsilonGO.transform;
                    _currentState = AIState.GRABOBJECTIVE;
                }
                else
                {
                    if (CanSeeLocation(transform.position, _toChase.position, heightEyesOffset, detectionRange))
                    {
                        // If so, set the state to CHASE
                        _currentState = AIState.CHASE;
                    }
                    else
                    {
                        _currentState = AIState.PATROL;
                    }
                }

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
    public void Chase()
    {
        GoToLocation(_toChase.position);
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

    /// <summary>
    /// Check if the AI is a Hunter
    /// </summary>
    /// <returns></returns>
    private bool isHunter()
    {
        return behaviour == AIBehaviour.HUNTER || behaviour == AIBehaviour.FLEX;
    }

    /// <summary>
    /// Check if the AI is a Seeker
    /// </summary>
    /// <returns></returns>
    private bool isSeeker()
    {
        return behaviour == AIBehaviour.SEEKER || behaviour == AIBehaviour.FLEX;
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
        // I set the randompoint from the world origin, then i add a random point in a sphere
        // and i mulitply it by the radius of the navmesh
        Vector3 randomPoint = _originPosition + Random.insideUnitSphere * _navmeshRange;

        if (CanReachLocation(randomPoint, out Vector3 dest)) // if the path with the point is found and Complete
        {
            return dest;
        }

        return transform.position; // otherwise stay there
    }


    /// <summary>
    /// Set the world origin
    /// </summary>
    /// <param name="radius">the radius of the world</param>
    /// <returns>the position of the world origin</returns>
    private Vector3 SetWorldOrigin(out float radius)
    {
        float width, height;

        switch (floorType)
        {

            case WorldOriginGenerationType.TERRAIN:
                width = terrainData.size.x;
                height = terrainData.size.z;

                break;

            case WorldOriginGenerationType.PLANE:
                width = plane.lossyScale.x * 10f;
                height = plane.lossyScale.x * 10f;
                break;


            case WorldOriginGenerationType.CUSTOM:
                width = size.x;
                height = size.y;

                break;

            default:
                width = 100f;
                height = 100f;
                break;
        }

        width /= 2;
        height /= 2;

        radius = Mathf.Sqrt(Mathf.Pow(width, 2) + Mathf.Pow(height, 2)); // Math formula for calculating the radius of the map.

        /*
         * This lines here needs for testing, it checks if the radius generate is correct by creating a sphere at the center of the map.
        GameObject sphere =
            GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = new Vector3(width, 0, height);
        sphere.transform.localScale = new Vector3(radius*2, radius*2, radius*2); // multiply by 2 because this is the scale not the radius
        */

        return new Vector3(width, 0, height); // return the origin position
    }

    #endregion
}