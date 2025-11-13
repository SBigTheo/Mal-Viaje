using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval = 4.5f;
    public int maxEnemies = 5;

    private bool isSpawning = true;
    private float minDistanceBetweenEnemies = 3f;
    private List<GameObject> activeEnemies = new List<GameObject>();

    private Vector2 leftSpawnMin = new Vector2(-15f, -2.5f);
    private Vector2 leftSpawnMax = new Vector2(-11f, -2.5f);
    private Vector2 rightSpawnMin = new Vector2(11f, -2.5f);
    private Vector2 rightSpawnMax = new Vector2(14.4f, -2.5f);

    void Start()
    {
        if (enemyPrefab == null) return;
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (isSpawning)
        {
            yield return new WaitForSeconds(spawnInterval);
            if (this == null || !isSpawning) yield break;

            CleanEnemyList();

            if (activeEnemies.Count < maxEnemies)
                TrySpawnEnemy();
        }
    }

    void CleanEnemyList()
    {
        for (int i = activeEnemies.Count - 1; i >= 0; i--)
            if (activeEnemies[i] == null)
                activeEnemies.RemoveAt(i);
    }

    void TrySpawnEnemy()
    {
        if (enemyPrefab == null) return;

        for (int i = 0; i < 10; i++)
        {
            Vector2 spawnPosition = GetRandomSpawnPosition();
            if (IsSpawnPositionValid(spawnPosition))
            {
                SpawnEnemyAtPosition(spawnPosition);
                return;
            }
        }
    }

    Vector2 GetRandomSpawnPosition()
    {
        bool spawnLeft = Random.value < 0.5f;
        float randomX = spawnLeft
            ? Random.Range(leftSpawnMin.x, leftSpawnMax.x)
            : Random.Range(rightSpawnMin.x, rightSpawnMax.x);

        float spawnY = spawnLeft ? leftSpawnMin.y : rightSpawnMin.y;
        return new Vector2(randomX, spawnY);
    }

    bool IsSpawnPositionValid(Vector2 spawnPosition)
    {
        foreach (GameObject enemy in activeEnemies)
        {
            if (enemy == null) continue;
            if (Vector2.Distance(spawnPosition, enemy.transform.position) < minDistanceBetweenEnemies)
                return false;
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && Vector2.Distance(spawnPosition, player.transform.position) < 4f)
            return false;

        return true;
    }

    void SpawnEnemyAtPosition(Vector2 position)
    {
        GameObject newEnemy = Instantiate(enemyPrefab, position, Quaternion.identity);
        activeEnemies.Add(newEnemy);
    }

    public void RemoveEnemy(GameObject enemy)
    {
        if (activeEnemies.Contains(enemy))
            activeEnemies.Remove(enemy);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(leftSpawnMin.x, leftSpawnMin.y, 0), new Vector3(leftSpawnMax.x, leftSpawnMax.y, 0));
        Gizmos.DrawLine(new Vector3(rightSpawnMin.x, rightSpawnMin.y, 0), new Vector3(rightSpawnMax.x, rightSpawnMax.y, 0));
    }

    public void StopSpawning()
    {
        isSpawning = false;
        StopAllCoroutines();
    }

    void OnDestroy() => StopSpawning();
    void OnDisable() => StopSpawning();
}