using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyStateMachine : MonoBehaviour
{
    [Header("Objective Tag")]
    public string objectiveTag = "EnemyFlag";

    [Header("Navigation")]
    [Range(10, 100)] public float detectionRange = 20f;
    [Range(1, 50)] public float speed = 3.5f, acceleration = 8f;
    [Range(0, 10)] public float stoppingDistance = 1f;

    private Vector3 _originPosition, _destination;
    private Transform _player, _closestFlag;
    private float _achievement;
    private NavMeshAgent _agent;
    private float _navmeshRange;

    public enum AIState { CHASE, GRABFLAG, PATROL };
    private AIState _currentState = AIState.PATROL;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();

        _agent.speed = speed;
        _agent.acceleration = acceleration;
        _agent.stoppingDistance = stoppingDistance;

        _player = GameObject.FindGameObjectWithTag("Player").transform;

        _originPosition = SetWorldOrigin(out float range);
        _navmeshRange = range;

        GoToLocation(RandomPoint());
    }

    private void Update()
    {

#if DEBUG
        Debug.Log(_currentState);
#endif

        if (CanReachLocation(transform.position, _player.position, detectionRange))
        {
            _currentState = AIState.CHASE;
        }
        else
        {
            if (UnityExtension.TryGetClosestGameObjectWithTag(transform, objectiveTag, out GameObject closest)
                && CanReachLocation(transform.position, closest.transform.position, detectionRange))
            {
                _closestFlag = closest.transform;
                _currentState = AIState.GRABFLAG;
            }
            else
            {
                 _currentState = AIState.PATROL;
            }
        }

        switch (_currentState)
        {
            case AIState.CHASE:
                Chase();
                break;
            case AIState.GRABFLAG:
                GrabFlag();
                break;
            default:
                Patrol();
                break;
        }
    }

    #region State Machine Tasks
    private void Patrol()
    {
        if (Vector3.Distance(transform.position, _destination) < _agent.stoppingDistance + 1f)
        {
            GoToLocation(RandomPoint());
        }
    }

    private void GrabFlag()
    {
        GoToLocation(_closestFlag.position);
    }

    private void Chase()
    {
        GoToLocation(_player.position);
    }
    #endregion
    private void GoToLocation(Vector3 position)
    {
        _agent.SetDestination(position);
        _destination = position;
    }

    #region Condition Methods

    private bool CanReachLocation(Vector3 from, Vector3 to, float range)
    {
        bool r = Vector3.Distance(from, to) <= range
            && !NavMesh.Raycast(from, to, out NavMeshHit hit, NavMesh.AllAreas);

#if DEBUG
        Color color = r ? Color.green : Color.red;
        Debug.DrawLine(from, to, color);
#endif

        return r;
    }
    #endregion

    private Vector3 RandomPoint()
    {
        NavMeshHit hit;

        Vector3 randomPoint = _originPosition + Random.insideUnitSphere * _navmeshRange;

        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return transform.position;
    }

    private Vector3 SetWorldOrigin(out float radius)
    {
        TerrainData data = FindObjectOfType<Terrain>().terrainData;

        float width = data.size.x;
        float height = data.size.z;

        radius = Mathf.Sqrt(Mathf.Pow(width / 2, 2) + Mathf.Pow(height / 2, 2));

        return new Vector3(width / 2, 0, height / 2);
    }
}
