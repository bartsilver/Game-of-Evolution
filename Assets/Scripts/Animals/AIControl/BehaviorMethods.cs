using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorMethods : MonoBehaviour
{

    public void CheckForFood(BasicStats basicStats, out bool didFindFood, out Food foodFound)
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

    public void CheckForMate(BasicStats basicStats, out bool didFindMate, out BasicStats mateFound)
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


    public State Eat(Food foodFound, BasicStats basicStats)
    {
        foodFound.GetEaten(out float nutrition);
        basicStats.energy += nutrition;

        if (basicStats.energy > 100) basicStats.energy = 100f;

        if (basicStats.energy > 99f)
        {
            return State.LookingForMate;
        }
        else
        {
            return State.LookingForFood;
        }
    }
}
