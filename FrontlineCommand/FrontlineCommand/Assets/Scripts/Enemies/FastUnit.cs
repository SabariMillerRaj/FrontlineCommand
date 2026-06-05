using UnityEngine;

// Fast enemy — low HP, moves quickly, less outpost damage
public class FastUnit : EnemyAI
{
    protected override void Awake()
    {
        base.Awake();
        maxHP = 15;
        moveSpeed = 4f;
        outpostDamage = 5;
        resourceDrop = 8; // Harder to hit = more reward
    }
}
