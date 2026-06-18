using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Звуки рикошета")]
    [Tooltip("Массив звуков рикошета — при каждом отскоке выбирается случайный")]
    public AudioClip[] ricochetSounds;
    [Range(0f, 1f)] public float ricochetVolume = 0.6f;
    [Range(0f, 0.3f)] public float ricochetVolumeVariation = 0.1f;
    [Range(0f, 0.5f)] public float ricochetPitchVariation = 0.15f;

    [Header("Звуки убийства врагов")]
    [Tooltip("Массив звуков попадания во врага — выбирается случайный")]
    public AudioClip[] hitEnemySounds;
    [Range(0f, 1f)] public float hitEnemyVolume = 0.8f;
    [Range(0f, 0.3f)] public float hitEnemyVolumeVariation = 0.1f;
    [Range(0f, 0.5f)] public float hitEnemyPitchVariation = 0.15f;

    private List<Vector2> points;
    private int currentTargetIndex = 1;
    private float speed;
    private bool isMoving = false;
    private LayerMask enemyMask;

    // Защита от двойного попадания в одного врага
    private HashSet<Collider2D> hitEnemies = new HashSet<Collider2D>();

    void Update()
    {
        if (!isMoving || points == null) return;

        if (currentTargetIndex < points.Count)
        {
            Vector2 target = points[currentTargetIndex];

            // Поворот спрайта в сторону движения
            Vector2 moveDir = target - (Vector2)transform.position;
            if (moveDir.sqrMagnitude > 0.0001f)
            {
                float angle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, 0f, angle);
            }

            transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);

            // Проверка врагов (пуля пролетает сквозь них, но убивает)
            Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, 0.2f, enemyMask);
            foreach (Collider2D enemy in enemies)
            {
                if (enemy == null) continue;
                if (hitEnemies.Contains(enemy)) continue;

                if (enemy.CompareTag("Enemy"))
                {
                    hitEnemies.Add(enemy);

                    // Звук убийства врага (своя громкость)
                    PlayRandomSound(hitEnemySounds, transform.position,
                        hitEnemyVolume, hitEnemyVolumeVariation, hitEnemyPitchVariation);

                    EnemyDeath death = enemy.GetComponent<EnemyDeath>();
                    if (death != null) death.Die();
                    else Destroy(enemy.gameObject);
                }
            }

            // Достигли точки
            if (Vector2.Distance(transform.position, target) < 0.05f)
            {
                transform.position = target;
                currentTargetIndex++;

                // Звук рикошета (своя громкость) — если впереди ещё есть точки
                if (currentTargetIndex < points.Count)
                {
                    PlayRandomSound(ricochetSounds, transform.position,
                        ricochetVolume, ricochetVolumeVariation, ricochetPitchVariation);
                }
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Воспроизводит случайный звук из массива с настраиваемой громкостью и питчем.
    /// </summary>
    void PlayRandomSound(AudioClip[] clips, Vector3 position,
        float volume, float volumeVariation, float pitchVariation)
    {
        if (clips == null || clips.Length == 0) return;

        AudioClip clip = clips[Random.Range(0, clips.Length)];
        if (clip == null) return;

        // Создаём временный объект для воспроизведения звука
        GameObject tempGO = new GameObject("TempSFX_" + clip.name);
        tempGO.transform.position = position;

        AudioSource src = tempGO.AddComponent<AudioSource>();
        src.clip = clip;
        src.volume = Mathf.Clamp01(volume + Random.Range(-volumeVariation, volumeVariation));
        src.pitch = 1f + Random.Range(-pitchVariation, pitchVariation);
        src.spatialBlend = 0f; // 2D звук
        src.Play();

        // Уничтожаем объект после окончания звука
        Destroy(tempGO, clip.length / Mathf.Abs(src.pitch) + 0.1f);
    }

    public void Initialize(List<Vector2> bulletPoints, float bulletSpeed, LayerMask mask)
    {
        points = bulletPoints;
        speed = bulletSpeed;
        enemyMask = mask;
        transform.position = points[0];
        currentTargetIndex = 1;
        isMoving = true;

        // Сразу выставляем поворот в направлении первого сегмента
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