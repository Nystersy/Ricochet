/*using UnityEngine;
using TMPro;

public class EnemyCounter : MonoBehaviour
{
    public TextMeshProUGUI counterText;
    private int enemiesRemaining = 0;

    private GameStarter gameStarter;

    void Start()
    {
        gameStarter = FindObjectOfType<GameStarter>();
        UpdateUI();
    }

    public void AddEnemy()
    {
        enemiesRemaining++;
        UpdateUI();
    }

    public void EnemyDied()
    {
        enemiesRemaining--;
        UpdateUI();

        if (enemiesRemaining <= 0 && gameStarter != null)
        {
            gameStarter.WinGame();
        }
    }

    void UpdateUI()
    {
        if (counterText != null)
            counterText.text = $"Врагов: {enemiesRemaining}";
    }
}*/