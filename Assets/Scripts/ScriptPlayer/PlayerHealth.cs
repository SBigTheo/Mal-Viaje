using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth;

    [SerializeField] private int currentHealth;

    [SerializeField] private BarraDeVidaUI barraDeVidaUI;

    private void Awake()
    {
        currentHealth = maxHealth;
        barraDeVidaUI.IniciarBarraDeVidaPlayer(maxHealth, currentHealth);
    }

    public void TomarDano(int dano)
    {
        int temporaryHealth = currentHealth - dano;

        temporaryHealth = Mathf.Clamp(temporaryHealth, 0, maxHealth);

        currentHealth = temporaryHealth;

        barraDeVidaUI.CambiarBarraDeVida(currentHealth);

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