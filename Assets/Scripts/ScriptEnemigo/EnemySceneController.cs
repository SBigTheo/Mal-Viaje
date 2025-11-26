using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySceneController : MonoBehaviour
{
    [Header("Configuración")]
    public int enemiesRequired = 10;

    private int enemiesKilled = 0;

    public void RegisterEnemyKill()
    {
        enemiesKilled++;

        if (GameFlowManager.Instance != null)
        {
            GameFlowManager.Instance.RegisterEnemyKill();
        }
    }
}