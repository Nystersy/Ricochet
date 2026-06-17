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
    public int lineReflections = 3;        // сколько отскоков показывает линия
    public float raycastDistance = 100f;
    public LayerMask collisionMask;        // только стены (Walls) — для расчёта рикошетов

    [Header("Настройки пули")]
    public float bulletSpeed = 10f;
    public int bulletReflections = 5;      // сколько реальных отскоков делает пуля
    public LayerMask enemyMask;            // только враги (Enemy) — для проверки попаданий

    private const float SkinWidth = 0.01f; // отступ от стены при отражении

    private Vector3 FirePointPosition => characterAim != null ? characterAim.GetFirePointPosition() : transform.position;

    void Start()
    {
        if (lineRenderer == null) lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer != null)
        {
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;
            lineRenderer.positionCount = 0;
            lineRenderer.useWorldSpace = true;
        }
    }

    void Update()
    {
        DrawPredictionLine();
        if (Input.GetMouseButtonDown(0))
{
    // Если курсор над UI — не стрелять
    if (UnityEngine.EventSystems.EventSystem.current != null &&
        UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        return;

    // Если игра на паузе или не активна — тоже не стрелять
    if (!GameStarter.isGameActive || GameStarter.isGamePaused)
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
        Vector3 startPoint = FirePointPosition;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        Vector2 direction = ((Vector2)(mousePos - startPoint)).normalized;

        List<Vector2> bulletPoints = CalculatePath(startPoint, direction, bulletReflections);

        GameObject bullet = Instantiate(bulletPrefab, startPoint, Quaternion.identity);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            // Передаём маску ВРАГОВ, чтобы пуля проверяла попадания именно по ним
            bulletScript.Initialize(bulletPoints, bulletSpeed, enemyMask);
        }

        StartCoroutine(HideLineTemporarily());
    }

    /// <summary>
    /// Рассчитывает путь луча с отражениями от стен.
    /// Врагов игнорирует (их нет в collisionMask) — пуля будет пролетать сквозь них.
    /// Возвращает список точек: [старт, точка_отражения_1, точка_отражения_2, ..., конец]
    /// </summary>
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
                // Стен впереди нет - летим до конца
                path.Add(currentPoint + currentDir * raycastDistance);
                break;
            }

            // Добавляем точку столкновения
            path.Add(hit.point);

            // Отражаем направление от нормали стены
            currentDir = Vector2.Reflect(currentDir, hit.normal).normalized;

            // Смещаемся ПО НОРМАЛИ наружу от стены, чтобы луч не застрял в углу
            currentPoint = hit.point + hit.normal * SkinWidth;
        }

        return path;
    }

    IEnumerator HideLineTemporarily()
    {
        if (lineRenderer != null) lineRenderer.enabled = false;
        yield return new WaitForSeconds(0.2f);
        if (lineRenderer != null) lineRenderer.enabled = true;
    }
}