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


    /*public Vector3 ChooseRandomDestination(float seeDistance)
    {
        Vector3 randDirection = Random.insideUnitSphere * seeDistance;

        randDirection += transform.position;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, seeDistance, -1);

        if (!navHit.hit) ChooseRandomDestination(seeDistance);

        NavMeshPath path = new NavMeshPath();
        this.navMeshAgent.CalculatePath(navHit.position, path);
        //bool canReachPoint = path.status == NavMeshPathStatus.PathComplete;
        bool canReachPoint = false;
        if (path.status == NavMeshPathStatus.PathComplete)
            canReachPoint = true;

        if (!canReachPoint)
        {
            ChooseRandomDestination(seeDistance);
        }

        targetDestination = navHit.position;
        return targetDestination;
    }*/

        public void MoveTo(Vector3 destination)
    {
        navMeshAgent.destination = destination;
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
