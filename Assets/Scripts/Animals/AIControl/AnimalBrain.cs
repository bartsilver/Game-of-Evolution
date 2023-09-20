using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Mover))]
[RequireComponent(typeof(BasicStats))]
[RequireComponent(typeof(BehaviorMethods))]
public class AnimalBrain : MonoBehaviour
{
    State currentState = State.LookingForFood;
    Mover mover;
    Food foodFound;
    BasicStats basicStats;
    BasicStats mate;

    BehaviorMethods behaviorMethods;

    Vector3 targetDestination;

    private void Awake()
    {
        behaviorMethods = GetComponent<BehaviorMethods>();
        mover = GetComponent<Mover>();
        basicStats = GetComponent<BasicStats>();
    }
    void Update()
    {
        Behave();
    }

    private void Behave()
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
        behaviorMethods.CheckForFood(basicStats, out bool didFindFood, out foodFound);

        if (didFindFood)
        {
            currentState = State.MovingToFood;
        }
        else
        {
            if (targetDestination == Vector3.zero || AtWaypoint())
            {
                mover.FindRandomPoint(transform.position, basicStats.sight, out targetDestination);
                mover.MoveTo(targetDestination, basicStats.speed);
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
        mover.MoveTo(targetDestination, basicStats.speed);
        if (Vector3.Distance(transform.position, foodFound.transform.position) < 2f)
        {
            currentState = behaviorMethods.Eat(foodFound, basicStats);
        }
    }

    private void LookingForMate()
    {
        if (basicStats.energy <= 70f)
        {
            targetDestination = Vector3.zero;
            currentState = State.LookingForFood;
        }

        behaviorMethods.CheckForMate(basicStats, out bool didFindMate, out mate);

        if (didFindMate)
        {
            currentState = State.MovingToMate;
        }
        else
        {
            if (targetDestination == Vector3.zero || AtWaypoint())
            {
                mover.FindRandomPoint(transform.position, basicStats.sight, out targetDestination);
                mover.MoveTo(targetDestination, basicStats.speed);
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
        mover.MoveTo(targetDestination, basicStats.speed);
        if (Vector3.Distance(transform.position, mate.transform.position) < 2f)
        {
            AnimalReproductionSingleton.Instance.Reproduce(basicStats, mate);

            //GetComponent<AnimalReproduction>().Reproduce();
            basicStats.energy = 50f;
            currentState = State.LookingForFood;
        }
    }

    private bool AtWaypoint()
    {
        float distanceToWaypoint = Vector3.Distance(transform.position, targetDestination);
        return distanceToWaypoint < 1.1f;
    }

}
