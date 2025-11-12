using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public Transform player;
    public float speed = 3f;
    public bool flipToFacePlayer = true;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        if (player == null)
            TryFindPlayer();
    }

    void FixedUpdate()
    {
        if (player == null)
        {
            TryFindPlayer();
            if (player == null) return;
        }

        Vector2 target = new Vector2(player.position.x, transform.position.y);
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);

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
        if (Mathf.Abs(dir) > 0.01f)
        {
            Vector3 s = transform.localScale;
            s.x = Mathf.Sign(dir) * Mathf.Abs(s.x);
            transform.localScale = s;
        }
    }

    void OnDestroy()
    {
        player = null;
    }
}