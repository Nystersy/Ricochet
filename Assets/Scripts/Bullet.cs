using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private List<Vector2> points;
    private int currentTargetIndex = 1;
    private float speed;
    private bool isMoving = false;

    public LayerMask enemyLayer; // Слой врага

    void Update()
    {
        if (!isMoving) return;
        if (points == null) return;

        if (currentTargetIndex < points.Count)
        {
            Vector2 target = points[currentTargetIndex];

            // Движение
            transform.position = Vector2.MoveTowards(
                transform.position,
                target,
                speed * Time.deltaTime
            );

            // Ручная проверка попадания во врага
            CheckEnemyHit();

            if (Vector2.Distance(transform.position, target) < 0.1f)
            {
                transform.position = target;
                currentTargetIndex++;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void CheckEnemyHit()
    {
        // Пускаем маленький луч во все стороны
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.3f, enemyLayer);

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                Destroy(hit.gameObject);
                Destroy(gameObject);
                break;
            }
        }
    }

    public void Initialize(List<Vector2> bulletPoints, List<Vector2> bulletDirections, float bulletSpeed, LayerMask layer)
    {
        points = bulletPoints;
        speed = bulletSpeed;
        enemyLayer = layer;

        transform.position = points[0];
        currentTargetIndex = 1;
        isMoving = true;
    }

    // Визуализация для отладки
    void OnDrawGizmos()
    {
        if (!isMoving) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 0.3f);
    }
}