using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private int normalDamage = 1;
    [SerializeField] private int chargeDamage = 2;
    [SerializeField] private bool damageOnContact = true;

    [SerializeField] private BusEnemy busEnemy;
    private Animator animator;

    private bool playerInContact = false;
    private PlayerHealth currentPlayerHealth;
    [SerializeField] private float damageCooldown = 1.7f;
    private float lastDamageTime;

    [Header("Animación de Ataque")]
    [SerializeField] private string attackTrigger = "Attacking";
    [SerializeField] private float attackAnimationDuration = 0.1f;

    private void Awake()
    {
        if (busEnemy == null)
            busEnemy = GetComponentInParent<BusEnemy>();
        
        animator = GetComponent<Animator>();

        if (animator != null)
        {
            animator = GetComponent<Animator>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInContact = true;
            currentPlayerHealth = collision.GetComponent<PlayerHealth>();
            ApplyDamage(currentPlayerHealth);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInContact = false;
            currentPlayerHealth = null;
        }
    }

    private void Update()
    {
        if (!playerInContact || !damageOnContact || currentPlayerHealth != null)
        {
            if (Time.time - lastDamageTime >= damageCooldown)
            {
                ApplyDamage(currentPlayerHealth);
            }
        }
    }

    private void ApplyDamage(PlayerHealth playerHealth)
    {
        if (playerHealth != null && busEnemy != null)
        {
            int damageToApply = 0;
            bool shouldPlayAnimation = false;

            switch (busEnemy.GetCurrentState())
            {
                case BusEnemy.EnemyState.Attacking:
                    damageToApply = normalDamage;
                    shouldPlayAnimation = true;
                    break;

                case BusEnemy.EnemyState.Charging:
                    damageToApply = chargeDamage;
                    shouldPlayAnimation = true;
                    break;

                case BusEnemy.EnemyState.Appearing:
                case BusEnemy.EnemyState.Cooldown:
                shouldPlayAnimation = true;
                    return; 
            }

            if (damageToApply > 0)
            {

                if(shouldPlayAnimation && animator != null && !string.IsNullOrEmpty(attackTrigger))
                {
                    PlayAttackAnimation();
                }
                playerHealth.TomarDano(damageToApply);
                lastDamageTime = Time.time;
                Debug.Log($"Daño aplicado: {damageToApply} - Estado: {busEnemy.GetCurrentState()}");
            }
        }
    }

    private void PlayAttackAnimation()
    {
        // Activar el trigger de ataque
        animator.SetTrigger(attackTrigger);
        
        // Opcional: Programar reset del trigger después de un tiempo
        StartCoroutine(ResetAttackTriggerAfterDelay(attackAnimationDuration));
    }

    private System.Collections.IEnumerator ResetAttackTriggerAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        // Resetear el trigger para que pueda activarse nuevamente
        animator.ResetTrigger(attackTrigger);
    }

    // Método para activar la animación desde otros scripts si es necesario
    public void TriggerAttackAnimation()
    {
        if (animator != null && !string.IsNullOrEmpty(attackTrigger))
        {
            PlayAttackAnimation();
        }
    }

    // Método para saber si está en medio de una animación de ataque
    public bool IsAttacking()
    {
        if (animator == null) return false;
        
        return animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") || 
                animator.GetCurrentAnimatorStateInfo(0).IsName("Attacking");
    }
}