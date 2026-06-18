using UnityEngine;

public class CharacterAim : MonoBehaviour
{
    [SerializeField] private Transform upperBody;
    [SerializeField] private Transform body;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float maxLeanAngle = 60f;

    private SpriteRenderer upperSprite;
    private SpriteRenderer bodySprite;
    public bool IsFacingRight { get; private set; } = true;

    // Сохраняем исходную локальную позицию FirePoint (для взгляда вправо)
    private Vector3 firePointLocalPos;

    void Start()
    {
        if (upperBody == null)
            upperBody = transform.Find("UpperBody");
        if (body == null)
            body = transform.Find("Body");

        upperSprite = upperBody?.GetComponent<SpriteRenderer>();
        bodySprite = body?.GetComponent<SpriteRenderer>();

        if (firePoint == null && upperBody != null)
            firePoint = upperBody.Find("FirePoint");

        if (firePoint != null)
            firePointLocalPos = firePoint.localPosition;
    }

    void Update()
    {
        // В меню или на паузе — игрок замирает, не реагирует на мышь
        if (!GameStarter.isGameActive || GameStarter.isGamePaused)
            return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.position;

        bool shouldFaceRight = direction.x > 0;

        if (shouldFaceRight != IsFacingRight)
        {
            IsFacingRight = shouldFaceRight;
            // Зеркалим спрайты
            if (upperSprite != null) upperSprite.flipX = !IsFacingRight;
            if (bodySprite != null) bodySprite.flipX = !IsFacingRight;

            // Зеркалим FirePoint
            if (firePoint != null)
            {
                Vector3 newPos = firePointLocalPos;
                newPos.x = Mathf.Abs(newPos.x) * (IsFacingRight ? 1f : -1f);
                firePoint.localPosition = newPos;
            }
        }

        // Поворот верхней части
        Vector2 forward = IsFacingRight ? Vector2.right : Vector2.left;
        float targetAngle = Vector2.SignedAngle(forward, direction);
        targetAngle = Mathf.Clamp(targetAngle, -maxLeanAngle, maxLeanAngle);
        upperBody.rotation = Quaternion.Euler(0, 0, targetAngle);
    }

    public Vector3 GetFirePointPosition()
    {
        if (firePoint != null)
            return firePoint.position;
        else
            return transform.position;
    }
}