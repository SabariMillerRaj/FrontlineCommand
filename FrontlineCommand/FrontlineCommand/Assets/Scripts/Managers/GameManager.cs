using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game State")]
    public int currentWave = 0;
    public int score = 0;
    public int outpostHP = 100;
    public int maxOutpostHP = 100;
    public int resources = 150;
    public bool isPlacementPhase = true;
    public bool isGameOver = false;

    [Header("Wave Settings")]
    public float placementPhaseDuration = 20f;
    public WaveData[] waves;

    private float placementTimer;

    public static event System.Action OnPlacementPhaseStart;
    public static event System.Action OnWaveStart;
    public static event System.Action<int> OnResourceChanged;
    public static event System.Action<int> OnScoreChanged;
    public static event System.Action<int, int> OnOutpostHPChanged;
    public static event System.Action OnGameOver;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        StartPlacementPhase();
    }

    void Update()
    {
        if (isGameOver) return;

        if (isPlacementPhase)
        {
            placementTimer -= Time.deltaTime;
            if (placementTimer <= 0f)
                StartWave();
        }
    }

    public void StartPlacementPhase()
    {
        isPlacementPhase = true;
        placementTimer = placementPhaseDuration;
        int reward = currentWave == 0 ? 0 : GetCurrentWaveData().resourceReward;
        if (reward > 0) AddResources(reward);
        OnPlacementPhaseStart?.Invoke();
    }

    public void StartWave()
    {
        if (currentWave >= waves.Length)
        {
            // Loop back harder after all defined waves
            currentWave = waves.Length - 1;
        }
        isPlacementPhase = false;
        currentWave++;
        SpawnManager.Instance?.BeginWave(GetCurrentWaveData());
        OnWaveStart?.Invoke();
    }

    public void OnWaveComplete()
    {
        AddScore(currentWave * 100);
        StartPlacementPhase();
    }

    public void DamageOutpost(int dmg)
    {
        outpostHP = Mathf.Max(0, outpostHP - dmg);
        OnOutpostHPChanged?.Invoke(outpostHP, maxOutpostHP);
        if (outpostHP <= 0) TriggerGameOver();
    }

    public void TriggerGameOver()
    {
        if (isGameOver) return;
        isGameOver = true;
        PlayerPrefs.SetInt("FinalScore", score);
        PlayerPrefs.SetInt("WavesSurvived", currentWave);
        PlayerPrefs.Save();
        OnGameOver?.Invoke();
        Invoke(nameof(LoadGameOver), 2f);
    }

    void LoadGameOver() => SceneManager.LoadScene("GameOver");

    public bool SpendResources(int amount)
    {
        if (resources < amount) return false;
        resources -= amount;
        OnResourceChanged?.Invoke(resources);
        return true;
    }

    public void AddResources(int amount)
    {
        resources += amount;
        OnResourceChanged?.Invoke(resources);
    }

    public void AddScore(int amount)
    {
        score += amount;
        OnScoreChanged?.Invoke(score);
    }

    public float GetPlacementTimeNormalized() =>
        isPlacementPhase ? placementTimer / placementPhaseDuration : 0f;

    public WaveData GetCurrentWaveData() =>
        waves[Mathf.Clamp(currentWave, 0, waves.Length - 1)];
}
