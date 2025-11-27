using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private int normalDamage = 1;
    [SerializeField] private int chargeDamage = 2;

    [SerializeField] private bool damageOnContact = true;

    [SerializeField] private float damageCooldown = 1.7f;
    private float lastDamageTime = 0f;

    [Header("Animación")]
    [SerializeField] private string attackTrigger = "Attacking";
    [SerializeField] private float attackAnimationDuration = 0.1f;

    private Animator animator;
    private BusEnemy busEnemy;

    private PlayerHealth currentPlayerHealth;
    private bool playerInContact = false;

    private void Awake()
    {
        busEnemy = GetComponentInParent<BusEnemy>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!playerInContact || currentPlayerHealth == null)
            return;

        if (Time.time >= lastDamageTime + damageCooldown)
        {
            ApplyDamage(currentPlayerHealth);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        playerInContact = true;
        currentPlayerHealth = collision.GetComponent<PlayerHealth>();

        ApplyDamage(currentPlayerHealth);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        playerInContact = false;
        currentPlayerHealth = null;
    }

    private void ApplyDamage(PlayerHealth playerHealth)
    {
        if (playerHealth == null || busEnemy == null) return;

        int damage = busEnemy.CurrentState switch
        {
            BusEnemy.EnemyState.Attacking => normalDamage,
            BusEnemy.EnemyState.Charging => chargeDamage,
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

    private System.Collections.IEnumerator ResetAttackTriggerAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        animator.ResetTrigger(attackTrigger);
    }
}
