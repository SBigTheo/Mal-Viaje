using UnityEngine;

public class Spiid : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.currentHealth = playerHealth.MaxHealth;
            }

            Destroy(gameObject);
        }
    }
}