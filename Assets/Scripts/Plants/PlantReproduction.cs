using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlantReproduction : MonoBehaviour
{
    [SerializeField] GameObject startingPlant;
    [SerializeField] float areaOfReproduction = 5f;

    public void Reproduce()
    {
        Instantiate(startingPlant, FindPlaceOnNavMesh(), Quaternion.identity);
    }

    private Vector3 FindPlaceOnNavMesh()
    {
        Vector3 randDirection = Random.insideUnitSphere * areaOfReproduction;

        randDirection += transform.position;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, areaOfReproduction, -1);

        if (!navHit.hit) FindPlaceOnNavMesh();

        Vector3 targetPosition = navHit.position;
        return targetPosition;
    }
}
