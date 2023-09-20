using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicStats : MonoBehaviour, IStats
{
    [SerializeField] Material femaleMaterial;
    [SerializeField] Material maleMaterial;
    private float maxEnergy;
    public float energy = 100f;

    public float speed = 3f;
    public float sight = 10f;
    public float testNumber = 5f;
    public Sex sex;

    public Dictionary<string, float> stats = new Dictionary<string, float>()
    {
        {"speed", 5 },
        {"sight", 20 },
        {"testNumber", 5f }
    };


    private void Awake()
    {
        InitializeStats();
        sex = RandomSex();
        if (sex == Sex.Male)
        {
            GetComponent<MeshRenderer>().material = maleMaterial;
        }
        else
        {
            GetComponent<MeshRenderer>().material = femaleMaterial;
        }

        energy = 50f;
    }

    private void Update()
    {
        energy -= Time.deltaTime * 1f;
        if (energy <= 0f) Die();
    }

    private Sex RandomSex()
    {
        int i = Random.Range(1, 10);
        if (i <= 5)
        {
            return Sex.Male;
        }
        else
        {
            return Sex.Female;
        }
    }

    public void InitializeStats()
    {
        speed = stats["speed"];
        sight = stats["sight"];
        testNumber = stats["testNumber"];
    }

    private void Die()
    {
        Destroy(gameObject);
    }

}
