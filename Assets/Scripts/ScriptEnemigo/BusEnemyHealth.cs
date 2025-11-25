using UnityEngine;

public class BusEnemyHealth : MonoBehaviour
{
    public int maxHealth = 10;
    public int currentHealth;

    [Header("Dropeo de Objeto")]
    [SerializeField] private GameObject objetoMuerte;
    [SerializeField] private Transform spawnObjeto;

    [Header("Animaciones de Daño")]
    [SerializeField] private float primerDañoThreshold = 0.7f; // 70% de vida
    [SerializeField] private float segundoDañoThreshold = 0.3f; // 30% de vida
    [SerializeField] private string primerDañoTrigger = "PrimerDaño";
    [SerializeField] private string segundoDañoTrigger = "SegundoDaño";
    [SerializeField] private string muerteTrigger = "Muere";
    [SerializeField] private float muerteAnimationDelay = 1.0f;

    private BusEnemy busEnemy;
    private Animator animator;
    [SerializeField] private BarraVida barraVida;
    private bool primerDañoActivado = false;
    private bool segundoDañoActivado = false;

    private void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        busEnemy = GetComponent<BusEnemy>();

        if (barraVida != null)
        {
            barraVida.IniciarBarraVida(maxHealth);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        Debug.Log($"BusEnemy recibi� {damage} de da�o. Vida restante: {currentHealth} / {maxHealth}");

        if (barraVida != null)
        {
            barraVida.CambiarVidaActual(currentHealth);
        }

        AnimacionesDano();

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    public void Soltarobjeto()
    {
        if (objetoMuerte != null && spawnObjeto != null)
        {
            GameObject objeto = Instantiate(objetoMuerte, spawnObjeto.position, Quaternion.identity);
        }
    }

    private void AnimacionesDano()
    {
        float healthPercentage = GetHealthPercentage();
        if (!primerDañoActivado && healthPercentage <= primerDañoThreshold)
        {
            PlayAnimacionesDano("PrimerDaño");
            primerDañoActivado = true;
            Debug.Log("ANimacion de daño 1 ativada");

        } else if (!segundoDañoActivado && healthPercentage <= segundoDañoThreshold)
        {
            PlayAnimacionesDano("SegundoDaño");
            segundoDañoActivado = true;
            Debug.Log("ANimacion de daño 2 ativada");
        }
    }

    private void PlayAnimacionesDano(string triggerName)
    {
        if (animator != null)
        {
            animator.SetTrigger(triggerName);
        }
    }

    void Die()
    {
        GetComponent<Collider2D>().enabled = false;
        if (muerteTrigger != null)
        {
            animator.SetTrigger("Muere");
        }
        
        FindFirstObjectByType<EnemySceneController>()?.RegisterEnemyKill();

        // Destruirlo después de la animacion
        Invoke("CompleteDeath", muerteAnimationDelay);
    }

    private void CompleteDeath()
    {
    Soltarobjeto();
    Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Ataque ataque = collision.GetComponent<Ataque>();
        if (ataque != null)
        {
            TakeDamage(ataque.daño);
        }
    }

    public float GetHealthPercentage()
    {
        return (float)currentHealth / maxHealth;
    }
}