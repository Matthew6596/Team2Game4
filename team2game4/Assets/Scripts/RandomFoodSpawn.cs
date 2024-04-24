using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomFoodSpawn : MonoBehaviour
{
    public GameObject[] foodPrefabs;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(foodPrefabs[Random.Range(0, foodPrefabs.Length)],transform);
    }
}
