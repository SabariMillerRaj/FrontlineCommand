using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDController : MonoBehaviour
{
    [Header("Labels")]
    public TMP_Text waveLabel;
    public TMP_Text scoreLabel;
    public TMP_Text resourceLabel;
    public TMP_Text phaseLabel;
    public TMP_Text timerLabel;

    [Header("Outpost HP Bar")]
    public Slider outpostHPSlider;
    public Image outpostHPFill;
    public Color fullHPColor = Color.green;
    public Color lowHPColor = Color.red;

    [Header("Unit Buttons")]
    public Button soldierButton;
    public Button sniperButton;
    public Button barrierButton;
    public Button scoutButton;

    [Header("Unit Prefabs (assign in Inspector)")]
    public GameObject soldierPrefab;
    public GameObject sniperPrefab;
    public GameObject barrierPrefab;

    [Header("Unit Costs")]
    public int soldierCost = 50;
    public int sniperCost = 80;
    public int barrierCost = 30;

    [Header("Game Over Panel")]
    public GameObject gameOverPanel;

    void Start()
    {
        GameManager.OnResourceChanged += UpdateResources;
        GameManager.OnScoreChanged += UpdateScore;
        GameManager.OnOutpostHPChanged += UpdateOutpostHP;
        GameManager.OnPlacementPhaseStart += OnPlacementPhase;
        GameManager.OnWaveStart += OnWavePhase;
        GameManager.OnGameOver += ShowGameOver;

        if (gameOverPanel != null) gameOverPanel.SetActive(false);

        // Wire up buttons
        soldierButton?.onClick.AddListener(() => UnitPlacer.Instance?.SelectUnit(soldierPrefab, soldierCost));
        sniperButton?.onClick.AddListener(() => UnitPlacer.Instance?.SelectUnit(sniperPrefab, sniperCost));
        barrierButton?.onClick.AddListener(() => UnitPlacer.Instance?.SelectUnit(barrierPrefab, barrierCost));
        scoutButton?.onClick.AddListener(OnScoutClicked);

        // Initial state
        UpdateResources(GameManager.Instance != null ? GameManager.Instance.resources : 0);
        UpdateScore(0);
    }

    void OnDestroy()
    {
        GameManager.OnResourceChanged -= UpdateResources;
        GameManager.OnScoreChanged -= UpdateScore;
        GameManager.OnOutpostHPChanged -= UpdateOutpostHP;
        GameManager.OnPlacementPhaseStart -= OnPlacementPhase;
        GameManager.OnWaveStart -= OnWavePhase;
        GameManager.OnGameOver -= ShowGameOver;
    }

    void Update()
    {
        if (GameManager.Instance == null) return;

        // Update placement timer
        if (GameManager.Instance.isPlacementPhase && timerLabel != null)
        {
            float t = GameManager.Instance.GetPlacementTimeNormalized();
            int secs = Mathf.CeilToInt(t * GameManager.Instance.placementPhaseDuration);
            timerLabel.text = $"Deploy: {secs}s";
            timerLabel.color = secs <= 5 ? Color.red : Color.white;
        }

        // Wave label
        if (waveLabel != null)
            waveLabel.text = $"Wave {GameManager.Instance.currentWave}";
    }

    void UpdateResources(int amount)
    {
        if (resourceLabel != null) resourceLabel.text = $"⚡ {amount}";

        // Dim buttons if can't afford
        SetButtonAffordable(soldierButton, amount >= soldierCost);
        SetButtonAffordable(sniperButton, amount >= sniperCost);
        SetButtonAffordable(barrierButton, amount >= barrierCost);
    }

    void SetButtonAffordable(Button btn, bool affordable)
    {
        if (btn == null) return;
        var colors = btn.colors;
        colors.normalColor = affordable ? Color.white : new Color(0.5f, 0.5f, 0.5f);
        btn.colors = colors;
        btn.interactable = affordable;
    }

    void UpdateScore(int s)
    {
        if (scoreLabel != null) scoreLabel.text = $"Score: {s}";
    }

    void UpdateOutpostHP(int current, int max)
    {
        if (outpostHPSlider != null)
        {
            outpostHPSlider.maxValue = max;
            outpostHPSlider.value = current;
        }
        if (outpostHPFill != null)
            outpostHPFill.color = Color.Lerp(lowHPColor, fullHPColor, (float)current / max);
    }

    void OnPlacementPhase()
    {
        if (phaseLabel != null) phaseLabel.text = "— DEPLOY PHASE —";
        SetUnitButtonsActive(true);
        if (timerLabel != null) timerLabel.gameObject.SetActive(true);
    }

    void OnWavePhase()
    {
        if (phaseLabel != null) phaseLabel.text = "— WAVE INCOMING —";
        SetUnitButtonsActive(false);
        UnitPlacer.Instance?.CancelPlacement();
        if (timerLabel != null) timerLabel.gameObject.SetActive(false);
    }

    void SetUnitButtonsActive(bool active)
    {
        soldierButton?.gameObject.SetActive(active);
        sniperButton?.gameObject.SetActive(active);
        barrierButton?.gameObject.SetActive(active);
        scoutButton?.gameObject.SetActive(active);
    }

    void OnScoutClicked()
    {
        // Deploy scout at center of map (can be improved to click-to-place)
        FogOfWarController.Instance?.DeployScout(Vector2.zero);
    }

    void ShowGameOver()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
    }
}
