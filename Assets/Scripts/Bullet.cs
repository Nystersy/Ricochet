using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private List<Vector2> points;
    private int currentTargetIndex = 1;
    private float speed;
    private bool isMoving = false;

    void Update()
    {
        if (!isMoving) return;
        if (points == null || points.Count == 0) return;

        if (currentTargetIndex < points.Count)
        {
            Vector2 target = points[currentTargetIndex];

            transform.position = Vector2.MoveTowards(
                transform.position,
                target,
                speed * Time.deltaTime
            );

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

    public void Initialize(List<Vector2> bulletPoints, List<Vector2> bulletDirections, float bulletSpeed, LayerMask layer)
    {
        points = bulletPoints;
        speed = bulletSpeed;

        if (points.Count > 0)
        {
            transform.position = points[0];
            currentTargetIndex = 1;
            isMoving = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}