using UnityEngine;

// Balanced unit — medium range, medium fire rate
public class Soldier : UnitBase
{
    public int cost = 50;

    void Awake()
    {
        attackRange = 3f;
        damage = 10;
        fireRate = 1f;
    }
}
