using UnityEngine;

// Barrier: no shooting, just blocks enemy path and absorbs damage
[RequireComponent(typeof(Collider2D))]
public class Barrier : MonoBehaviour
{
    public int cost = 30;
    public int maxHP = 80;
    private int currentHP;

    public SpriteRenderer spriteRenderer;

    void Start()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(int dmg)
    {
        currentHP -= dmg;
        float ratio = (float)currentHP / maxHP;

        // Darken sprite as HP drops
        if (spriteRenderer != null)
            spriteRenderer.color = Color.Lerp(Color.red, Color.white, ratio);

        if (currentHP <= 0)
            Destroy(gameObject);
    }
}
