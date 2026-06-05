using UnityEngine;

// Long range, high damage, slow fire rate
public class Sniper : UnitBase
{
    public int cost = 80;

    void Awake()
    {
        attackRange = 6f;
        damage = 25;
        fireRate = 0.4f;
    }
}
