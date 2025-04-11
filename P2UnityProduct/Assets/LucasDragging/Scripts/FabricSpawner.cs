using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class FabricSpawner : MonoBehaviour
{
    public GameObject fabric;
    public GameObject Scorer;
    [SerializeField, Tooltip("The distance between each piece of fabric")] private float distance = 1f;
    [SerializeField, Tooltip("Number of fabrics to spawn")] private int repetitions;

    private List<Vector3> fabricPositions = new List<Vector3>();
    private int nextScorerIndex = 0;
    private int index = 0;
    
    private int scorerBaseIndex = 0;
    private bool moveForward = true;
    void Start()
    {
        Vector3 fabricSpawnPoint = transform.position;

        // Spawn fabrics and store their positions
        for (int i = 0; i < repetitions; i++)
        {
            Instantiate(fabric, fabricSpawnPoint, quaternion.identity, gameObject.transform);
            fabricPositions.Add(fabricSpawnPoint);
            fabricSpawnPoint += new Vector3(distance, 0, 0);
        }
        SpawnScorerFirstTime();
    }

    public void SpawnScorer()
    {
        if (scorerBaseIndex < 0 || scorerBaseIndex + 1 >= fabricPositions.Count)
        {
            Debug.LogWarning("Out of bounds - can't spawn scorer.");
            return;
        }

        Vector3 posA = fabricPositions[scorerBaseIndex];
        Vector3 posB = fabricPositions[scorerBaseIndex + 1];
        Vector3 midpoint = (posA + posB) / 2;

        Instantiate(Scorer, midpoint, quaternion.identity, gameObject.transform);
        
        if (moveForward)
        {
            scorerBaseIndex += 2;
        }
        else
        {
            scorerBaseIndex -= 1;
        }

        moveForward = !moveForward; // Flip direction every time
    }

    private void SpawnScorerFirstTime()
    {
        Vector3 posA = fabricPositions[nextScorerIndex];
        Vector3 posB = fabricPositions[nextScorerIndex + 1];
        Vector3 midpoint = (posA + posB) / 2;

        Instantiate(Scorer, midpoint, quaternion.identity, gameObject.transform);
        //nextScorerIndex++;
    }

    private bool isEven(int value)
    {
        return value % 2 == 0;
    }
}