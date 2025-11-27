using UnityEngine;

public class BusEnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 10;

    [Header("Dropeo de Objeto")]
    [SerializeField] private GameObject objetoMuerte;
    [SerializeField] private Transform spawnObjeto;

    [Header("Daño Visual")]
    [SerializeField] private float primerDañoThreshold = 0.7f;
    [SerializeField] private float segundoDañoThreshold = 0.3f;

    [SerializeField] private string primerDañoTrigger = "PrimerDaño";
    [SerializeField] private string segundoDañoTrigger = "SegundoDaño";
    [SerializeField] private string muerteTrigger = "Muere";

    [SerializeField] private float muerteAnimationDelay = 1f;

    [SerializeField] private EfectoDano efectoDano;
    [SerializeField] private BarraVida barraVida;

    private int currentHealth;
    private Animator animator;
    private BusEnemy busEnemy;

    private bool primerDañoActivado = false;
    private bool segundoDañoActivado = false;

    private AudioManager audioManager;

    private void Awake()
    {
        audioManager =
            GameObject.FindGameObjectWithTag("Audio")
            ?.GetComponent<AudioManager>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        busEnemy = GetComponent<BusEnemy>();
        barraVida?.IniciarBarraVida(maxHealth);
    }

    public void TakeDamage(int dmg)
    {
        currentHealth = Mathf.Max(0, currentHealth - dmg);

        efectoDano?.ActivarEfecto();
        barraVida?.CambiarVidaActual(currentHealth);

        HandleDamageAnimations();

        if (currentHealth <= 0)
            Die();
    }

    private void HandleDamageAnimations()
    {
        float pct = (float)currentHealth / maxHealth;

        if (!primerDañoActivado && pct <= primerDañoThreshold)
        {
            animator.SetTrigger(primerDañoTrigger);
            primerDañoActivado = true;
        }
        else if (!segundoDañoActivado && pct <= segundoDañoThreshold)
        {
            animator.SetTrigger(segundoDañoTrigger);
            segundoDañoActivado = true;
        }
    }

    private void Die()
    {
        audioManager?.PlaySFX(audioManager.muerteEnemigo);

        GetComponent<Collider2D>().enabled = false;
        animator.SetTrigger(muerteTrigger);

        GameFlowManager.Instance?.RegisterEnemyKill();

        Invoke(nameof(FinishDeath), muerteAnimationDelay);
    }

    private void FinishDeath()
    {
        if (objetoMuerte != null && spawnObjeto != null)
            Instantiate(objetoMuerte, spawnObjeto.position, Quaternion.identity);

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Ataque atk = col.GetComponent<Ataque>();
        if (atk != null)
            TakeDamage(atk.Daño);
    }
}
