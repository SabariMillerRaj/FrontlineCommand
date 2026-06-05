using UnityEngine;

public abstract class UnitBase : MonoBehaviour
{
    [Header("Combat")]
    public float attackRange = 3f;
    public int damage = 10;
    public float fireRate = 1f; // shots per second

    [Header("Bullet")]
    public GameObject bulletPrefab;
    public Transform firePoint;

    protected float fireTimer = 0f;
    protected EnemyBase currentTarget;

    void Update()
    {
        if (GameManager.Instance == null || GameManager.Instance.isPlacementPhase) return;

        fireTimer += Time.deltaTime;
        FindTarget();

        if (currentTarget != null && fireTimer >= 1f / fireRate)
        {
            Shoot();
            fireTimer = 0f;
        }
    }

    void FindTarget()
    {
        // Find nearest enemy in range
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange, LayerMask.GetMask("Enemy"));
        float closest = Mathf.Infinity;
        currentTarget = null;

        foreach (var hit in hits)
        {
            EnemyBase enemy = hit.GetComponent<EnemyBase>();
            if (enemy == null || enemy.gameObject == null) continue;

            float dist = Vector2.Distance(transform.position, hit.transform.position);
            if (dist < closest)
            {
                closest = dist;
                currentTarget = enemy;
            }
        }
    }

    protected virtual void Shoot()
    {
        if (bulletPrefab != null && firePoint != null && currentTarget != null)
        {
            GameObject b = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            Bullet bullet = b.GetComponent<Bullet>();
            if (bullet != null)
                bullet.Init(currentTarget.transform, damage);
        }
        AudioManager.Instance?.PlayGunShot();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
