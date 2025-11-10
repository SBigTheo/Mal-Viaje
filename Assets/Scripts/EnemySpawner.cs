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
    private bool isSpawning = true;
    
    void Start()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("EnemyPrefab no está asignado en el Inspector!", this);
            return;
        }
        
        StartCoroutine(SpawnEnemies());
    }
    
    IEnumerator SpawnEnemies()
{
    while (isSpawning)
    {
        yield return new WaitForSecondsRealtime(spawnInterval);
        
        if (this == null || !isSpawning) yield break;
        
        if (Time.timeScale > 0)
        {
            int currentEnemyCount = CountActiveEnemies();
            
            if (currentEnemyCount < maxEnemies)
            {
                TrySpawnEnemy();
            }
        }
    }
}
    
    int CountActiveEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        int count = 0;
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null) count++;
        }
        return count;
    }
    
    void TrySpawnEnemy()
    {
        if (enemyPrefab == null)
        {
            Debug.LogWarning("EnemyPrefab es null, no se puede spawnear enemigo");
            return;
        }
        
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
            // Verificación más robusta
            if (enemy == null) continue;
            
            float distance = Vector2.Distance(spawnPosition, enemy.transform.position);
            if (distance < minDistanceBetweenEnemies)
            {
                return false;
            }
        }
        return true;
    }
    
    void SpawnEnemyAtPosition(Vector2 position)
    {
        if (enemyPrefab != null)
        {
            Instantiate(enemyPrefab, position, Quaternion.identity);
        }
        else
        {
            Debug.LogError("No se puede instanciar - enemyPrefab es null!");
        }
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
    

    public void StopSpawning()
    {
        isSpawning = false;
        StopAllCoroutines();
    }


    void OnDestroy()
    {
        isSpawning = false;
        StopAllCoroutines();
    }

    void OnDisable()
    {
        StopSpawning();
    }
}