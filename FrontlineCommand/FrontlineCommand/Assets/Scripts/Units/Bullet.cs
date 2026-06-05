using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 8f;
    public float lifetime = 3f;

    private Transform target;
    private int damage;

    public void Init(Transform target, int damage)
    {
        this.target = target;
        this.damage = damage;
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, target.position) < 0.1f)
            HitTarget();
    }

    void HitTarget()
    {
        EnemyBase enemy = target.GetComponent<EnemyBase>();
        enemy?.TakeDamage(damage);
        Destroy(gameObject);
    }
}
