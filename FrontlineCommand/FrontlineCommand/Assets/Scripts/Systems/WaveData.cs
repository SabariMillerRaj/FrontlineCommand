using UnityEngine;

public enum EnemyType { Infantry, Fast }

[CreateAssetMenu(fileName = "WaveData", menuName = "FrontlineCommand/WaveData")]
public class WaveData : ScriptableObject
{
    public int waveNumber;
    public int enemyCount = 5;
    public EnemyType enemyType = EnemyType.Infantry;
    public float spawnInterval = 2f;
    public int resourceReward = 50;
    public float placementTimeSecs = 20f;
}
