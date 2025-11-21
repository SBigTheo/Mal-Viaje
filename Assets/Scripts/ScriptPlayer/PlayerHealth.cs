using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] public int maxHealth;

    [SerializeField] public int currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TomarDano(int dano)
    {
        int temporaryHealth = currentHealth - dano;

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