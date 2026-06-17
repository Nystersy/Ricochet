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
<<<<<<< HEAD
    public static int currentLevelIndex = 0;
=======
    public static bool isInputLocked = true;
>>>>>>> 33d891e374110ecb5480056823633093554ddc02

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

    // ---------- Главное меню ----------
    public void ShowMainMenu()
    {
<<<<<<< HEAD
        SetActiveSafe(mainMenuPanel, true);
        SetActiveSafe(levelSelectPanel, false);
        SetActiveSafe(pausePanel, false);
        SetActiveSafe(winPanel, false);

=======
        mainMenuPanel.SetActive(true);
        pausePanel.SetActive(false);
        winPanel.SetActive(false);
>>>>>>> 33d891e374110ecb5480056823633093554ddc02
        Time.timeScale = 0f;
        isGameActive = false;
        isGamePaused = false;
        isInputLocked = true;
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
<<<<<<< HEAD
        SetActiveSafe(mainMenuPanel, false);
        SetActiveSafe(levelSelectPanel, false);
        SetActiveSafe(pausePanel, false);
        SetActiveSafe(winPanel, false);

=======
        mainMenuPanel.SetActive(false);
        pausePanel.SetActive(false);
        winPanel.SetActive(false);
>>>>>>> 33d891e374110ecb5480056823633093554ddc02
        Time.timeScale = 1f;
        isGameActive = true;
        isGamePaused = false;
        isInputLocked = false;
    }

    // ---------- Пауза ----------
    public void PauseGame()
    {
<<<<<<< HEAD
        SetActiveSafe(pausePanel, true);
=======
        pausePanel.SetActive(true);
>>>>>>> 33d891e374110ecb5480056823633093554ddc02
        Time.timeScale = 0f;
        isGamePaused = true;
        isInputLocked = true;
    }

    public void ResumeGame()
    {
<<<<<<< HEAD
        SetActiveSafe(pausePanel, false);
=======
        pausePanel.SetActive(false);
>>>>>>> 33d891e374110ecb5480056823633093554ddc02
        Time.timeScale = 1f;
        isGamePaused = false;
        isInputLocked = false;
    }

    // ---------- Победа ----------
    public void WinGame()
    {
<<<<<<< HEAD
        if (winPanel != null)
        {
            winPanel.SetActive(true);
            Debug.Log("Победа! Уровень пройден");
        }
        else
        {
            Debug.LogError("winPanel не назначен в GameStarter!");
        }

=======
        winPanel.SetActive(true);
>>>>>>> 33d891e374110ecb5480056823633093554ddc02
        Time.timeScale = 0f;
        isGameActive = false;
        isGamePaused = true;
        isInputLocked = true;
    }

    // ---------- Следующий уровень ----------
    public void NextLevel()
    {
<<<<<<< HEAD
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
=======
        // ������������� �����
>>>>>>> 33d891e374110ecb5480056823633093554ddc02
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitToMenu()
    {
<<<<<<< HEAD
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
=======
        // ������������� ����� � ���������� ����
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
>>>>>>> 33d891e374110ecb5480056823633093554ddc02
    }
}