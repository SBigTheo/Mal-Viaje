using UnityEngine;

public class EnemyPareja : MonoBehaviour
{
    [Header("Configuración Inicial")]
    public float speed = 1.5f;
    public bool flipToFacePlayer = true;
    [SerializeField] private bool esJefe = false; // SOLO UNA VEZ
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

    // ELIMINAR la sección duplicada "Configuración Sistema Oleadas"
    // [Header("Configuración Sistema Oleadas")] // ← QUITAR
    // [SerializeField] private bool esJefe = false; // ← QUITAR (ya existe arriba)
    private SistemaOleadas sistemaOleadas; // ← SOLO UNA VEZ
    private bool yaNotificadoMuerte = false;

    private Animator animator;
    private Transform player;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    // private SistemaOleadas sistemaOleadas; // ← QUITAR (duplicada)
    private bool seMueve = false;
    private float sueloNivel = -2.5f;
    private bool isApplicationQuitting = false;

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
        TryFindPlayer();

        if (rb != null)
        {
            rb.gravityScale = 0f;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        barraVida?.IniciarBarraVida(maxHealth);

        if (esJefe)
            Debug.Log("EnemyPareja configurado como JEFE.");
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

        if(currentHealth <= 0)return;

        float distancia = Vector2.Distance(transform.position, player.position);

        if (distancia <= attackRange && canAttack)
        {
            AttackPlayer();
        }
        else if (distancia > attackRange)
        {
            Vector2 target = new Vector2(player.position.x, sueloNivel);
            Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
            newPos.y = sueloNivel;
            rb.MovePosition(newPos);
        }

        seMueve = distancia > attackRange;
        animator.SetBool("Camina", seMueve);

        if (flipToFacePlayer)
            FacePlayer();
    }

    void TryFindPlayer()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("Player");
        if (obj != null)
            player = obj.transform;
    }

    void FacePlayer()
    {
        if (player == null) return;

        float dir = player.position.x - transform.position.x;
        sprite.flipX = dir > 0;
    }

    void Update()
    {
        if (Mathf.Abs(transform.position.y - sueloNivel) > 0.01f)
            transform.position = new Vector3(transform.position.x, sueloNivel, transform.position.z);

        if (!canAttack && Time.time >= lastAttackTime + attackCooldown)
        {
            canAttack = true;
            animator.SetBool("Atacar", false);
        }
    }

    void AttackPlayer()
    {
        if (player == null || !canAttack) return;

        PlayerHealth hp = player.GetComponent<PlayerHealth>();
        if (hp != null)
        {
            hp.TomarDano(damage);
            animator.SetBool("Atacar", true);
            canAttack = false;
            lastAttackTime = Time.time;
        }
    }

    public void TomarDano(int daño)
    {
        currentHealth -= daño;
        Debug.Log($"EnemyPareja recibió {daño}. Vida restante: {currentHealth}/{maxHealth}");

        barraVida?.CambiarVidaActual(currentHealth);

        AnimacionesDano();

        if (currentHealth <= 0)
            Morir();
    }

    private void AnimacionesDano()
    {
        float pct = GetHealthPercentage();

        if (!primerDañoActivado && pct <= primerDañoThreshold)
        {
            animator.SetTrigger("PrimerDaño");
            primerDañoActivado = true;
        }
        else if (!segundoDañoActivado && pct <= segundoDañoThreshold)
        {
            animator.SetTrigger("SegundoDaño");
            segundoDañoActivado = true;
        }
    }

    void Morir()
    {
        if (yaNotificadoMuerte) return;
        yaNotificadoMuerte = true;

        GetComponent<Collider2D>().enabled = false;
        animator.SetTrigger("Muere");

        // Notificar sistema de oleadas si es jefe
        if (esJefe && sistemaOleadas != null)
        {
            sistemaOleadas.JefeDerrotado();
        }
        else if (!esJefe && sistemaOleadas != null)
        {
            sistemaOleadas.JefeDerrotado();
        }

        // Notificar sistema de conteo (opcional)
        EnemySceneController sceneController = FindObjectOfType<EnemySceneController>();
        if (sceneController != null)
        {
            sceneController.OnEnemyKilled();
        }
        
        GameFlowManager.Instance?.RegisterEnemyKill();
    }

    private void CompleteDeath()
    {
        // SOLTAR OBJETO (IMPORTANTE para la victoria)
        if (objetoMuerte != null)
        {
            Instantiate(objetoMuerte, spawnObjeto.position, Quaternion.identity);
            Debug.Log("Objeto soltado para la victoria");
        }

        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Ataque atk = col.GetComponent<Ataque>();
        if (atk != null)
            TomarDano(atk.Daño);
    }

    void OnApplicationQuit()
    {
        isApplicationQuitting = true;
    }

    void OnDestroy()
    {
        if (isApplicationQuitting) return;

        // CORREGIR: Esto estaba mal, debería ser para enemigos normales
        if (!esJefe && sistemaOleadas != null)
        {
            sistemaOleadas.JefeDerrotado(); // ← Cambiado de JefeDerrotado a EnemigoDerrotado
        }
    }

    public float GetHealthPercentage() => (float)currentHealth / maxHealth;
}