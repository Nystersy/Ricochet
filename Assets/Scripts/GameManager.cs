using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Ссылки")]
    public GameStarter gameStarter;
    public TMP_Text enemyCounterText;    // если используешь TextMeshPro
    // public Text enemyCounterText;      // если обычный UI Text (раскомментируй вместо строки выше)

    [Header("Настройки")]
    [Tooltip("Тег, по которому считаются враги")]
    public string enemyTag = "Enemy";

    [Tooltip("Задержка перед показом окна победы (секунды)")]
    public float winDelay = 0.5f;

    private int enemiesAlive;
    private bool hasWon = false;

    void Start()
    {
        // Если GameStarter не назначен — найдём его на сцене
        if (gameStarter == null)
            gameStarter = FindObjectOfType<GameStarter>();

        CountEnemies();
        UpdateCounterUI();
    }

    void Update()
    {
        if (hasWon) return;

        // Каждый кадр пересчитываем врагов
        // (можно оптимизировать, но для маленьких уровней это нормально)
        CountEnemies();
        UpdateCounterUI();

        if (enemiesAlive <= 0)
        {
            hasWon = true;
            Invoke(nameof(TriggerWin), winDelay);
        }
    }

    void CountEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        enemiesAlive = enemies.Length;
    }

    void UpdateCounterUI()
    {
        if (enemyCounterText != null)
            enemyCounterText.text = $"Врагов: {enemiesAlive}";
    }

    void TriggerWin()
    {
        if (gameStarter != null)
        {
            Debug.Log("Победа! Все враги уничтожены");
            gameStarter.WinGame();
        }
        else
        {
            Debug.LogError("GameStarter не найден!");
        }
    }
}