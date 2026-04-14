using UnityEngine;

public class Enemy : MonoBehaviour
{
    void Start()
    {
        EnemyCounter counter = FindObjectOfType<EnemyCounter>();
        if (counter != null)
            counter.AddEnemy();
        else
            Debug.LogError("EnemyCounter не найден!");
    }

    void OnDestroy()
    {
        EnemyCounter counter = FindObjectOfType<EnemyCounter>();
        if (counter != null)
            counter.EnemyDied();
        else
            Debug.LogError("EnemyCounter не найден при уничтожении!");
    }
}