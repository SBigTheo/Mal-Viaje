using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Vida")]
    [SerializeField] private int maxHealth = 10;
    public int CurrentHealth { get; private set; }

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

        CurrentHealth = maxHealth;

        if (barraVida != null)
            barraVida.IniciarBarraVida(maxHealth);
    }

    public void TomarDano(int dano)
    {
        if (EstaMuerto) return;

        CurrentHealth = Mathf.Clamp(CurrentHealth - dano, 0, maxHealth);

        audioManager?.PlaySFX(audioManager.dañoRecibidoPorEnemigo);
        efectoDano?.ActivarEfecto();
        barraVida?.CambiarVidaActual(CurrentHealth);

        if (CurrentHealth <= 0)
            Morir();
    }

    private void Morir()
    {
        if (EstaMuerto) return;
        EstaMuerto = true;

        audioManager?.PlaySFX(audioManager.muertePlayer);
        animator.SetTrigger("Muerta");

        // desactivar controles
        if (TryGetComponent(out PlayerController controller))
            controller.enabled = false;

        Invoke(nameof(TriggerGameOver), muerteAnimationDelay);
    }

    private void TriggerGameOver()
    {
        GameFlowManager.Instance?.TriggerGameOver();
    }
}
