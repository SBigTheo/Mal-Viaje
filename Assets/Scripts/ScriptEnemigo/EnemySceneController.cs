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
        Debug.Log($"Enemigos eliminados: {enemiesKilled}/{enemiesRequired}");
        
        if (enemiesKilled >= enemiesRequired)
        {
            LoadNextScene();
        }
    }
    
    private void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        int totalScenes = SceneManager.sceneCountInBuildSettings;
        
        if (nextSceneIndex < totalScenes)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }
}