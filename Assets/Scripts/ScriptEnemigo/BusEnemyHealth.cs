using UnityEngine;

public class BusEnemyHealth : MonoBehaviour
{
    public int maxHealth = 10;
    public int currentHealth;

    [Header("Dropeo de Objeto")]
    [SerializeField] private GameObject objetoMuerte;
    [SerializeField] private Transform spawnObjeto;

    [Header("Animaciones de Daño")]
    [SerializeField] private float primerDañoThreshold = 0.7f; //70% de vida
    [SerializeField] private float segundoDañoThreshold = 0.3f; //30% de vida
    [SerializeField] private string primerDañoTrigger = "PrimerDaño";
    [SerializeField] private string segundoDañoTrigger = "SegundoDaño";
    [SerializeField] private string muerteTrigger = "Muere";
    [SerializeField] private float muerteAnimationDelay = 1f;

    //Efecto visual de daño
    [SerializeField] private EfectoDano efectoDano;
    [SerializeField] private BarraVida barraVida;

    private Animator animator;
    private BusEnemy busEnemy;
    private bool primerDañoActivado = false;
    private bool segundoDañoActivado = false;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        busEnemy = GetComponent<BusEnemy>();

        barraVida?.IniciarBarraVida(maxHealth);
    }

    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;

        Debug.Log($"BusEnemy recibe {dmg} de daño. Vida: {currentHealth}/{maxHealth}");

        efectoDano?.ActivarEfecto();
        barraVida?.CambiarVidaActual(currentHealth);

        AnimacionesDano();

        if (currentHealth <= 0)
            Die();
    }

    private void AnimacionesDano()
    {
        float pct = GetHealthPercentage();

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

    void Die()
    {
        GetComponent<Collider2D>().enabled = false;
        animator.SetTrigger(muerteTrigger);

        FindFirstObjectByType<EnemySceneController>()?.RegisterEnemyKill();

        //Destruirlo despues de la animacion
        Invoke(nameof(CompleteDeath), muerteAnimationDelay);
    }

    private void CompleteDeath()
    {
        if (objetoMuerte != null)
            Instantiate(objetoMuerte, spawnObjeto.position, Quaternion.identity);

        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Ataque atk = col.GetComponent<Ataque>();
        if (atk != null)
            TakeDamage(atk.daño);
    }

    public float GetHealthPercentage() => (float)currentHealth / maxHealth;
}