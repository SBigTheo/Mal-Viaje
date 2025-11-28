using UnityEngine;
using UnityEngine.SceneManagement;

public class SUBE : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth player = collision.GetComponent<PlayerHealth>(); 
            if (player != null)
            {
                player.WinGame();

                SceneManager.LoadScene("SlideNivel3");

                Destroy(gameObject);
            }
        }
    }
}