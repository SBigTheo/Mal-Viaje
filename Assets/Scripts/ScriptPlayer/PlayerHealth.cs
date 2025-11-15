using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth;

    [SerializeField] private int currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TomarDaño(int daño)
    {
        int temporaryHealth = currentHealth - daño;

        temporaryHealth = Mathf.Clamp(temporaryHealth, 0, maxHealth);

        currentHealth = temporaryHealth;

        if (currentHealth <= 0)
        {
            Morir();
        }
    }

    private void Morir ()
    {
        Destroy(gameObject);
    }
}