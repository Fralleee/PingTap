using Fralle.AI;
using Fralle.AI.Spawning;
using Fralle.Core.Attributes;
using Fralle.Core.Infrastructure;
using System;
using UnityEngine;

namespace Fralle.Gameplay
{
  public class WaveManager : Singleton<WaveManager>
  {
    public static event Action OnNewSchema = delegate { };
    public static event Action OnNewWave = delegate { };
    public static event Action OnWavesComplete = delegate { };
    public static event Action<float> OnWaveProgress = delegate { };

    [Header("Armies")]
    public Army[] armies;
    [SerializeField] Spawner spawner = null;

    public bool WavesRemaining => currentArmy < armies.Length - 1 || currentWave < maxWaves;
    public Army GetCurrentArmy => armies[currentArmy];
    public WaveDefinition GetCurrentWave => armies[currentArmy].NextWave(currentWave);

    [Header("Current Stats")]
    [Readonly] public int maxArmies;
    [Readonly] public int maxWaves;
    [Readonly] public int currentArmy;
    [Readonly] public int currentWave;
    [Readonly] public int waveProgress;

    int currentWaveCount;

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

      OnWavesComplete();
      return 0;
    }

    void SetNextSchema()
    {
      var army = armies[currentArmy];
      maxWaves = army.MaxRounds;
      spawner.army = army;
      OnNewSchema();
    }

    int SpawnWave()
    {
      OnNewWave();
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