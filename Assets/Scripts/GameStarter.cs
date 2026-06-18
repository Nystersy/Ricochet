using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStarter : MonoBehaviour
{
    [Header("Панели UI")]
    public GameObject mainMenuPanel;
    public GameObject levelSelectPanel;
    public GameObject pausePanel;
    public GameObject winPanel;

    [Header("HUD")]
    public GameObject hudPanel;

    [Header("Уровни")]
    [Tooltip("Имена сцен уровней в порядке прохождения. ВАЖНО: заполняй одинаково во всех GameStarter!")]
    public string[] levelSceneNames;

    [Header("Сцены")]
    public string mainMenuSceneName = "MainMenu";

    public static bool isGameActive = false;
    public static bool isGamePaused = false;

    void Start()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == mainMenuSceneName)
        {
            ShowMainMenu();
        }
        else
        {
            // Это игровая сцена — сразу запускаем игру
            ActivateGameplay();
        }
    }

    void Update()
    {
        if (!isGameActive) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused) ResumeGame();
            else PauseGame();
        }

        if (!isGamePaused && Input.GetKeyDown(KeyCode.R))
        {
            ReloadCurrentScene();
        }
    }

    // ---------- Главное меню ----------
    public void ShowMainMenu()
    {
        SetActiveSafe(mainMenuPanel, true);
        SetActiveSafe(levelSelectPanel, false);
        SetActiveSafe(pausePanel, false);
        SetActiveSafe(winPanel, false);
        SetActiveSafe(hudPanel, false);

        Time.timeScale = 0f;
        isGameActive = false;
        isGamePaused = false;
    }

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

    // ---------- Старт игры (кнопка "Играть" в меню) ----------
    public void StartGame()
    {
        if (levelSceneNames != null && levelSceneNames.Length > 0)
        {
            LoadLevel(0);
        }
        else
        {
            Debug.LogError("Массив levelSceneNames пуст! Добавь уровни в инспекторе GameStarter.");
        }
    }

    /// <summary>
    /// Активирует геймплей (вызывается в Start() игровой сцены)
    /// </summary>
    void ActivateGameplay()
    {
        SetActiveSafe(mainMenuPanel, false);
        SetActiveSafe(levelSelectPanel, false);
        SetActiveSafe(pausePanel, false);
        SetActiveSafe(winPanel, false);
        SetActiveSafe(hudPanel, true);

        Time.timeScale = 1f;
        isGameActive = true;
        isGamePaused = false;
    }

    // ---------- Загрузка уровня ----------
    public void LoadLevel(int levelIndex)
    {
        if (levelSceneNames == null || levelIndex < 0 || levelIndex >= levelSceneNames.Length)
        {
            Debug.LogError($"Неверный индекс уровня: {levelIndex}");
            return;
        }

        Time.timeScale = 1f;
        isGameActive = true;
        isGamePaused = false;

        SceneManager.LoadScene(levelSceneNames[levelIndex]);
    }

    // ---------- Следующий уровень ----------
    public void NextLevel()
    {
        if (levelSceneNames == null || levelSceneNames.Length == 0)
        {
            Debug.LogError("Массив levelSceneNames пуст!");
            return;
        }

        string currentSceneName = SceneManager.GetActiveScene().name;

        // Ищем индекс текущей сцены в массиве уровней
        int currentIndex = -1;
        for (int i = 0; i < levelSceneNames.Length; i++)
        {
            if (levelSceneNames[i] == currentSceneName)
            {
                currentIndex = i;
                break;
            }
        }

        if (currentIndex == -1)
        {
            Debug.LogError($"Текущая сцена '{currentSceneName}' не найдена в массиве levelSceneNames! Проверь поле Level Scene Names в инспекторе GameStarter.");
            return;
        }

        int nextIndex = currentIndex + 1;

        if (nextIndex < levelSceneNames.Length)
        {
            Debug.Log($"Загружаем следующий уровень: {levelSceneNames[nextIndex]}");
            LoadLevel(nextIndex);
        }
        else
        {
            Debug.Log("Все уровни пройдены! Возвращаемся в меню.");
            QuitToMenu();
        }
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
        if (winPanel != null) winPanel.SetActive(true);
        SetActiveSafe(hudPanel, false);

        Time.timeScale = 0f;
        isGameActive = false;
        isGamePaused = true;
    }

    // ---------- Перезапуск ----------
    public void ReloadCurrentScene()
    {
        Time.timeScale = 1f;
        isGameActive = true;
        isGamePaused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // ---------- Выход в меню ----------
    public void QuitToMenu()
    {
        Time.timeScale = 1f;
        isGameActive = false;
        isGamePaused = false;
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void SetActiveSafe(GameObject go, bool state)
    {
        if (go != null) go.SetActive(state);
    }
}