using UnityEngine;
using UnityEngine.SceneManagement;

public class Plata : MonoBehaviour
{
    [Header("Configuración")]
    public string playerTag = "Player";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(playerTag))
        {
            PlayerHealth player = collision.GetComponent<PlayerHealth>(); 
            if (player != null)
            {
                player.WinGame();

                SceneManager.LoadScene("SlideNivel2");

                Destroy(gameObject);
            }
        }
    }
}