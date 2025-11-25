using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    private Animator animator;
    public Transform player;
    public float speed = 1.5f;
    public bool flipToFacePlayer = true;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
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

        Vector2 target = new Vector2(player.position.x, sueloNivel);
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
        newPos.y = sueloNivel;
        rb.MovePosition(newPos);

        float distanciaPlayer = Mathf.Abs(player.position.x - transform.position.x);
        seMueve = distanciaPlayer > 0.1f;
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
        // if (Mathf.Abs(dir) > 0.01f)
        // {
        //     Vector3 s = transform.localScale;
        //     s.x = Mathf.Sign(dir) * Mathf.Abs(s.x);
        //     transform.localScale = s;
        // }
    }

    void OnDestroy()
    {
        player = null;
    }
}