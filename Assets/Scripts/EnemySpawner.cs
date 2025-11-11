using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject floorObject;
    public float spawnInterval = 4.5f;
    public int maxEnemies = 5;
    
    private float minDistanceBetweenEnemies = 3f;
    private bool isSpawning = true;
    private Vector2 floorSize;
    private Vector2 floorCenter;
    private List<GameObject> activeEnemies = new List<GameObject>();
    
    void Start()
    {
        if (enemyPrefab == null)
        {
            return;
        }
        
        if (floorObject != null)
        {
            CalculateFloorSpawnArea();
        }
        else
        {
            floorObject = GameObject.Find("Floor");
            if (floorObject != null)
            {
                CalculateFloorSpawnArea();
            }
            else
            {
                return;
            }
        }
        
        StartCoroutine(SpawnEnemies());
    }
    
    void CalculateFloorSpawnArea()
    {
        Collider2D floorCollider = floorObject.GetComponent<Collider2D>();
        if (floorCollider != null)
        {
            floorSize = floorCollider.bounds.size;
            floorCenter = floorCollider.bounds.center;
        }
        else
        {
            floorSize = floorObject.transform.localScale;
            floorCenter = floorObject.transform.position;
        }
    }
    
    IEnumerator SpawnEnemies()
    {
        while (isSpawning)
        {
            yield return new WaitForSeconds(spawnInterval);
            
            if (this == null || !isSpawning) yield break;
            
            CleanEnemyList();
            
            if (activeEnemies.Count < maxEnemies)
            {
                TrySpawnEnemy();
            }
        }
    }
    
    void CleanEnemyList()
    {

        for (int i = activeEnemies.Count - 1; i >= 0; i--)
        {
            if (activeEnemies[i] == null)
            {
                activeEnemies.RemoveAt(i);
            }
        }
    }
    
    void TrySpawnEnemy()
    {
        if (enemyPrefab == null || floorObject == null) return;
        
        for (int i = 0; i < 10; i++) 
        {
            Vector2 spawnPosition = GetRandomSpawnPositionOnFloor();
            
            if (IsSpawnPositionValid(spawnPosition))
            {
                SpawnEnemyAtPosition(spawnPosition);
                return; 
            }
        }
    }
    
    Vector2 GetRandomSpawnPositionOnFloor()
    {
        float randomX = Random.Range(
            floorCenter.x - floorSize.x / 2 + 1f,
            floorCenter.x + floorSize.x / 2 - 1f
        );
        
        float spawnY = floorCenter.y + floorSize.y / 2 + 0.5f;
        
        return new Vector2(randomX, spawnY);
    }
    
    bool IsSpawnPositionValid(Vector2 spawnPosition)
    {

        foreach (GameObject enemy in activeEnemies)
        {
            if (enemy == null) continue;
            
            float distance = Vector2.Distance(spawnPosition, enemy.transform.position);
            if (distance < minDistanceBetweenEnemies)
            {
                return false;
            }
        }
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(spawnPosition, player.transform.position);
            if (distanceToPlayer < 4f)
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
            GameObject newEnemy = Instantiate(enemyPrefab, position, Quaternion.identity);
            activeEnemies.Add(newEnemy);
        }
    }
    
    public void RemoveEnemy(GameObject enemy)
    {
        if (activeEnemies.Contains(enemy))
        {
            activeEnemies.Remove(enemy);
        }
    }
    
    void OnDrawGizmosSelected()
    {
        if (floorObject != null)
        {
            Gizmos.color = Color.green;
            Collider2D collider = floorObject.GetComponent<Collider2D>();
            if (collider != null)
            {
                Gizmos.DrawWireCube(collider.bounds.center, collider.bounds.size);
            }
            else
            {
                Gizmos.DrawWireCube(floorObject.transform.position, floorObject.transform.localScale);
            }
        }
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