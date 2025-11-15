using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("Configuración de Daño")]
    [SerializeField] private int normalDamage = 1;
    [SerializeField] private int chargeDamage = 2;

    [Header("Referencias")]
    [SerializeField] private BusEnemy busEnemy;

    private void Awake()
    {
        if (busEnemy == null)
            busEnemy = GetComponentInParent<BusEnemy>();

        if (busEnemy == null)
            Debug.LogError("BusEnemy no encontrado en el padre!", this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ApplyDamage(collision.GetComponent<PlayerHealth>());
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
                    Debug.Log($"Ataque normal - Daño: {normalDamage}");
                    break;

                case BusEnemy.EnemyState.Charging:
                    damageToApply = chargeDamage;
                    Debug.Log($"Ataque de carga - Daño: {chargeDamage}");
                    break;

                case BusEnemy.EnemyState.Appearing:
                    return;
            }

            if (damageToApply > 0)
            {
                playerHealth.TomarDaño(damageToApply);
            }
        }
    }
}