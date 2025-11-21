using UnityEngine;
using System.Collections;
public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 1;
    private int currentHealth;
    private SpriteRenderer sprite;
    private Color originalColor;
    
    void Start()
    {
        currentHealth = maxHealth;

        sprite = GetComponent<SpriteRenderer>();

        if(sprite != null)
        {
            originalColor = sprite.color;
        }
    }
    
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        StartCoroutine(DamageEffect());
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator DamageEffect()
    {
        if (sprite == null) yield break;

        sprite.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        sprite.color = originalColor;
    }
    
    void Die()
    {
        FindFirstObjectByType<EnemySceneController>()?.RegisterEnemyKill();

        Destroy(gameObject);
    }
}