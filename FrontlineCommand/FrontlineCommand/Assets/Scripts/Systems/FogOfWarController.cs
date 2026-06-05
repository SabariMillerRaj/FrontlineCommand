using System.Collections;
using UnityEngine;

// Attach to a full-screen dark sprite that sits above the map.
// Units poke "holes" in visibility — simulated by tracking revealed zones.
// For a beginner-friendly approach: fog is a dark overlay,
// scout drone temporarily fades it out in a radius using a RenderTexture-free trick.
public class FogOfWarController : MonoBehaviour
{
    public static FogOfWarController Instance { get; private set; }

    [Header("Fog Settings")]
    public SpriteRenderer fogOverlay;
    [Range(0f, 1f)] public float fogAlpha = 0.85f;
    public float revealRadius = 3f;
    public float scoutRevealDuration = 5f;
    public int scoutCost = 40;

    private Color fogColor;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        if (fogOverlay != null)
        {
            fogColor = new Color(0.05f, 0.05f, 0.05f, fogAlpha);
            fogOverlay.color = fogColor;
        }
    }

    // Call from Scout Drone button in UI
    public void DeployScout(Vector2 worldPosition)
    {
        if (GameManager.Instance == null || !GameManager.Instance.SpendResources(scoutCost)) return;
        StartCoroutine(RevealArea(worldPosition));
    }

    IEnumerator RevealArea(Vector2 center)
    {
        float elapsed = 0f;
        float halfDuration = scoutRevealDuration / 2f;

        // Fade fog out in area (simplified: fade entire overlay for prototype)
        while (elapsed < halfDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / halfDuration;
            if (fogOverlay != null)
                fogOverlay.color = new Color(fogColor.r, fogColor.g, fogColor.b, Mathf.Lerp(fogAlpha, 0.1f, t));
            yield return null;
        }

        yield return new WaitForSeconds(scoutRevealDuration - halfDuration);

        // Fade fog back in
        elapsed = 0f;
        while (elapsed < halfDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / halfDuration;
            if (fogOverlay != null)
                fogOverlay.color = new Color(fogColor.r, fogColor.g, fogColor.b, Mathf.Lerp(0.1f, fogAlpha, t));
            yield return null;
        }

        if (fogOverlay != null)
            fogOverlay.color = fogColor;
    }
}
