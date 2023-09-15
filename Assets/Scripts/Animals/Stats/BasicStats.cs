using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicStats : MonoBehaviour, IStats
{
    [SerializeField] Material femaleMaterial;
    [SerializeField] Material maleMaterial;
    public float energy = 100f;
    public float speed = 3f;
    public float sight = 10f;
    public Sex sex;


    private void Awake()
    {
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

    private void Die()
    {
        Destroy(gameObject);
    }

}
