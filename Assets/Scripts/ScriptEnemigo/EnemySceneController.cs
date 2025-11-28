using UnityEngine;

public class EnemySceneController : MonoBehaviour
{
    [Header("Configuración")]
    public int enemiesRequired = 10;

    private int enemiesKilled = 0;

    public void RegisterEnemyKill()
    {
        enemiesKilled++;

        // Notificar al GameFlowManager
        if (GameFlowManager.Instance != null)
        {
            GameFlowManager.Instance.RegisterEnemyKill();
        }

        Debug.Log($"Enemigos eliminados en escena: {enemiesKilled}/{enemiesRequired}");
    }

    // Método para que los enemigos llamen cuando mueren
    public void OnEnemyKilled()
    {
        RegisterEnemyKill();
    }
}