using UnityEngine;
using UnityEngine.SceneManagement;

public class SUBE : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameFlowManager.Instance.RegisterCollectable();
            Destroy(gameObject);
        }
    }
}