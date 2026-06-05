using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance { get; private set; }

    [Header("Spawn Points")]
    public Transform[] spawnPoints; // Place these at map edges in the editor

    [Header("Prefabs")]
    public GameObject infantryPrefab;
    public GameObject fastUnitPrefab;

    private int enemiesRemaining = 0;
    private bool waveActive = false;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void BeginWave(WaveData data)
    {
        waveActive = true;
        StartCoroutine(SpawnRoutine(data));
    }

    IEnumerator SpawnRoutine(WaveData data)
    {
        enemiesRemaining = data.enemyCount;

        for (int i = 0; i < data.enemyCount; i++)
        {
            SpawnEnemy(data.enemyType);
            yield return new WaitForSeconds(data.spawnInterval);
        }
    }

    void SpawnEnemy(EnemyType type)
    {
        if (spawnPoints == null || spawnPoints.Length == 0) return;

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject prefab = type == EnemyType.Fast ? fastUnitPrefab : infantryPrefab;

        if (prefab != null)
            Instantiate(prefab, spawnPoint.position, Quaternion.identity);
    }

    public void OnEnemyKilled()
    {
        enemiesRemaining = Mathf.Max(0, enemiesRemaining - 1);
        GameManager.Instance?.AddScore(10);

        if (enemiesRemaining <= 0 && waveActive)
        {
            waveActive = false;
            // Small delay so last enemy death animation can play
            Invoke(nameof(NotifyWaveComplete), 1.5f);
        }
    }

    void NotifyWaveComplete() => GameManager.Instance?.OnWaveComplete();
}
