using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aim : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public int maxReflectionsLine = 2;
    public int maxReflectionsBullet = 5;
    public LayerMask collisionMask;
    public GameObject bulletPrefab;
    public float bulletSpeed = 15f;
    public float raycastDistance = 100f;
    public float wallBounceOffset = 0.2f;

    private List<Vector3> points = new List<Vector3>();

    void Start()
    {
        if (lineRenderer == null)
            lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.positionCount = 0;
        lineRenderer.useWorldSpace = true;
    }

    void Update()
    {
        if (GameStarter.isInputLocked || !GameStarter.isGameActive)
        {
            if (lineRenderer.enabled)
                lineRenderer.enabled = false;
            return;
        }

        if (!lineRenderer.enabled)
            lineRenderer.enabled = true;

        DrawPredictionLine();

        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void DrawPredictionLine()
    {
        Vector3 startPoint = transform.position;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        Vector3 direction = (mousePos - startPoint).normalized;

        points.Clear();
        points.Add(startPoint);

        Vector3 currentPoint = startPoint;
        Vector3 currentDir = direction;

        for (int i = 0; i < maxReflectionsLine; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(currentPoint, currentDir, raycastDistance, collisionMask);

            if (hit.collider != null)
            {
                Vector3 hitPoint = new Vector3(hit.point.x, hit.point.y, 0);
                points.Add(hitPoint);

                if (hit.collider.CompareTag("Enemy"))
                {
                    break;
                }

                Vector2 reflected = Vector2.Reflect(currentDir, hit.normal);
                currentDir = new Vector3(reflected.x, reflected.y, 0);
                currentPoint = hitPoint + currentDir * wallBounceOffset;
            }
            else
            {
                points.Add(currentPoint + currentDir * raycastDistance);
                break;
            }
        }

        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());

        lineRenderer.startColor = Color.white;
        lineRenderer.endColor = Color.white;
    }

    void Shoot()
    {
        Vector3 startPoint = transform.position;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        Vector3 direction = (mousePos - startPoint).normalized;

        List<Vector2> bulletPoints = new List<Vector2>();
        List<Vector2> bulletDirections = new List<Vector2>();

        Vector2 currentPoint = startPoint;
        Vector2 currentDir = direction;

        bulletPoints.Add(currentPoint);
        bulletDirections.Add(currentDir);

        for (int i = 0; i < maxReflectionsBullet; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(currentPoint, currentDir, raycastDistance, collisionMask);

            if (hit.collider != null)
            {
                bulletPoints.Add(hit.point);
                bulletDirections.Add(currentDir);

                if (hit.collider.CompareTag("Enemy"))
                {
                    break;
                }

                currentDir = Vector2.Reflect(currentDir, hit.normal);
                currentDir.Normalize();
                currentPoint = hit.point + currentDir * wallBounceOffset;
            }
            else
            {
                bulletPoints.Add(currentPoint + currentDir * raycastDistance);
                bulletDirections.Add(currentDir);
                break;
            }
        }

        GameObject bullet = Instantiate(bulletPrefab, startPoint, Quaternion.identity);
        Bullet bulletScript = bullet.GetComponent<Bullet>();

        if (bulletScript != null)
        {
            bulletScript.Initialize(bulletPoints, bulletDirections, bulletSpeed, collisionMask);
        }

        StartCoroutine(HideLineTemporarily());
    }

    IEnumerator HideLineTemporarily()
    {
        lineRenderer.enabled = false;
        yield return new WaitForSeconds(0.2f);
        lineRenderer.enabled = true;
    }
}