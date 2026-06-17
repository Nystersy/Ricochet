using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private List<Vector2> points;
    private int currentTargetIndex = 1;
    private float speed;
    private bool isMoving = false;
    private LayerMask enemyMask;

    // Список уже убитых врагов, чтобы не убивать одного и того же дважды
    private HashSet<Collider2D> hitEnemies = new HashSet<Collider2D>();

    void Update()
    {
        if (!isMoving || points == null) return;

        if (currentTargetIndex < points.Count)
        {
            Vector2 target = points[currentTargetIndex];

            // Поворачиваем спрайт в сторону движения
            Vector2 moveDir = target - (Vector2)transform.position;
            if (moveDir.sqrMagnitude > 0.0001f)
            {
                float angle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, 0f, angle);
            }

            transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);

            // Проверка врагов (пуля пролетает сквозь них, но убивает)
            Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, 0.3f, enemyMask);
            foreach (Collider2D enemy in enemies)
            {
                if (enemy == null) continue;
                if (hitEnemies.Contains(enemy)) continue; // уже убили

                if (enemy.CompareTag("Enemy"))
                {
                    hitEnemies.Add(enemy);

                    EnemyDeath death = enemy.GetComponent<EnemyDeath>();
                    if (death != null) death.Die();
                    else Destroy(enemy.gameObject);
                    // НЕ уничтожаем пулю — летит дальше
                }
            }

            if (Vector2.Distance(transform.position, target) < 0.05f)
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

    public void Initialize(List<Vector2> bulletPoints, float bulletSpeed, LayerMask mask)
    {
        points = bulletPoints;
        speed = bulletSpeed;
        enemyMask = mask;
        transform.position = points[0];
        currentTargetIndex = 1;
        isMoving = true;

        // Сразу выставляем правильный поворот на старте
        if (points.Count > 1)
        {
            Vector2 startDir = points[1] - points[0];
            if (startDir.sqrMagnitude > 0.0001f)
            {
                float angle = Mathf.Atan2(startDir.y, startDir.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, 0f, angle);
            }
        }
    }
}