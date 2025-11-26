using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] public int maxHealth;

    [SerializeField] public int currentHealth;
    [SerializeField] private BarraVida barraVida;
    private Animator animator;

    [SerializeField] private EfectoDano efectoDano;

    private AudioManager audioManager;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;

        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

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
        
        if (audioManager != null)
        audioManager.PlaySFX(audioManager.dañoRecibidoPorEnemigo);
        
        if (efectoDano != null)
        efectoDano.ActivarEfecto();
        
        if (barraVida != null)
        barraVida.CambiarVidaActual(currentHealth);
        
        if (currentHealth <= 0)
        Morir();
        }

    private void Morir()
    {
        if (audioManager != null)
            audioManager.PlaySFX(audioManager.muertePlayer);

        animator.SetTrigger("Muerta");
    }
}