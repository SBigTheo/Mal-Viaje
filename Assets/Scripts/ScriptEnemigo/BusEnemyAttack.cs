using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("Configuraci�n de Da�o")]
    [SerializeField] private int normalDamage = 1;
    [SerializeField] private int chargeDamage = 2;
    [SerializeField] private bool damageOnContact = true;

    [Header("Referencias")]
    [SerializeField] private BusEnemy busEnemy;

    private bool playerInContact = false;
    private PlayerHealth currentPlayerHealth;
    private float damageCooldown = 0.5f;
    private float lastDamageTime;

    private void Awake()
    {
        if (busEnemy == null)
            busEnemy = GetComponentInParent<BusEnemy>();
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
        if (playerInContact && damageOnContact && currentPlayerHealth != null)
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

            switch (busEnemy.GetCurrentState())
            {
                case BusEnemy.EnemyState.Attacking:
                    damageToApply = normalDamage;
                    break;

                case BusEnemy.EnemyState.Charging:
                    damageToApply = chargeDamage;
                    break;

                case BusEnemy.EnemyState.Appearing:
                    return;
            }

            if (damageToApply > 0)
            {
                playerHealth.TomarDano(damageToApply);
                lastDamageTime = Time.time;
                Debug.Log($"Da�o aplicado: {damageToApply} - Estado: {busEnemy.GetCurrentState()}");
            }
        }
    }
}