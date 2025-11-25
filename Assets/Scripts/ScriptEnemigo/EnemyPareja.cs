using UnityEngine;

public class EnemyPareja : MonoBehaviour
{
    [Header("Configuración Inicial")]
    public float speed = 1.5f;
    public bool flipToFacePlayer = true;
    private bool esJefe = false;
    public int punto = 10;

    [Header("Vida")]
    public int maxHealth = 10;
    public int currentHealth;
    [SerializeField] private BarraVida barraVida;
    private bool primerDañoActivado = false;
    private bool segundoDañoActivado = false;

    [Header("Dropeo de Objeto")]
    [SerializeField] private GameObject objetoMuerte;
    [SerializeField] private Transform spawnObjeto;

    [Header("Animaciones de Daño")]
    [SerializeField] private float primerDañoThreshold = 0.7f;
    [SerializeField] private float segundoDañoThreshold = 0.3f;
    [SerializeField] private float muerteAnimationDelay = 1.0f;

    [Header("Ataque")]
    private int damage = 3;
    private float attackCooldown = 0.5f;
    private float attackRange = 1.5f;
    private float lastAttackTime = 0f;
    private bool canAttack = true;

    private Animator animator;
    public Transform player;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private SistemaOleadas sistemaOleadas;
    private bool seMueve = false;
    private float sueloNivel = -2.5f;

    public void ConfigurarSistemaOleadas(SistemaOleadas sistema)
    {
        sistemaOleadas = sistema;
    }

    private void Awake() 
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        currentHealth = maxHealth;
        
        if (player == null)
            TryFindPlayer();

        if (rb != null)
        {
            rb.gravityScale = 0f;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        
        if (barraVida != null)
        {
            barraVida.IniciarBarraVida(maxHealth);
        }
    }

    private void FixedUpdate() 
    {
        if (player == null)
        {
            TryFindPlayer();
            if (player == null)
            {
                animator.SetBool("Camina", false);
                return;
            }
        }

        float distanciaDelPlayer = Vector2.Distance(transform.position, player.position);

        if (distanciaDelPlayer <= attackRange && canAttack)
        {
            AttackPlayer();
        }
        else if (distanciaDelPlayer > attackRange)
        {
            Vector2 target = new Vector2(player.position.x, sueloNivel);
            Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
            newPos.y = sueloNivel;
            rb.MovePosition(newPos);
        }
        
        seMueve = distanciaDelPlayer > attackRange;
        animator.SetBool("Camina", seMueve);

        if (flipToFacePlayer)
            FacePlayer();
    }

    void TryFindPlayer()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    void FacePlayer()
    {
        if (player == null) return;
        float dir = player.position.x - transform.position.x;

        if (sprite != null)
        {
            sprite.flipX = dir > 0;
        }
        else
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * (dir > 0 ? -1 : 1);
            transform.localScale = scale;
        }
    }

    void Update()
    {
        if (Mathf.Abs(transform.position.y - sueloNivel) > 0.01f)
        {
            Vector3 fixedPos = new Vector3(transform.position.x, sueloNivel, transform.position.z);
            transform.position = fixedPos;
        }

        if (!canAttack && Time.time >= lastAttackTime + attackCooldown)
        {
            canAttack = true;
            animator.SetBool("Atacar", false);
        }
    }

    void AttackPlayer()
    {
        if (player == null || !canAttack) return;
        
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            playerHealth.TomarDano(damage);
            animator.SetBool("Atacar", true);

            canAttack = false;
            lastAttackTime = Time.time;
        }
    }

    void OnDestroy()
    {
        player = null;
    }

    public void TomarDano(int cantidadDaño)
    {
        currentHealth -= cantidadDaño;

        Debug.Log($"Jefe Pareja recibió {cantidadDaño} de daño. Vida restante: {currentHealth} / {maxHealth}");

        if (barraVida != null)
        {
            barraVida.CambiarVidaActual(currentHealth);
        }

        AnimacionesDano();

        if (currentHealth <= 0)
        {
            Morir();
        }
    }

    public void SoltarObjeto()
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
            Debug.Log("Animación de daño 1 activada");
        } 
        else if (!segundoDañoActivado && healthPercentage <= segundoDañoThreshold)
        {
            PlayAnimacionesDano("SegundoDaño");
            segundoDañoActivado = true;
            Debug.Log("Animación de daño 2 activada");
        }
    }

    private void PlayAnimacionesDano(string triggerName)
    {
        if (animator != null)
        {
            animator.SetTrigger(triggerName);
        }
    }

    void Morir()
    {
        GetComponent<Collider2D>().enabled = false;
        animator.SetTrigger("Muere");
        
        FindFirstObjectByType<EnemySceneController>()?.RegisterEnemyKill();

        // Destruirlo después de la animación
        Invoke("CompleteDeath", muerteAnimationDelay);
    }

    private void CompleteDeath()
    {
        SoltarObjeto();
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Ataque ataque = collision.GetComponent<Ataque>();
        if (ataque != null)
        {
            TomarDano(ataque.daño);
        }
    }

    public float GetHealthPercentage()
    {
        return (float)currentHealth / maxHealth;
    }
}