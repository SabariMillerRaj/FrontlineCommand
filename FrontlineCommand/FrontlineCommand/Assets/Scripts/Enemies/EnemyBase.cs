using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EnemyBase : MonoBehaviour
{
    [Header("Stats")]
    public int maxHP = 30;
    public int outpostDamage = 10;
    public int resourceDrop = 5;

    protected int currentHP;
    protected bool isDead = false;

    [Header("Visuals")]
    public SpriteRenderer spriteRenderer;
    public Color hitColor = Color.red;

    private Color originalColor;

    protected virtual void Awake()
    {
        currentHP = maxHP;
        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;
    }

    public virtual void TakeDamage(int dmg)
    {
        if (isDead) return;
        currentHP -= dmg;
        StartCoroutine(FlashHit());

        if (currentHP <= 0) Die();
    }

    protected virtual void Die()
    {
        if (isDead) return;
        isDead = true;
        AudioManager.Instance?.PlayExplosion();
        GameManager.Instance?.AddResources(resourceDrop);
        SpawnManager.Instance?.OnEnemyKilled();
        Destroy(gameObject);
    }

    // Called when enemy reaches the outpost
    public virtual void ReachOutpost()
    {
        GameManager.Instance?.DamageOutpost(outpostDamage);
        Destroy(gameObject);
    }

    System.Collections.IEnumerator FlashHit()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = hitColor;
            yield return new WaitForSeconds(0.08f);
            spriteRenderer.color = originalColor;
        }
    }
}
