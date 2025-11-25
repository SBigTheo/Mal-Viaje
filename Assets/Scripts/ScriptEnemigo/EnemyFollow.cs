using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    private Animator animator;
    public Transform player;
    
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    public float speed = 1.5f;
    public bool flipToFacePlayer = true;

    [Header("Ataque")]
    private int damage = 1;
    private float attackCooldown = 0.5f;
    private float attackRange = 1.5f;
    private float lasAttackTime = 0f;
    private bool canAtack = true;

    private bool seMueve = false;
    private float sueloNivel = -2.5f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        if (player == null)
            TryFindPlayer();

        if (rb != null)
        {
            rb.gravityScale =0f;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    void FixedUpdate()
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

        float disntaciaDelPlayer = Vector2.Distance(transform.position, player.position);

        if (disntaciaDelPlayer <= attackRange && canAtack)
        {
            AttackPlayer();
        }else if(disntaciaDelPlayer > attackRange)
        {
            Vector2 target = new Vector2(player.position.x, sueloNivel);
            Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
            newPos.y = sueloNivel;
        rb.MovePosition(newPos);
        }
        seMueve = disntaciaDelPlayer > 0.1f;
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

    private void Update() {

        if(Mathf.Abs(transform.position.y - sueloNivel) > 0.01f)
        {
            Vector3 fixedPos = new Vector3(transform.position.x, sueloNivel, transform.position.z);
            transform.position = fixedPos;
        }

        if ( !canAtack && Time.time >= lasAttackTime + attackCooldown)
        {
            canAtack = true;
        }
    }

    void AttackPlayer()
    {
        if (player == null || !canAtack) return;
        
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            playerHealth.TomarDano(damage);
            animator.SetTrigger("Atacar");
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.CompareTag("Player") && canAtack)
            {
                AttackPlayer();
            }
        }
    }

    void OnDestroy()
    {
        player = null;
    }
}