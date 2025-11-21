using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    private Animator animator;
    public Transform player;
    public float speed = 3f;
    public bool flipToFacePlayer = true;

    private Rigidbody2D rb;
    private bool seMueve = false;
    private SpriteRenderer sprite;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        if (player == null)
            TryFindPlayer();
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

        Vector2 target = new Vector2(player.position.x, transform.position.y);
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);

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