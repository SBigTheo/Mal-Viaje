using UnityEngine;
using UnityEngine.SceneManagement;

public class SUBE : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Player"))
        {
            PlayerHealth player = collision.GetComponent<PlayerHealth>(); // CORREGIDO: usar collision
            if (player != null)
            {
                player.WinGame();
                Destroy(gameObject);
            };
        }
    }
}