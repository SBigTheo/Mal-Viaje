using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval = 4.5f;
    public int maxEnemies = 5;
    
    private Vector2[] spawnPositions = new Vector2[]
    {
        new Vector2(-12.4f, -2.2f),
        new Vector2(12.4f, -2.2f)
    };
    
    private float minDistanceBetweenEnemies = 3f;
    
    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }
    
    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            
            int currentEnemyCount = CountActiveEnemies();
            
            if (currentEnemyCount < maxEnemies)
            {
                TrySpawnEnemy();
            }
        }
    }
    
    int CountActiveEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        return enemies.Length;
    }
    
    void TrySpawnEnemy()
    {
        Vector2[] shuffledPositions = ShuffleSpawnPositions();
        
        foreach (Vector2 spawnPos in shuffledPositions)
        {
            if (IsSpawnPositionValid(spawnPos))
            {
                SpawnEnemyAtPosition(spawnPos);
                break;
            }
        }
    }
    
    bool IsSpawnPositionValid(Vector2 spawnPosition)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
            {
                float distance = Vector2.Distance(spawnPosition, enemy.transform.position);
                if (distance < minDistanceBetweenEnemies)
                {
                    return false;
                }
            }
        }
        return true;
    }
    
    void SpawnEnemyAtPosition(Vector2 position)
    {
        Instantiate(enemyPrefab, position, Quaternion.identity);
    }
    
    Vector2[] ShuffleSpawnPositions()
    {
        Vector2[] shuffled = (Vector2[])spawnPositions.Clone();
        for (int i = 0; i < shuffled.Length; i++)
        {
            Vector2 temp = shuffled[i];
            int randomIndex = Random.Range(i, shuffled.Length);
            shuffled[i] = shuffled[randomIndex];
            shuffled[randomIndex] = temp;
        }
        return shuffled;
    }
}