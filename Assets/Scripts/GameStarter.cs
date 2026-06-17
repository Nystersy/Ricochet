using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStarter : MonoBehaviour
{
    [Header("Панели UI")]
    public GameObject mainMenuPanel;
    public GameObject levelSelectPanel;  // новая панель выбора уровня
    public GameObject pausePanel;
    public GameObject winPanel;

    [Header("Уровни")]
    [Tooltip("Имена сцен уровней в порядке прохождения (должны быть добавлены в Build Settings)")]
    public string[] levelSceneNames;

    public static bool isGameActive = false;
    public static bool isGamePaused = false;
    public static int currentLevelIndex = 0;

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

    // ---------- Главное меню ----------
    public void ShowMainMenu()
    {
        SetActiveSafe(mainMenuPanel, true);
        SetActiveSafe(levelSelectPanel, false);
        SetActiveSafe(pausePanel, false);
        SetActiveSafe(winPanel, false);

        Time.timeScale = 0f;
        isGameActive = false;
        isGamePaused = false;
    }

    // ---------- Выбор уровня ----------
    public void ShowLevelSelect()
    {
        SetActiveSafe(mainMenuPanel, false);
        SetActiveSafe(levelSelectPanel, true);
    }

    public void BackToMainMenu()
    {
        SetActiveSafe(levelSelectPanel, false);
        SetActiveSafe(mainMenuPanel, true);
    }

    /// <summary>
    /// Загружает уровень по индексу из массива levelSceneNames.
    /// Привяжи этот метод к кнопкам выбора уровня (в OnClick укажи номер).
    /// </summary>
    public void LoadLevel(int levelIndex)
    {
        if (levelSceneNames == null || levelIndex < 0 || levelIndex >= levelSceneNames.Length)
        {
            Debug.LogError($"Неверный индекс уровня: {levelIndex}");
            return;
        }

        currentLevelIndex = levelIndex;
        Time.timeScale = 1f;
        isGameActive = true;
        isGamePaused = false;

        SceneManager.LoadScene(levelSceneNames[levelIndex]);
    }

    // ---------- Старт текущей сцены (для кнопки "Играть" в главном меню) ----------
    public void StartGame()
    {
        SetActiveSafe(mainMenuPanel, false);
        SetActiveSafe(levelSelectPanel, false);
        SetActiveSafe(pausePanel, false);
        SetActiveSafe(winPanel, false);

        Time.timeScale = 1f;
        isGameActive = true;
        isGamePaused = false;
    }

    // ---------- Пауза ----------
    public void PauseGame()
    {
        SetActiveSafe(pausePanel, true);
        Time.timeScale = 0f;
        isGamePaused = true;
    }

    public void ResumeGame()
    {
        SetActiveSafe(pausePanel, false);
        Time.timeScale = 1f;
        isGamePaused = false;
    }

    // ---------- Победа ----------
    public void WinGame()
    {
        if (winPanel != null)
        {
            winPanel.SetActive(true);
            Debug.Log("Победа! Уровень пройден");
        }
        else
        {
            Debug.LogError("winPanel не назначен в GameStarter!");
        }

        Time.timeScale = 0f;
        isGameActive = false;
        isGamePaused = true;
    }

    // ---------- Следующий уровень ----------
    public void NextLevel()
    {
        int nextIndex = currentLevelIndex + 1;

        if (levelSceneNames != null && nextIndex < levelSceneNames.Length)
        {
            LoadLevel(nextIndex);
        }
        else
        {
            // Если уровней больше нет — возвращаемся в главное меню
            Debug.Log("Все уровни пройдены!");
            ReloadCurrentScene();
        }
    }

    // ---------- Перезапуск текущей сцены ----------
    public void ReloadCurrentScene()
    {
        Time.timeScale = 1f;
        isGameActive = true;
        isGamePaused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitToMenu()
    {
        // Если меню в той же сцене — просто показать панель
        ShowMainMenu();

        // Если меню в отдельной сцене — раскомментируй:
        // SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    // ---------- Утилита ----------
    private void SetActiveSafe(GameObject go, bool state)
    {
        if (go != null) go.SetActive(state);
    }
}