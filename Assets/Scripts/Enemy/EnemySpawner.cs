using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject[] enemies;
    [SerializeField] Vector3[] spawnLocations;
    [SerializeField] float minTime, maxTime;

    

    void Start()
    {
        StartCoroutine("SpawnEnemies");
    }

    void Update()
    {
        if (Input.GetKeyDown("p"))
        {
            StopCoroutine("SpawnEnemies");
        }
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            int x = Random.Range(0, enemies.Length);                            // enemy
            int y = Random.Range(0, spawnLocations.Length);                     // location

            Instantiate(enemies[x], spawnLocations[y], Quaternion.identity);

            yield return new WaitForSeconds(Random.Range(minTime, maxTime));    // wait time
        }
    }
}
