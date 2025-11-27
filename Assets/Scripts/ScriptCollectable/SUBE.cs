using UnityEngine;
using UnityEngine.SceneManagement;

public class SUBE : MonoBehaviour
{
    private bool yaRecolectado = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(yaRecolectado)return;
        
        if (collision.CompareTag("Player"))
        {
            yaRecolectado = true;

            if(GameFlowManager.Instance != null)
            GameFlowManager.Instance.RegisterCollectable();

            Destroy(gameObject);
        }
    }
}