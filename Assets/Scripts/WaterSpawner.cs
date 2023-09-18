using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSpawner : MonoBehaviour
{
    public GameObject WaterPrefab;
    public float SpawnTime = 1.0f;
    public int MaxObjects = 10;

    private List<GameObject> _spawnedObjects = new List<GameObject>();
    private bool _stopSpawning = false;
    private Vector3 _initialPosition;

    // Use this for initialization
    void Start()
    {
        _initialPosition = transform.position;
        StartCoroutine(WaterWave());
    }

    private void SpawnWater(Vector3 position)
    {
        if (!_stopSpawning && _spawnedObjects.Count < MaxObjects)
        {
            GameObject water = Instantiate(WaterPrefab, position, Quaternion.identity);
            _spawnedObjects.Add(water);
        }
        else if (_spawnedObjects.Count >= MaxObjects)
        {
            _stopSpawning = true;
        }
    }

    IEnumerator WaterWave()
    {
        while (!_stopSpawning)
        {
            yield return new WaitForSeconds(SpawnTime);
            SpawnWater(_initialPosition);
        }
    }
}
