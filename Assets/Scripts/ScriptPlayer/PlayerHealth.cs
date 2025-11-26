using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] public int maxHealth;

    [SerializeField] public int currentHealth;
    [SerializeField] private BarraVida barraVida;
    private Animator animator;

    //Efecto visual del daño
    [SerializeField] private EfectoDano efectoDano;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;

        if (barraVida != null)
        {
            barraVida.IniciarBarraVida(maxHealth);
        }
    }

    public void TomarDano(int dano)
    {
        int temporaryHealth = currentHealth - dano;

        temporaryHealth = Mathf.Clamp(temporaryHealth, 0, maxHealth);

        currentHealth = temporaryHealth;

        if (efectoDano != null)
        {
            efectoDano.ActivarEfecto();
        }

        if (barraVida != null)
        {
            barraVida.CambiarVidaActual(currentHealth);
        }

        if (currentHealth <= 0)
        {
            Morir();
        }
    }

    private void Morir ()
    {
        animator.SetTrigger("Muerta");
        // Destroy(gameObject);
    }
}