using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Collider2D enemyCollider;
    [SerializeField] private MonoBehaviour[] scriptsToDisable;

    private bool isDead = false;

    // Этот метод вызывается ТОЛЬКО из пули (или другого внешнего источника)
    public void Die()
    {
        Debug.Log("Die() вызван. Стек вызовов:\n" + StackTraceUtility.ExtractStackTrace());
        if (isDead) return;
        isDead = true;

        if (enemyCollider != null) enemyCollider.enabled = false;

        foreach (MonoBehaviour script in scriptsToDisable)
            if (script != null) script.enabled = false;

        if (animator != null)
            animator.SetTrigger("death");
        else
            Destroy(gameObject, 0.1f);
    }

    // Вызывается из Animation Event в конце анимации смерти
    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}