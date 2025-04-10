using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class FabricSpawner : MonoBehaviour
{
    public GameObject fabric;
    public GameObject Scorer;
    [SerializeField, Tooltip("The distance between each piece of fabric")] private float distance = 1f;
    [SerializeField, Tooltip("Number of fabrics to spawn")] private int repetitions;
    private Vector3 spawnPoint;

    void Start()
    {
        spawnPoint = transform.position;
        for (int i = 0; i < repetitions; i++)
        {
            Instantiate(fabric, spawnPoint, quaternion.identity);
            spawnPoint += new Vector3(distance / 2, 0, 0);
            Instantiate(Scorer, spawnPoint, quaternion.identity);
            spawnPoint += new Vector3(distance / 2, 0, 0);
        }
    }

}
