using UnityEngine;

public class JugadorVida : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;
    [SerializeField] private BarraVida barraDeVida; 

    private void Awake()
    {
        currentHealth = maxHealth;
        barraDeVida.IniciarBarraDeVida(maxHealth);
    }

    public void TomarDano(int dano)
    {
        currentHealth = Mathf.Clamp(currentHealth - dano, 0, maxHealth);
        barraDeVida.CambiarVidaActual(currentHealth);
    
        if (currentHealth <= 0) Morir();
    }

    private void Morir()
    {
        Destroy(gameObject);
    }
}