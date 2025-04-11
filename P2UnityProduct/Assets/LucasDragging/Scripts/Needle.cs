using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Needle : MonoBehaviour
{
    public FabricSpawner spawner;

    private void Start()
    {
        spawner = GameObject.FindWithTag("Spawner").GetComponent<FabricSpawner>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Score"))
        {
            //Add some score
            Destroy(other.gameObject);
            spawner.SpawnScorer();
        }
    }
}
