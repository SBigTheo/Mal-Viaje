using UnityEngine;

public class EnemyDrop : MonoBehaviour
{
    [Header("Configuración de Drop")]
    public GameObject dropPrefab;
    public float dropChance = 1f;
    public Vector2 dropOffset = Vector2.zero;

    private bool dropped = false;

    private void OnDestroy()
    {
        if (!Application.isPlaying) return;

        if (!dropped && dropPrefab != null)
        {
            dropped = true;

            if (Random.value <= dropChance)
            {
                Vector2 spawnPos = (Vector2)transform.position + dropOffset;
                Instantiate(dropPrefab, spawnPos, Quaternion.identity);
            }
        }
    }
}