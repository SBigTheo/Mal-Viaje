using UnityEngine;

public class BusEnemyHealth : MonoBehaviour
{
    public int maxHealth = 10;
    public int currentHealth;

    [Header("Dropeo de Objeto")]
    [SerializeField] private GameObject objetoMuerte;
    [SerializeField] private Transform spawnObjeto;

    private BusEnemy busEnemy;
    [SerializeField] private BarraVida barraVida;

    private void Start()
    {
        currentHealth = maxHealth;
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

    void Die()
    {
        Debug.Log("BusEnemy ha sido destruido.");
        
        FindFirstObjectByType<EnemySceneController>()?.RegisterEnemyKill();

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