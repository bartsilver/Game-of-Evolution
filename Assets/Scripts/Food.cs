using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField] float nutrition;
    public void GetEaten()
    {
        Destroy(gameObject);
    }
}
