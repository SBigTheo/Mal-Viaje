using UnityEngine;

public class Plata : MonoBehaviour
{
    [Header("Configuración")]
    public string playerTag = "Player";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(playerTag))
        {
            PlayerHealth player = collision.GetComponent<PlayerHealth>(); // CORREGIDO: usar collision
            if (player != null)
            {
                player.WinGame();
                Destroy(gameObject);
            }
        }
    }
}