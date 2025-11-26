using UnityEngine;

public class BusEnemy : MonoBehaviour
{
    public enum EnemyState { Appearing, Attacking, Charging, Cooldown } //El colectivo Aparece, ataca como golpe normal, y embiste/impacta con el jugador como ataque especial

    [SerializeField] private float normalAttackSpeed = 2f;
    [SerializeField] private float chargeSpeed = 8f;
    [SerializeField] private float chargeDistance = 5f;

    [SerializeField] private int facingDirection = -1; //1 para mirar a la derecha, -1 para la izquierda
    [SerializeField] private float groundCheckDistance = 0.5f;
    [SerializeField] private LayerMask capaSuelo;

    private EnemyState currentState;
    private Transform player;
    private bool hasCharged = false;
    private Vector2 currentDirection;
    private Vector2 chargeDirection;
    private float chargeTimer = 0f;
    private bool enSuelo;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentState = EnemyState.Appearing;
        currentDirection = Vector2.right;
        Invoke("StartAttacking", 1f);
    }

    private void Update()
    {
        if (player == null) return;

        CheckGrounded();

        switch (currentState)
        {
            case EnemyState.Attacking:
                if (enSuelo) HandleNormalAttack();
                CheckForChargeCondition();
                break;

            case EnemyState.Charging:
                HandleChargeAttack();
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
        Vector2 targetDirection = new Vector2(player.position.x - transform.position.x, 0f).normalized;
        currentDirection = targetDirection;

        Vector3 newPosition = transform.position + (Vector3)(currentDirection * normalAttackSpeed * Time.deltaTime);

        RaycastHit2D groundCheck = Physics2D.Raycast(newPosition, Vector2.down, groundCheckDistance, capaSuelo);
        if (groundCheck.collider != null)
        {
            transform.position = new Vector3(newPosition.x, groundCheck.point.y + 0.5f, newPosition.z);
        }

        if (currentDirection.x != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(currentDirection.x), 1f, 1f);
        }

        if (currentDirection.x != 0)
        {
            transform.localScale = new Vector3(
                facingDirection * Mathf.Sign(currentDirection.x),
                1f,
                1f
            );
        }

    }

    private void HandleChargeAttack()
    {
        transform.position += (Vector3)(chargeDirection * chargeSpeed * Time.deltaTime);

        chargeTimer += Time.deltaTime;
        if (chargeTimer >= 1.5f)
        {
            EndCharge();
        }
    }

    private void CheckForChargeCondition()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= chargeDistance && !hasCharged && enSuelo)
        {
            StartCharge();
        }
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
        Invoke("ResetCharge", 2f);
    }

    private void ResetCharge()
    {
        hasCharged = false;
    }

    private void StartAttacking()
    {
        currentState = EnemyState.Attacking;
    }

    public EnemyState GetCurrentState()
    {
        return currentState;
    }
}