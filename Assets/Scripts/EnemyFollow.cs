using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public Transform player;
    public float speed = 3f;
    
    void Start()
    {
         if (Time.timeScale == 0f) return;

        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }
    }
    
    void Update()
    {
        if (player != null && this != null)
        {
            transform.position = Vector2.MoveTowards(
                transform.position, 
                player.position, 
                speed * Time.deltaTime
            );
        }
    }
    
    void OnDestroy()
    {
        player = null;
    }
}