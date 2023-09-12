using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantStats : MonoBehaviour, IStats
{
    public float maxEnergy = 100f;
    public float energy = 50f;
    public float nutrition;

    private void Awake()
    {
        energy = maxEnergy / 2f;
    }
    private void Update()
    {
        if (energy < maxEnergy)
        {
            energy += Time.deltaTime * 2f;
        }
        nutrition = energy / 5f;

        if (energy >= maxEnergy)
        {
            GetComponent<PlantReproduction>().Reproduce();
            energy = maxEnergy / 2f;
        }
    }
}
