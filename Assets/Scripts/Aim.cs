using System.Collections.Generic;
using UnityEngine;

public class Aim : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public int maxReflections = 5;
    public LayerMask collisionMask;

    private List<Vector3> points = new List<Vector3>(); 

    void Start()
    {
        if (lineRenderer == null)
            lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.positionCount = 0;
    }

    void Update()
    {
        DrawPredictionLine();

        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void DrawPredictionLine()
    {
        Vector2 startPoint = transform.position;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - startPoint).normalized;

        points.Clear();
        points.Add(startPoint); 

        Vector2 currentPoint = startPoint;
        Vector2 currentDir = direction;
        bool hitEnemy = false;

        for (int i = 0; i < maxReflections; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(currentPoint, currentDir, 100f, collisionMask);

            if (hit.collider != null)
            {
                points.Add(hit.point); 

                if (hit.collider.CompareTag("Enemy"))
                {
                    hitEnemy = true;
                    break;
                }

                currentDir = Vector2.Reflect(currentDir, hit.normal);
                currentPoint = hit.point + currentDir * 0.05f;
            }
            else
            {
                points.Add(currentPoint + currentDir * 20f);
                break;
            }
        }

        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());

        if (hitEnemy)
        {
            lineRenderer.startColor = Color.red;
            lineRenderer.endColor = Color.red;
        }
        else
        {
            lineRenderer.startColor = Color.white;
            lineRenderer.endColor = Color.white;
        }
    }

    void Shoot()
    {
        Vector2 startPoint = transform.position;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - startPoint).normalized;

        Vector2 currentPoint = startPoint;
        Vector2 currentDir = direction;

        for (int i = 0; i < maxReflections; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(currentPoint, currentDir, 100f, collisionMask);

            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    Destroy(hit.collider.gameObject);
                    Debug.Log("Враг уничтожен!");
                    break;
                }

                currentDir = Vector2.Reflect(currentDir, hit.normal);
                currentPoint = hit.point + currentDir * 0.05f;
            }
            else
            {
                break;
            }
        }
    }
}