using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Vida")]
    [SerializeField] private int maxHealth = 10;
    public int currentHealth;

    [SerializeField] private BarraVida barraVida;
    [SerializeField] private float muerteAnimationDelay = 1f;
    public int MaxHealth => maxHealth;

    private Animator animator;
    private EfectoDano efectoDano;
    private AudioManager audioManager;

    public bool EstaMuerto { get; private set; } = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        efectoDano = GetComponent<EfectoDano>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        currentHealth = maxHealth;

        if (barraVida != null)
            barraVida.IniciarBarraVida(maxHealth);
    }

    public void TomarDano(int dano)
    {
        if (EstaMuerto) return;

        currentHealth = Mathf.Clamp(currentHealth - dano, 0, maxHealth);

        audioManager?.PlaySFX(audioManager.dañoRecibidoPorEnemigo);
        efectoDano?.ActivarEfecto();
        barraVida?.CambiarVidaActual(currentHealth);

        if (currentHealth <= 0)
            Morir();
    }

    private void Morir()
    {
        if (EstaMuerto) return;
        EstaMuerto = true;

        // CORREGIDO: Usar GameFlowManager en lugar de GameManager
        GameFlowManager.Instance.PlayerDied();
        audioManager?.PlaySFX(audioManager.muertePlayer);
        animator.SetTrigger("Muerta");

        // desactivar controles
        if (TryGetComponent(out PlayerController controller))
            controller.enabled = false;

        // ELIMINÉ el Invoke porque ya llamamos a PlayerDied() directamente
    }

    // CORREGIDO: Este método estaba mal
    public void WinGame()
    {
        if (!EstaMuerto) // CORREGIDO: usar !EstaMuerto en lugar de isAlive
        {
            GameFlowManager.Instance.PlayerWon();
        }
    }

    // ELIMINÉ TriggerGameOver porque ya no es necesario
}