using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    void OnDestroy()
    {
        // Находим все объекты с тегом "Enemy"
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // Если этот враг был последним (остался только тот, который сейчас умирает)
        if (enemies.Length <= 1)
        {
            GameStarter starter = FindFirstObjectByType<GameStarter>();
            if (starter != null)
            {
                starter.WinGame();
                Debug.Log("ПОБЕДА! Все враги уничтожены.");
            }
        }
    }
}