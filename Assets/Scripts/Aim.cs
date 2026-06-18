using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Aim : MonoBehaviour
{
    [Header("Ссылки")]
    public CharacterAim characterAim;
    public LineRenderer lineRenderer;
    public GameObject bulletPrefab;

    [Header("Настройки прицельной линии")]
    public int lineReflections = 3;
    public float raycastDistance = 100f;
    public LayerMask collisionMask;

    [Header("Настройки пули")]
    public float bulletSpeed = 10f;
    public int bulletReflections = 5;
    public LayerMask enemyMask;

    [Header("Звуки")]
    public AudioClip shootSound;
    [Range(0f, 1f)] public float shootVolume = 0.7f;

    private const float SkinWidth = 0.01f;
    private AudioSource sfxSource;

    private Vector3 FirePointPosition => characterAim != null ? characterAim.GetFirePointPosition() : transform.position;

    void Start()
    {
        if (lineRenderer == null) lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer != null)
        {
            lineRenderer.startWidth = 0.02f;
            lineRenderer.endWidth = 0.02f;
            lineRenderer.positionCount = 0;
            lineRenderer.useWorldSpace = true;
        }

        sfxSource = GetComponent<AudioSource>();
        if (sfxSource == null)
            sfxSource = gameObject.AddComponent<AudioSource>();

        sfxSource.playOnAwake = false;
        sfxSource.loop = false;
    }

    void Update()
    {
        // В меню или на паузе — скрываем прицел и не обрабатываем стрельбу
        if (!GameStarter.isGameActive || GameStarter.isGamePaused)
        {
            if (lineRenderer != null && lineRenderer.enabled)
                lineRenderer.enabled = false;
            return;
        }

        // Возобновляем линию, если только что вышли из меню
        if (lineRenderer != null && !lineRenderer.enabled)
            lineRenderer.enabled = true;

        DrawPredictionLine();

        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;

            Shoot();
        }
    }

    void DrawPredictionLine()
    {
        if (lineRenderer == null) return;

        Vector3 startPoint = FirePointPosition;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        Vector2 direction = ((Vector2)(mousePos - startPoint)).normalized;

        List<Vector2> path = CalculatePath(startPoint, direction, lineReflections);

        lineRenderer.positionCount = path.Count;
        Vector3[] arr = new Vector3[path.Count];
        for (int i = 0; i < path.Count; i++)
            arr[i] = new Vector3(path[i].x, path[i].y, 0f);
        lineRenderer.SetPositions(arr);
    }

    void Shoot()
    {
        if (shootSound != null && sfxSource != null)
            sfxSource.PlayOneShot(shootSound, shootVolume);

        Vector3 startPoint = FirePointPosition;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        Vector2 direction = ((Vector2)(mousePos - startPoint)).normalized;

        List<Vector2> bulletPoints = CalculatePath(startPoint, direction, bulletReflections);

        GameObject bullet = Instantiate(bulletPrefab, startPoint, Quaternion.identity);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
            bulletScript.Initialize(bulletPoints, bulletSpeed, enemyMask);

        StartCoroutine(HideLineTemporarily());
    }

    List<Vector2> CalculatePath(Vector2 startPoint, Vector2 direction, int maxReflections)
    {
        List<Vector2> path = new List<Vector2>();
        path.Add(startPoint);

        Vector2 currentPoint = startPoint;
        Vector2 currentDir = direction;

        for (int i = 0; i < maxReflections; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(currentPoint, currentDir, raycastDistance, collisionMask);

            if (hit.collider == null)
            {
                path.Add(currentPoint + currentDir * raycastDistance);
                break;
            }

            path.Add(hit.point);
            currentDir = Vector2.Reflect(currentDir, hit.normal).normalized;
            currentPoint = hit.point + hit.normal * SkinWidth;
        }

        return path;
    }

    IEnumerator HideLineTemporarily()
    {
        if (lineRenderer != null) lineRenderer.enabled = false;
        yield return new WaitForSeconds(0.2f);
        // Включаем обратно только если игра активна
        if (lineRenderer != null && GameStarter.isGameActive && !GameStarter.isGamePaused)
            lineRenderer.enabled = true;
    }
}