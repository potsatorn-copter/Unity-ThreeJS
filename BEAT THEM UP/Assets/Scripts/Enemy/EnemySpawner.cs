using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;


public class EnemySpawner : MonoBehaviour
{ 
    public GameObject punkEnemyPrefab; // the prefab for the enemy
    public GameObject bigPunkEnemyPrefab;
    public int waveSize = 10; // the number of enemies to spawn
    public float spawnRadius = 5f; // the radius in which enemies can spawn
    public Transform spawnPoint; // the spawn point for the enemies
    public float spawnInterval = 5f; // the time between enemy waves
    public int maxEnemyCount = 10; // maximum number of active enemies

    private int currentEnemyCount = 0; // current number of active enemies
    private float timeUntilNextWave; // the time until the next enemy wave spawns

private List<GameObject> punkEnemies = new List<GameObject>();
private List<GameObject> bigPunkEnemies = new List<GameObject>();

void Start()
{
    timeUntilNextWave = spawnInterval; // set the time until the next wave to the spawn interval
    
}

void OnDrawGizmosSelected()
{
    // draw a wire sphere to show the spawn radius
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(spawnPoint.position, spawnRadius);
}

void Update()
{
    if (currentEnemyCount < maxEnemyCount)
    {
        for (int i = currentEnemyCount; i < Mathf.Min(waveSize + currentEnemyCount, maxEnemyCount); i++)
        {
            GameObject punkEnemy = Instantiate(punkEnemyPrefab);
            punkEnemy.SetActive(false);
            punkEnemies.Add(punkEnemy);

            GameObject bigPunkEnemy = Instantiate(bigPunkEnemyPrefab);
            bigPunkEnemy.SetActive(false);
            bigPunkEnemies.Add(bigPunkEnemy);

            currentEnemyCount++;
        }
    }
    
    timeUntilNextWave -= Time.deltaTime; // subtract the time since the last frame from the time until the next wave

    if (timeUntilNextWave <= 0)
    {
        SpawnWave(); // spawn a new wave of enemies
        timeUntilNextWave = spawnInterval; // reset the time until the next wave to the spawn interval
    }
}

void SpawnWave()
{
    int spawnedCount = 0; // initialize the count of spawned enemies to 0
    while (spawnedCount < 5) // spawn enemies until 5 enemies are spawned
    {
        // calculate a random position within the spawn radius
        Vector3 spawnPosition = spawnPoint.position + Random.insideUnitSphere * spawnRadius;

        if (SpawnFromPool(punkEnemies, spawnPosition, Quaternion.identity))
        {
            spawnedCount++; // increment the count of spawned enemies
        }
        else if (SpawnFromPool(bigPunkEnemies, spawnPosition, Quaternion.identity))
        {
            spawnedCount++; // increment the count of spawned enemies
        }
        // else, continue the loop to spawn another enemy
    }
}

bool SpawnFromPool(List<GameObject> pool, Vector3 position, Quaternion rotation)
{
    for (int i = 0; i < pool.Count; i++)
    {
        if (pool[i] != null && !pool[i].activeInHierarchy)
        {
            pool[i].transform.position = position;
            pool[i].transform.rotation = rotation;
            pool[i].SetActive(true);
            return true;
        }
    }

    return false;
}
}
