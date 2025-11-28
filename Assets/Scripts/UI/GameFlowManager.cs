using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlowManager : MonoBehaviour
{
    public static GameFlowManager Instance;

    [Header("UI References")]
    public GameObject victoryPanel;
    public GameObject defeatPanel;

    [Header("Level Settings")]
    public string slideSceneName = "SlideNivel1";
    public string nextLevelScene = "Nivel2";

    [Header("Enemy System")]
    public int enemiesRequired = 10; // Agregar esta variable

    private bool gameEnded = false;
    private int enemiesKilled = 0; // Agregar este contador

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //AGREGAR ESTE MÉTODO
    public void RegisterEnemyKill()
    {
        enemiesKilled++;
        Debug.Log($"Enemigos eliminados: {enemiesKilled}/{enemiesRequired}");
    }

    public void PlayerDied()
    {
        if (gameEnded) return;

        gameEnded = true;
        Time.timeScale = 1f;
        GoToMainMenu();
    }

    public void PlayerWon()
    {
        if (gameEnded) return;

        gameEnded = true;
        Time.timeScale = 0f;
        if (victoryPanel != null)
            victoryPanel.SetActive(true);
    }

    //Metodo para los botones de UI
    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void ContinueToSlideScene()
    {
        Time.timeScale = 1f;
        if (!string.IsNullOrEmpty(slideSceneName))
        {
            SceneManager.LoadScene(slideSceneName);
        }
        else
        {
            GoToNextLevel();
        }
    }

    public void GoToNextLevel()
    {
        Time.timeScale = 1f;
        if (!string.IsNullOrEmpty(nextLevelScene))
        {
            SceneManager.LoadScene(nextLevelScene);
        }
        else
        {
            GoToMainMenu();
        }
    }

    // Método para resetear contadores cuando cambia de nivel
    public void ResetEnemyCount()
    {
        enemiesKilled = 0;
        gameEnded = false;
    }
}