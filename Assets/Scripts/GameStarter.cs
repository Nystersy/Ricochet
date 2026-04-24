using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStarter : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject pausePanel;
    public GameObject winPanel;

    public static bool isGameActive = false;
    public static bool isGamePaused = false;
    public static bool isInputLocked = true;

    void Start()
    {
        ShowMainMenu();
    }

    void Update()
    {
        if (isGameActive && !isGamePaused && Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
        else if (isGameActive && isGamePaused && Input.GetKeyDown(KeyCode.Escape))
        {
            ResumeGame();
        }
    }

    public void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        pausePanel.SetActive(false);
        winPanel.SetActive(false);
        Time.timeScale = 0f;
        isGameActive = false;
        isGamePaused = false;
        isInputLocked = true;
    }

    public void StartGame()
    {
        mainMenuPanel.SetActive(false);
        pausePanel.SetActive(false);
        winPanel.SetActive(false);
        Time.timeScale = 1f;
        isGameActive = true;
        isGamePaused = false;
        isInputLocked = false;
    }

    public void PauseGame()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        isGamePaused = true;
        isInputLocked = true;
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = false;
        isInputLocked = false;
    }

    public void WinGame()
    {
        winPanel.SetActive(true);
        Time.timeScale = 0f;
        isGameActive = false;
        isGamePaused = true;
        isInputLocked = true;
    }

    public void NextLevel()
    {
        // Перезагружаем сцену
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitToMenu()
    {
        // Перезагружаем сцену и показываем меню
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}