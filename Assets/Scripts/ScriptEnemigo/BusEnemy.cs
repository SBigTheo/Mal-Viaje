using UnityEngine;
using System.Collections;

public class BusEnemy : MonoBehaviour
{
    public enum EnemyState { Appearing, Attacking, Charging, Cooldown, Dead }

    [Header("Configuración Básica")]
    [SerializeField] private bool esJefe = false;
    public int puntos = 10;

    [Header("Velocidades")]
    [SerializeField] private float normalAttackSpeed = 2f;
    [SerializeField] private float chargeSpeed = 10f;
    [SerializeField] private float chargeDistance = 5f;

    [Header("Vida")]
    public int maxHealth = 10;
    public int currentHealth;
    [SerializeField] private BarraVida barraVida;
    [SerializeField] private EfectoDano efectoDano;

    [Header("Detección de suelo")]
    [SerializeField] private float groundCheckDistance = 0.5f;
    [SerializeField] private LayerMask capaSuelo;

    [Header("Dirección")]
    [SerializeField] private int facingDirection = -1;

    [Header("Dropeo de Objeto")]
    [SerializeField] private GameObject objetoMuerte;
    [SerializeField] private Transform spawnObjeto;

    [Header("Animaciones de Daño")]
    [SerializeField] private float primerDañoThreshold = 0.7f;
    [SerializeField] private float segundoDañoThreshold = 0.3f;
    [SerializeField] private float muerteAnimationDelay = 1.0f;
    [SerializeField] private string primerDañoTrigger = "PrimerDaño";
    [SerializeField] private string segundoDañoTrigger = "SegundoDaño";
    [SerializeField] private string muerteTrigger = "Muere";

    [Header("Ataque")]
    [SerializeField] private int normalDamage = 1;
    [SerializeField] private int chargeDamage = 2;
    [SerializeField] private float damageCooldown = 1.7f;
    [SerializeField] private string attackTrigger = "Attacking";
    [SerializeField] private float attackAnimationDuration = 0.1f;

    private EnemyState currentState = EnemyState.Appearing;
    private Transform player;
    private bool hasCharged = false;
    private Vector2 currentDirection;
    private Vector2 chargeDirection;
    private float chargeTimer = 0f;
    private bool enSuelo;
    private float lastDamageTime = 0f;
    private bool playerInContact = false;
    private PlayerHealth currentPlayerHealth;

    private Animator animator;
    private SistemaOleadas sistemaOleadas;
    private AudioManager audioManager;
    private bool isApplicationQuitting = false;
    private bool primerDañoActivado = false;
    private bool segundoDañoActivado = false;

    public EnemyState CurrentState => currentState;

    public void ConfigurarSistemaOleadas(SistemaOleadas sistema)
    {
        sistemaOleadas = sistema;
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioManager = GameObject.FindGameObjectWithTag("Audio")?.GetComponent<AudioManager>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
        GameObject pl = GameObject.FindGameObjectWithTag("Player");
        if (pl != null) player = pl.transform;

        currentDirection = Vector2.right;
        barraVida?.IniciarBarraVida(maxHealth);

        if (esJefe)
            Debug.Log("BusEnemy configurado como JEFE.");

        Invoke(nameof(StartAttacking), 1f);
    }

    private void Update()
    {
        if (currentState == EnemyState.Dead) return;
        if (player == null) return;

        CheckGrounded();

        switch (currentState)
        {
            case EnemyState.Attacking:
                if (enSuelo) HandleNormalAttack();
                CheckForChargeCondition();
                HandlePlayerDamage();
                break;

            case EnemyState.Charging:
                HandleChargeAttack();
                HandlePlayerDamage();
                break;
        }
    }

    private void CheckGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, capaSuelo);
        enSuelo = hit.collider != null;

        if (!enSuelo && currentState != EnemyState.Charging)
        {
            AdjustToGround();
        }
    }

    private void AdjustToGround()
    {
        RaycastHit2D groundSearch = Physics2D.Raycast(transform.position, Vector2.down, 5f, capaSuelo);
        if (groundSearch.collider != null)
        {
            transform.position = new Vector2(transform.position.x, groundSearch.point.y + 0.5f);
        }
    }

    private void HandleNormalAttack()
    {
        Vector2 targetDirection = (player.position - transform.position);
        targetDirection.y = 0;
        currentDirection = targetDirection.normalized;

        Vector3 newPosition = transform.position + (Vector3)(currentDirection * normalAttackSpeed * Time.deltaTime);

        RaycastHit2D groundCheck = Physics2D.Raycast(newPosition, Vector2.down, groundCheckDistance, capaSuelo);
        if (groundCheck.collider != null)
        {
            transform.position = new Vector3(newPosition.x, groundCheck.point.y + 0.5f, newPosition.z);
        }

        if (currentDirection.x != 0)
        {
            transform.localScale = new Vector3(facingDirection * Mathf.Sign(currentDirection.x), 1f, 1f);
        }
    }

    private void HandleChargeAttack()
    {
        transform.position += (Vector3)(chargeDirection * chargeSpeed * Time.deltaTime);
        chargeTimer += Time.deltaTime;

        if (chargeTimer >= 1.5f)
            EndCharge();
    }

    private void CheckForChargeCondition()
    {
        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= chargeDistance && !hasCharged && enSuelo)
            StartCharge();
    }

    private void StartCharge()
    {
        currentState = EnemyState.Charging;
        hasCharged = true;
        chargeTimer = 0f;
        chargeDirection = currentDirection;
    }

    private void EndCharge()
    {
        currentState = EnemyState.Attacking;
        Invoke(nameof(ResetCharge), 2f);
    }

    private void ResetCharge() => hasCharged = false;
    private void StartAttacking() => currentState = EnemyState.Attacking;

    private void HandlePlayerDamage()
    {
        if (!playerInContact || currentPlayerHealth == null) return;

        if (Time.time >= lastDamageTime + damageCooldown)
        {
            ApplyDamage(currentPlayerHealth);
        }
    }

    private void ApplyDamage(PlayerHealth playerHealth)
    {
        if (playerHealth == null) return;

        int damage = currentState switch
        {
            EnemyState.Attacking => normalDamage,
            EnemyState.Charging => chargeDamage,
            _ => 0
        };

        if (damage <= 0) return;

        PlayAttackAnimation();
        playerHealth.TomarDano(damage);
        lastDamageTime = Time.time;
    }

    private void PlayAttackAnimation()
    {
        if (animator == null || string.IsNullOrEmpty(attackTrigger)) return;
        animator.SetTrigger(attackTrigger);
        StartCoroutine(ResetAttackTriggerAfterDelay(attackAnimationDuration));
    }

    private IEnumerator ResetAttackTriggerAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        animator.ResetTrigger(attackTrigger);
    }

    public void TomarDano(int daño)
    {
        if (currentState == EnemyState.Dead) return;

        currentHealth -= daño;
        Debug.Log($"BusEnemy recibió {daño}. Vida restante: {currentHealth}/{maxHealth}");

        efectoDano?.ActivarEfecto();
        barraVida?.CambiarVidaActual(currentHealth);

        AnimacionesDano();

        if (currentHealth <= 0)
            Morir();
    }

    private void AnimacionesDano()
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

    private void Morir()
    {
        currentState = EnemyState.Dead;
        GetComponent<Collider2D>().enabled = false;
        animator.SetTrigger(muerteTrigger);

        audioManager?.PlaySFX(audioManager.muerteEnemigo);
        EnemySceneController sceneController = FindObjectOfType<EnemySceneController>();
        
        if (sceneController != null)
        {
            sceneController.OnEnemyKilled();
        }
        
        // O directamente al GameFlowManager
        GameFlowManager.Instance?.RegisterEnemyKill();
        Invoke(nameof(CompleteDeath), muerteAnimationDelay);
    }

    private void CompleteDeath()
    {
        if (objetoMuerte != null && spawnObjeto != null)
            Instantiate(objetoMuerte, spawnObjeto.position, Quaternion.identity);

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            playerInContact = true;
            currentPlayerHealth = col.GetComponent<PlayerHealth>();
            ApplyDamage(currentPlayerHealth);
        }
        else
        {
            Ataque atk = col.GetComponent<Ataque>();
            if (atk != null)
                TomarDano(atk.Daño);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            playerInContact = false;
            currentPlayerHealth = null;
        }
    }

    void OnApplicationQuit()
    {
        isApplicationQuitting = true;
    }

    void OnDestroy()
    {
        if (isApplicationQuitting) return;

        if (sistemaOleadas != null)
        {
            if (esJefe)
                sistemaOleadas.JefeDerrotado();
            else
                sistemaOleadas.JefeDerrotado();
        }
    }

    public float GetHealthPercentage() => (float)currentHealth / maxHealth;
}