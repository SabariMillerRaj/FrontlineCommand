using UnityEngine;

// Attach to an empty GameObject in the scene.
// Call SelectUnit() from UI buttons, then click map to place.
public class UnitPlacer : MonoBehaviour
{
    public static UnitPlacer Instance { get; private set; }

    [Header("Placement Settings")]
    public LayerMask groundLayer;       // Layer your tilemap/ground is on
    public LayerMask blockedLayer;      // Layer for obstacles/outpost
    public Color validColor = new Color(0f, 1f, 0f, 0.5f);
    public Color invalidColor = new Color(1f, 0f, 0f, 0.5f);

    private GameObject selectedPrefab;
    private int selectedCost;
    private GameObject ghostObject;
    private SpriteRenderer ghostRenderer;
    private bool canPlace = false;
    private Camera mainCam;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        mainCam = Camera.main;
    }

    void Update()
    {
        if (GameManager.Instance == null || !GameManager.Instance.isPlacementPhase)
        {
            CancelPlacement();
            return;
        }

        if (selectedPrefab == null) return;

        UpdateGhost();

        if (Input.GetMouseButtonDown(0) && canPlace)
            PlaceUnit();

        if (Input.GetMouseButtonDown(1))
            CancelPlacement();
    }

    public void SelectUnit(GameObject prefab, int cost)
    {
        if (GameManager.Instance == null || !GameManager.Instance.isPlacementPhase) return;
        CancelPlacement();
        selectedPrefab = prefab;
        selectedCost = cost;

        // Create a transparent ghost preview
        ghostObject = Instantiate(prefab);
        ghostRenderer = ghostObject.GetComponentInChildren<SpriteRenderer>();
        DisableScripts(ghostObject);

        if (ghostRenderer != null)
            ghostRenderer.color = validColor;
    }

    void UpdateGhost()
    {
        Vector3 mouseWorld = mainCam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0;

        // Snap to 1-unit grid
        mouseWorld.x = Mathf.Round(mouseWorld.x);
        mouseWorld.y = Mathf.Round(mouseWorld.y);

        if (ghostObject != null)
            ghostObject.transform.position = mouseWorld;

        // Check if position is valid
        Collider2D hit = Physics2D.OverlapPoint(mouseWorld, blockedLayer);
        bool affordable = GameManager.Instance.resources >= selectedCost;
        canPlace = hit == null && affordable;

        if (ghostRenderer != null)
            ghostRenderer.color = canPlace ? validColor : invalidColor;
    }

    void PlaceUnit()
    {
        if (!GameManager.Instance.SpendResources(selectedCost)) return;

        Vector3 pos = ghostObject.transform.position;
        Instantiate(selectedPrefab, pos, Quaternion.identity);
        AudioManager.Instance?.PlayPlacement();

        // Allow continued placement of same unit
        SelectUnit(selectedPrefab, selectedCost);
    }

    public void CancelPlacement()
    {
        if (ghostObject != null) Destroy(ghostObject);
        selectedPrefab = null;
        ghostObject = null;
    }

    void DisableScripts(GameObject obj)
    {
        foreach (var mono in obj.GetComponentsInChildren<MonoBehaviour>())
        {
            if (mono is not SpriteRenderer && mono is not Transform)
                mono.enabled = false;
        }
        foreach (var col in obj.GetComponentsInChildren<Collider2D>())
            col.enabled = false;
    }
}
