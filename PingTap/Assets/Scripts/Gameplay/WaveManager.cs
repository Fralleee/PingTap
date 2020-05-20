using Fralle.AI;
using Fralle.AI.Spawning;
using Fralle.Core.Attributes;
using System;
using UnityEngine;

namespace Fralle.Gameplay
{
  public class WaveManager : MonoBehaviour
  {
    public static event Action<WaveManager> OnNewSchema = delegate { };
    public static event Action<WaveManager> OnNewWave = delegate { };
    public static event Action<WaveManager> OnWavesComplete = delegate { };
    public static event Action<float> OnWaveProgress = delegate { };

    [Header("Armies")] public Army[] armies;
    [SerializeField] Spawner spawner;
    [SerializeField] GameObject blockerPrefab;

    public bool WavesRemaining => currentArmy < armies.Length - 1 || currentWave < maxWaves;
    public Army GetCurrentArmy => armies[currentArmy];
    public WaveDefinition GetCurrentWave => armies[currentArmy].NextWave(currentWave);

    [Header("Current Stats")] [Readonly] public int maxArmies;
    [Readonly] public int maxWaves;
    [Readonly] public int currentArmy;
    [Readonly] public int currentWave;
    [Readonly] public int waveProgress;

    GameObject blocker;
    int currentWaveCount;

    void Awake()
    {
      blocker = Instantiate(blockerPrefab, spawner.transform.position, Quaternion.identity, transform);
    }

    void Start()
    {
      SetNextSchema();
      maxArmies = armies.Length - 1;

      Enemy.OnAnyEnemyDeath += HandleEnemyDeath;
    }

    public int NextWave()
    {
      currentWave++;
      if (currentWave <= maxWaves) return SpawnWave();

      if (currentArmy < armies.Length - 1)
      {
        currentArmy++;
        SetNextSchema();
        currentWave = 1;
        return SpawnWave();
      }

      OnWavesComplete(this);
      return 0;
    }

    public void ToggleBlocker(bool active)
    {
      if (blocker != null) blocker.SetActive(active);
    }

    void SetNextSchema()
    {
      var army = armies[currentArmy];
      maxWaves = army.MaxRounds;
      spawner.army = army;
      OnNewSchema(this);
    }

    int SpawnWave()
    {
      OnNewWave(this);
      currentWaveCount = spawner.SpawnRound(currentWave);
      return currentWaveCount;
    }

    void UpdateWaveProgress()
    {
      currentWaveCount--;
      OnWaveProgress(1 - (currentWaveCount / (float)GetCurrentWave.count));
    }

    void HandleEnemyDeath(Enemy enemy)
    {
      UpdateWaveProgress();
    }
  }
}