using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{
    [SerializeField] float waypointTolerance = 1.1f;

    NavMeshAgent navMeshAgent;
    Vector3 currentDestination;
    Vector3 targetDestination;
    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    public void FindRandomPoint (Vector3 center, float seeDistance, out Vector3 result)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * seeDistance;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, seeDistance, NavMesh.AllAreas))
        {
            result = hit.position;
            return;
        }
        result = Vector3.zero;
    }

        
    public void MoveTo(Vector3 destination, float speed)
    {
        navMeshAgent.destination = destination;
        navMeshAgent.speed = speed;

    }

    public bool AtWaypoint()
    {
        float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
        return distanceToWaypoint < waypointTolerance;
    }

    private Vector3 GetCurrentWaypoint()
    {
        return targetDestination;
    }
}
