using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public float attackRange = 1.2f;
    public int attackDamage = 1;
    public float attackCooldown = 1.2f;
    public KeyCode attackKey = KeyCode.Space;
    public LayerMask enemyLayer;
    
    private float lastAttackTime = 0f;
    private bool canAttack = true;
    private PlayerController playerController;
    
    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }
    
    void Update()
    {
        if (Input.GetKeyDown(attackKey) && canAttack)
        {
            BasicAttack();
        }
        
        UpdateCooldown();
    }
    
    void BasicAttack()
    {
        if (playerController == null) 
        {
            Debug.LogError("PlayerController no encontrado!");
            return;
        }
        
        Vector2 attackDirection = playerController.GetLastMovementDirection();
        Vector2 attackPosition = (Vector2)transform.position + attackDirection * attackRange;
        
        Debug.Log($"Atacando en direcci�n: {attackDirection}, posici�n: {attackPosition}");
        
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPosition, attackRange * 0.5f, enemyLayer);
        
        Debug.Log($"Enemigos detectados: {hitEnemies.Length}");
        
        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log($"Golpeando: {enemy.gameObject.name}");
            
            BusEnemyHealth busEnemyHealth = enemy.GetComponent<BusEnemyHealth>();
            if (busEnemyHealth != null)
            {
                Debug.Log("BusEnemyHealth encontrado, aplicando da�o");
                busEnemyHealth.TakeDamage(attackDamage);
                continue;
            }
            
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(attackDamage);
            }
        }
        
        lastAttackTime = Time.time;
        canAttack = false;
    }
    
    void UpdateCooldown()
    {
        if (!canAttack && Time.time - lastAttackTime >= attackCooldown)
        {
            canAttack = true;
        }
    }
    
    void OnDrawGizmosSelected()
    {
        if (playerController == null) return;
        
        Vector2 attackDirection = playerController.GetLastMovementDirection();
        Vector2 attackPosition = (Vector2)transform.position + attackDirection * attackRange;
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPosition, attackRange * 0.5f);
        Gizmos.DrawLine(transform.position, attackPosition);
    }
}