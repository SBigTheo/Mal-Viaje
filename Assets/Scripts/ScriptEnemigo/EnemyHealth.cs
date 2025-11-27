using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 1;
    private int currentHealth;

    private SpriteRenderer sprite;
    private Color originalColor;

    public int MaxHealth => maxHealth;
    public int CurrentHealth => currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;

        sprite = GetComponent<SpriteRenderer>();
        if (sprite != null)
            originalColor = sprite.color;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        StartCoroutine(DamageEffect());

        if (currentHealth <= 0)
            Die();
    }

    private IEnumerator DamageEffect()
    {
        if (sprite == null) yield break;

        sprite.color = Color.red;
        yield return new WaitForSeconds(0.15f);
        sprite.color = originalColor;
    }

    private void Die()
    {
        FindFirstObjectByType<EnemySceneController>()?.RegisterEnemyKill();
        Destroy(gameObject);
    }
}
