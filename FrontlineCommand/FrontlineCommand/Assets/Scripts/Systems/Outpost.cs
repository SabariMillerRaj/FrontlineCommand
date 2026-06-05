using UnityEngine;

// Place this on your Outpost GameObject. Tag the GameObject "Outpost".
public class Outpost : MonoBehaviour
{
    // HP is managed by GameManager. This script handles visual feedback only.
    public SpriteRenderer spriteRenderer;
    public Color damagedColor = new Color(1f, 0.4f, 0.1f);
    private Color originalColor;

    void Start()
    {
        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;

        GameManager.OnOutpostHPChanged += OnHPChanged;
    }

    void OnDestroy()
    {
        GameManager.OnOutpostHPChanged -= OnHPChanged;
    }

    void OnHPChanged(int current, int max)
    {
        if (spriteRenderer == null) return;
        float ratio = (float)current / max;
        spriteRenderer.color = Color.Lerp(damagedColor, originalColor, ratio);
        StartCoroutine(FlashDamage());
    }

    System.Collections.IEnumerator FlashDamage()
    {
        if (spriteRenderer == null) yield break;
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.Lerp(damagedColor, Color.white,
            (float)(GameManager.Instance?.outpostHP ?? 100) / (GameManager.Instance?.maxOutpostHP ?? 100));
    }
}
