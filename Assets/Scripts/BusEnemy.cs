using UnityEngine;

public class BusEnemy : MonoBehaviour
{
    public enum EnemyState { Appearing, Attacking, Charging } //El colectivo Aparece, ataca como golpe normal, y embiste/impacta con el jugador como ataque especial

    public float moveSpeed = 3f; 
    public float chargeSpeed = 8f;
    public float attackDuration = 1.5f;
    public int maxAttacksBeforeCharge = 3;

    public EnemyState currentState = EnemyState.Appearing;

    private Transform player;
    private Vector3 targetPosition;
    private float attackTimer;
    private bool isFacingRight;
    private int attackCount = 0;
    private SpriteRenderer spriteRenderer;
    private bool appearedFromRight = false;

    private void Start()
    {
       player = GameObject.FindGameObjectWithTag("Player").transform;
       spriteRenderer = GetComponent<SpriteRenderer>();
        SetInitialAppearanceSide();
    }

    void Update() 
    {
        switch (currentState)
        {
            case EnemyState.Appearing:
                MoveToInitialPosition();
                break;

                case EnemyState.Attacking:
                AttackInPlace();
                break;

                case EnemyState.Charging:
                ChargePlayer();
                break;
        }

        UpdateFacingDirection();
    }

    void SetInitialAppearanceSide()
    {
        appearedFromRight = Random.Range(0, 2) == 0;
        SetupAppearancePosition();
    }

    void SetupAppearancePosition()
    {
        float spawnX, targetX;

        if (appearedFromRight)
        {
            spawnX = Camera.main.ViewportToWorldPoint(new Vector3(1.1f, 0, 0)).x;
            targetX = Camera.main.ViewportToWorldPoint(new Vector3(0.7f, 0, 0)).x;
            isFacingRight = false;
        }
        else 
        { 
            spawnX = Camera.main.ViewportToWorldPoint(new Vector3(-0.1f, 0, 0)).x;
            targetX = Camera.main.ViewportToWorldPoint(new Vector3(0.3f, 0, 0)).x;
            isFacingRight = true;
        }

        float spawnY = Random.Range(0.3f, 0.7f);
        Vector3 spawnPos = new Vector3(spawnX, Camera.main.ViewportToWorldPoint(new Vector3(0, spawnY, 0)).y, 0);

        transform.position = spawnPos;
        targetPosition = new Vector3(targetX, transform.position.y, 0);
    }

    void MoveToInitialPosition()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            currentState = EnemyState.Attacking;
            attackTimer = attackDuration;
            attackCount++;
        }
    }

    void AttackInPlace()
    {
        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0)
        {
            if (attackCount >= maxAttacksBeforeCharge)
            {
                currentState = EnemyState.Charging;
                attackCount = 0;
            }
            else
            {
                attackTimer = attackDuration;
                attackCount++;
            }
        }
    }

    void ChargePlayer()
    {
        Vector3 chargeDirection = appearedFromRight ? Vector3.left : Vector3.right;
        transform.position += chargeDirection * chargeSpeed * Time.deltaTime;

        if (IsOutOfScreen())
        {
            appearedFromRight = !appearedFromRight;
            SetupAppearancePosition();
            currentState = EnemyState.Appearing;
        }
    }

    bool IsOutOfScreen()
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        return screenPoint.x < -0.2f || screenPoint.x > 1.2f || screenPoint.y < -0.2f || screenPoint.y > 1.2f;
    }

    void UpdateFacingDirection()
    {
        if (spriteRenderer != null && player != null)
        {
            isFacingRight = player.position.x > transform.position.x;
            spriteRenderer.flipX = !isFacingRight;
        }
    }
}
