using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameFlowManager : MonoBehaviour
{
    public static GameFlowManager Instance;

    [Header("UI")]
    public GameObject gameOverPanel;
    public GameObject victoryPanel;

    public Button retryButton;
    public Button playAgainButton;
    public Button menuButton;

    [Header("Progreso del Nivel")]
    public int enemiesRequired = 0;
    private int enemiesKilled = 0;

    private bool gameEnded = false;

    void Awake()
    {
        Instance = this;
        gameEnded = false;
        Time.timeScale = 1f;
    }

    void Start()
    {
        if (gameOverPanel) gameOverPanel.SetActive(false);
        if (victoryPanel) victoryPanel.SetActive(false);

        if (retryButton) retryButton.onClick.AddListener(RestartLevel);
        if (playAgainButton) playAgainButton.onClick.AddListener(RestartLevel);
        if (menuButton) menuButton.onClick.AddListener(GoToMenu);
    }

    public void TriggerGameOver()
    {
        if (gameEnded) return;
        gameEnded = true;

        if (gameOverPanel)
            gameOverPanel.SetActive(true);

        Time.timeScale = 0f;
    }

    public void TriggerVictory()
    {
        if (gameEnded) return;
        gameEnded = true;

        if (victoryPanel)
            victoryPanel.SetActive(true);

        Time.timeScale = 0f;
    }

    public void RegisterEnemyKill()
    {
        enemiesKilled++;

        if (enemiesRequired > 0 && enemiesKilled >= enemiesRequired)
            TriggerVictory();
    }

    public void RegisterCollectable()
    {
        TriggerVictory();
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}