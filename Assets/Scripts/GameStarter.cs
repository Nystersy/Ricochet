using UnityEngine;

public class GameStarter : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject pausePanel;
    public GameObject winPanel;

    public static bool isGameActive = false;
    public static bool isGamePaused = false;

    void Start()
    {
        ShowMainMenu();
    }

    void Update()
    {
        if (isGameActive && Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void ShowMainMenu()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        if (pausePanel != null) pausePanel.SetActive(false);
        if (winPanel != null) winPanel.SetActive(false);
        Time.timeScale = 0f;
        isGameActive = false;
        isGamePaused = false;
    }

    public void StartGame()
    {
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(false);

        if (pausePanel != null)
            pausePanel.SetActive(false);

        if (winPanel != null)
            winPanel.SetActive(false);

        Time.timeScale = 1f;
        isGameActive = true;
        isGamePaused = false;
    }

    public void PauseGame()
    {
        if (pausePanel != null) pausePanel.SetActive(true);
        Time.timeScale = 0f;
        isGamePaused = true;
    }

    public void ResumeGame()
    {
        if (pausePanel != null) pausePanel.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = false;
    }

    public void WinGame()
    {
        if (winPanel != null)
        {
            winPanel.SetActive(true);
            Debug.Log("Победа! Панель активирована");
        }
        else
        {
            Debug.LogError("winPanel не назначен в GameStarter!");
        }

        Time.timeScale = 0f;
        isGameActive = false;
        isGamePaused = true;
    }

    public void NextLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex
        );
        isGameActive = true;
        isGamePaused = false;
        Time.timeScale = 1f;
    }

    public void QuitToMenu()
    {
        ShowMainMenu();
    }
}