using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    [Header("Referencias")]
    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;

    [SerializeField] private Transform player;

    [Header("Movimiento")]
    [SerializeField] private float speed = 1.5f;
    [SerializeField] private bool flipToFacePlayer = true;
    [SerializeField] private float sueloNivel = -2.5f;

    [Header("Ataque")]
    [SerializeField] private int damage = 1;
    [SerializeField] private float attackCooldown = 0.5f;
    [SerializeField] private float attackRange = 1.5f;

    private float lastAttackTime = 0f;
    private bool canAttack = true;
    private bool seMueve = false;

    // Propiedades POO
    public float Speed => speed;
    public bool CanAttack => canAttack;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        if (rb != null)
        {
            rb.gravityScale = 0f;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    private void Start()
    {
        if (player == null)
            TryFindPlayer();
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

        float distancia = Vector2.Distance(transform.position, player.position);

        if (distancia <= attackRange && canAttack)
        {
            AttackPlayer();
        }
        else if (distancia > attackRange)
        {
            MoverHaciaJugador();
        }

        seMueve = distancia > 0.1f;
        animator.SetBool("Camina", seMueve);

        if (flipToFacePlayer)
            FacePlayer();
    }

    private void Update()
    {
        MantenerAlturaFija();

        if (!canAttack && Time.time >= lastAttackTime + attackCooldown)
        {
            canAttack = true;
            animator.SetBool("Atacar", false);
        }
    }
    private void TryFindPlayer()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("Player");
        if (obj != null)
            player = obj.transform;
    }

    private void MoverHaciaJugador()
    {
        Vector2 target = new Vector2(player.position.x, sueloNivel);
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);

        newPos.y = sueloNivel;
        rb.MovePosition(newPos);
    }

    private void MantenerAlturaFija()
    {
        if (Mathf.Abs(transform.position.y - sueloNivel) > 0.01f)
        {
            Vector3 fixedPos = new Vector3(transform.position.x, sueloNivel, transform.position.z);
            transform.position = fixedPos;
        }
    }

    private void FacePlayer()
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

    private void AttackPlayer()
    {
        if (!canAttack) return;

        PlayerHealth hp = player.GetComponent<PlayerHealth>();

        if (hp != null)
        {
            hp.TomarDano(damage);
            animator.SetBool("Atacar", true);

            canAttack = false;
            lastAttackTime = Time.time;
        }
    }

    private void OnDestroy()
    {
        player = null;
    }
}
