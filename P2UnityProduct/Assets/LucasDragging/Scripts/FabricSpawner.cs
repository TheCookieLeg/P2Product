using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class FabricSpawner : MonoBehaviour
{
    public GameObject fabric;
    public GameObject Scorer;
    [SerializeField] private GameUI gameUIScript;
    [SerializeField, Tooltip("The distance between each piece of fabric")] private float distance = 1f;
    [SerializeField, Tooltip("Number of fabrics to spawn")] private int repetitions;

    private List<GameObject> fabricPositions = new List<GameObject>();
    private int nextScorerIndex = 0;
    
    private int scorerBaseIndex = 0;
    private bool moveForward = true;
    void Start()
    {
        Vector3 fabricSpawnPoint = transform.position;

        // Spawn fabrics and store their positions
        for (int i = 0; i < repetitions; i++)
        {
            fabricPositions.Add(Instantiate(fabric, fabricSpawnPoint, quaternion.identity, gameObject.transform));
            fabricSpawnPoint += new Vector3(distance, 0, 0);
        }
        SpawnScorerFirstTime();
    }

    public void SpawnScorer()
    {
        if (scorerBaseIndex < 0 || scorerBaseIndex + 1 >= fabricPositions.Count)
        {
            Debug.LogWarning("Out of bounds - can't spawn scorer.");
            gameUIScript.ExitLevel();
            return;
        }

        Vector3 posA = fabricPositions[scorerBaseIndex].transform.position;
        Vector3 posB = fabricPositions[scorerBaseIndex + 1].transform.position;
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
        Vector3 posA = fabricPositions[nextScorerIndex].transform.position;
        Vector3 posB = fabricPositions[nextScorerIndex + 1].transform.position;
        Vector3 midpoint = (posA + posB) / 2;

        Instantiate(Scorer, midpoint, quaternion.identity, gameObject.transform);
        //nextScorerIndex++;
    }

    private bool isEven(int value)
    {
        return value % 2 == 0;
    }
}