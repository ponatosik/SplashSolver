using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scripts : MonoBehaviour
{
    public GameObject waterPrefab;
    public float spawnTime = 1.0f;
    public int maxObjects = 10;

    private List<GameObject> spawnedObjects = new List<GameObject>();
    private bool stopSpawning = false;
    private Vector3 initialPosition;

    // Use this for initialization
    void Start()
    {
        initialPosition = transform.position;
        StartCoroutine(waterWave());
    }

    private void spawnWater(Vector3 position)
    {
        if (!stopSpawning && spawnedObjects.Count < maxObjects)
        {
            GameObject water = Instantiate(waterPrefab, position, Quaternion.identity);
            spawnedObjects.Add(water);
        }
        else if (spawnedObjects.Count >= maxObjects)
        {
            stopSpawning = true;
        }
    }

    IEnumerator waterWave()
    {
        while (!stopSpawning)
        {
            yield return new WaitForSeconds(spawnTime);
            spawnWater(initialPosition);
        }
    }
}
