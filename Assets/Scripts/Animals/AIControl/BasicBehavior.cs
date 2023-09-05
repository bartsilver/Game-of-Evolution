using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBehavior : MonoBehaviour
{
    State currentState;
    Mover mover;
    BasicStats basicStats;
    Food food;

    Vector3 targetDestination;

    private void Awake()
    {
        mover = GetComponent<Mover>();
        basicStats = GetComponent<BasicStats>();
    }

    private void Update()
    {
        Debug.Log(currentState);
        switch (currentState)
        {
            case State.LookingForFood:
                LookingForFood();
                //Debug.Log(targetDestination);
                break;

            case State.MovingToFood:
                MovingToFood();
                //Debug.Log(food.name);
                break;

            case State.LookingForMate:

            case State.MovingToMate:

            default:
                return;
            

               
        }
    }

    private void MovingToFood()
    {
        targetDestination = food.transform.position;
        mover.MoveTo(targetDestination);
        if (Vector3.Distance(transform.position, food.transform.position) < 2f)
        {
            Eat();
            currentState = State.LookingForFood;
        }
    }

    private void Eat()
    {
        food.GetEaten();      
    }

    private void LookingForFood()
    {
        CheckForFood(out bool didFindFood, out food);
        Debug.Log(didFindFood, food);

        if (didFindFood)
        {
            currentState = State.MovingToFood;
        }
        else
        {
            if (targetDestination == Vector3.zero)
            {
                targetDestination = mover.ChooseRandomDestination(basicStats.Sight);
                mover.MoveTo(targetDestination);
            }

            if (AtWaypoint())
            {
                targetDestination = mover.ChooseRandomDestination(basicStats.Sight);
                mover.MoveTo(targetDestination);
            }

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
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, basicStats.Sight, Vector3.up, 0);
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
}
