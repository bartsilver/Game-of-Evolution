using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField] float nutrition;
    public void GetEaten(out float nutrition)
    {
        nutrition = GetComponent<PlantStats>().nutrition;
        Destroy(gameObject);
    }
}
