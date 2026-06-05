using UnityEngine;

// Simple state-machine AI: Patrol edges → Move toward outpost → Attack on arrival
// No NavMesh needed — uses Vector2.MoveTowards for beginner-friendly movement
public class EnemyAI : EnemyBase
{
    public enum AIState { Moving, Attacking, Dead }
    public AIState currentState = AIState.Moving;

    [Header("Movement")]
    public float moveSpeed = 2f;
    public float stoppingDistance = 0.3f;

    [Header("Attack")]
    public float attackRange = 0.5f;
    public float attackCooldown = 1.5f;

    private Transform outpost;
    private float attackTimer = 0f;

    protected override void Awake()
    {
        base.Awake();
        // Find outpost by tag — make sure your Outpost GameObject has tag "Outpost"
        GameObject outpostObj = GameObject.FindWithTag("Outpost");
        if (outpostObj != null) outpost = outpostObj.transform;
    }

    void Update()
    {
        if (isDead || outpost == null || GameManager.Instance == null) return;
        if (GameManager.Instance.isGameOver) return;

        switch (currentState)
        {
            case AIState.Moving:   UpdateMoving();   break;
            case AIState.Attacking: UpdateAttacking(); break;
        }
    }

    void UpdateMoving()
    {
        float dist = Vector2.Distance(transform.position, outpost.position);

        if (dist <= attackRange)
        {
            currentState = AIState.Attacking;
            return;
        }

        // Move straight toward outpost — upgrade to A* later if desired
        Vector2 dir = ((Vector2)outpost.position - (Vector2)transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, outpost.position, moveSpeed * Time.deltaTime);

        // Flip sprite based on direction
        if (GetComponent<SpriteRenderer>() != null)
            GetComponent<SpriteRenderer>().flipX = dir.x < 0;
    }

    void UpdateAttacking()
    {
        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0f)
        {
            ReachOutpost();
            attackTimer = attackCooldown;
        }

        // If somehow pushed away, go back to moving
        if (outpost != null && Vector2.Distance(transform.position, outpost.position) > attackRange + 0.2f)
            currentState = AIState.Moving;
    }

    protected override void Die()
    {
        currentState = AIState.Dead;
        base.Die();
    }

    // Visualise attack range in editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
