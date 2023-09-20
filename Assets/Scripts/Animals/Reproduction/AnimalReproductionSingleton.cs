using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimalReproductionSingleton : MonoBehaviour
{
    public static AnimalReproductionSingleton Instance;

    [SerializeField] float reproductionDistance = 5f;
    [SerializeField] float percentageChange = 10f;
    [SerializeField] GameObject newAnimalPrefab;

    Dictionary<string, float> newAverage;
    Dictionary<string, float> newstats;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void Reproduce(BasicStats thisAnimal, BasicStats mate)
    {
        Vector3 targetArea = FindTargetArea(thisAnimal);
        BasicStats newAnimalStats;

        GameObject newAnimal =  Instantiate(newAnimalPrefab, targetArea, Quaternion.identity);

        newAverage = new Dictionary<string, float>();
        newstats = new Dictionary<string, float>();

        foreach (KeyValuePair<string, float> thisAnimalStats in thisAnimal.stats)
        {
            float thisValue;
            float mateValue;

            thisAnimal.stats.TryGetValue(thisAnimalStats.Key, out thisValue);
            mate.stats.TryGetValue(thisAnimalStats.Key, out mateValue);
            newAverage.Add(thisAnimalStats.Key, FindAverageValue(thisValue, mateValue));
        }
        
        newAnimalStats = newAnimal.GetComponent<BasicStats>();

        foreach (KeyValuePair<string, float> stats in newAnimalStats.stats)
        {
            float averageValue;
            newAverage.TryGetValue(stats.Key, out averageValue);
            newstats.Add(stats.Key, GenerateRandomProperty(averageValue));

        }
        newAnimalStats.stats = newstats;
        newAnimalStats.InitializeStats();

    }

    private Vector3 FindTargetArea(BasicStats thisAnimal)
    {
        Vector3 result = Vector3.zero;

        while (result == Vector3.zero)
        {
            FindRandomPoint(thisAnimal.transform.position, reproductionDistance, out result);
        }

        return result;
    }

    private void FindRandomPoint(Vector3 center, float seeDistance, out Vector3 result)
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

    /*private BasicStats GenerateNewAnimal(BasicStats thisAnimal, BasicStats mate)
    {
        BasicStats generatedStats = new BasicStats();
        float speed = FindAverageValue(thisAnimal.speed, mate.speed);
        float sight = FindAverageValue(thisAnimal.sight, mate.sight);

        generatedStats.speed = Random.Range(speed - ((speed / 100) * percentageChange), speed + ((speed / 100) * percentageChange));
        generatedStats.sight = Random.Range(sight - ((sight / 100) * percentageChange), sight + ((sight / 100) * percentageChange));

        return generatedStats;
    }*/

    private float GenerateRandomProperty(float average)
    {
        return Random.Range(average - ((average / 100) * percentageChange), average + ((average / 100) * percentageChange));
    }

    private float FindAverageValue(float a, float b)
    {
        return (a + b) / 2;
    }
}
