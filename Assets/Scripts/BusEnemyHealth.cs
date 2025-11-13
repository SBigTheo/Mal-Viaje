using UnityEngine;

public class BusEnemyHealth : MonoBehaviour
{
    public int maxHealth = 10;
    public int currentHealth;

    private BusEnemy busEnemy;

    private void Start()
    {
        currentHealth = maxHealth;
        busEnemy = GetComponent<BusEnemy>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        Debug.Log($"BusEnemy recibió {damage} de daño. Vida restante: {currentHealth} / {maxHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("BusEnemy ha sido destruido.");
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerBullet"))
        {
            TakeDamage(1);
            Destroy(collision.gameObject);
        }
    }

    public float GetHealthPercentage()
    {
        return (float)currentHealth / maxHealth;
    }
}