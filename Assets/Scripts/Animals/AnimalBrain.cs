using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Mover))]
[RequireComponent(typeof(BasicStats))]
public class AnimalBrain : MonoBehaviour
{
    State currentState = State.LookingForFood;
    Mover mover;
    Food foodFound;
    BasicStats basicStats;
    BasicStats mate;

    Vector3 targetDestination;

    private void Awake()
    {
        mover = GetComponent<Mover>();
        basicStats = GetComponent<BasicStats>();
    }
    void Update()
    {
        switch (currentState)
        {
            case State.LookingForFood:
                LookingForFood();
                break;

            case State.MovingToFood:
                MovingToFood();
                break;

            case State.LookingForMate:
                LookingForMate();
                break;

            case State.MovingToMate:
                MovingToMate();
                break;

            default:
                return;
        }
    }

    private void LookingForFood()
    {
        CheckForFood(out bool didFindFood, out foodFound);

        if (didFindFood)
        {
            currentState = State.MovingToFood;
        }
        else
        {
            if (targetDestination == Vector3.zero || AtWaypoint())
            {
                mover.FindRandomPoint(transform.position, basicStats.sight, out targetDestination);
                mover.MoveTo(targetDestination);
            }
        }
    }

    private void MovingToFood()
    {
        if (foodFound == null)
        {
            targetDestination = Vector3.zero;
            currentState = State.LookingForFood;
            return;
        }
        targetDestination = foodFound.transform.position;
        mover.MoveTo(targetDestination);
        if (Vector3.Distance(transform.position, foodFound.transform.position) < 2f)
        {
            Eat();
        }
    }

    private void LookingForMate()
    {
        if (basicStats.energy <= 70f)
        {
            targetDestination = Vector3.zero;
            currentState = State.LookingForFood;
        }
        CheckForMate(out bool didFindMate, out mate);

        if (didFindMate)
        {
            currentState = State.MovingToMate;
        }
        else
        {
            if (targetDestination == Vector3.zero || AtWaypoint())
            {
                mover.FindRandomPoint(transform.position, basicStats.sight, out targetDestination);
                mover.MoveTo(targetDestination);
            }
        }
    }

    private void MovingToMate()
    {
        if (mate == null)
        {
            targetDestination = Vector3.zero;
            currentState = State.LookingForMate;
            return;
        }
        targetDestination = mate.transform.position;
        mover.MoveTo(targetDestination);
        if (Vector3.Distance(transform.position, mate.transform.position) < 2f)
        {
            GetComponent<AnimalReproduction>().Reproduce();
            basicStats.energy = 50f;
            currentState = State.LookingForFood;
        }
    }

    private bool AtWaypoint()
    {
        float distanceToWaypoint = Vector3.Distance(transform.position, targetDestination);
        return distanceToWaypoint < 1.1f;
    }

    private void CheckForFood(out bool didFindFood, out Food foodFound)
    {
        Food foundFood = null;
        Food closestFood = null;
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, basicStats.sight, Vector3.up, 0);
        foreach (RaycastHit hit in hits)
        {
            foundFood = hit.collider.GetComponent<Food>();
            if (foundFood == null) continue;

            if (closestFood == null || Vector3.Distance(transform.position, foundFood.transform.position) < Vector3.Distance(transform.position, closestFood.transform.position))
            {
                closestFood = foundFood;
            }
        }
        if (closestFood != null)
        {
            didFindFood = true;
            foodFound = closestFood;
        }
        else
        {
            didFindFood = false;
            foodFound = null;
        }
    }

    private void CheckForMate(out bool didFindMate, out BasicStats mateFound)
    {
        BasicStats foundMate = null;
        BasicStats closestMate = null;
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, basicStats.sight, Vector3.up, 0);
        foreach (RaycastHit hit in hits)
        {
            foundMate = hit.collider.GetComponent<BasicStats>();
            if (foundMate == null) continue;
            if (foundMate.sex == basicStats.sex) continue;

            if (closestMate == null || Vector3.Distance(transform.position, foundMate.transform.position) < Vector3.Distance(transform.position, closestMate.transform.position))
            {
                closestMate = foundMate;
            }
        }
        if (closestMate != null)
        {
            didFindMate = true;
            mateFound = closestMate;
        }
        else
        {
            didFindMate = false;
            mateFound = null;
        }
    }

    private void Eat()
    {
        foodFound.GetEaten(out float nutrition);
        basicStats.energy += nutrition;

        if (basicStats.energy > 100) basicStats.energy = 100f;

        if (basicStats.energy > 99f)
        {
            currentState = State.LookingForMate;
        }
        else
        {
            currentState = State.LookingForFood;
        }
    }

}
