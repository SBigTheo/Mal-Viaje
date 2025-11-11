using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public Transform player;
    public float speed = 3f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }
    }

    void FixedUpdate()
    {
        if (player == null) return;

        Vector3 target = new Vector3(player.position.x, player.position.y, transform.position.z);

        Vector3 next = Vector3.MoveTowards(transform.position, target, speed * Time.fixedDeltaTime);
        transform.position = next;
    }

    void OnDestroy()
    {
        player = null;
    }
}