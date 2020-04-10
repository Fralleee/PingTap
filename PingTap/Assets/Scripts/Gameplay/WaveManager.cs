using System;
using System.Collections;
using System.Collections.Generic;
using Fralle;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
  public static event Action<WaveManager> OnNewSchema = delegate { };
  public static event Action<WaveManager> OnNewWave = delegate { };
  public static event Action<WaveManager> OnWavesComplete = delegate { };
  public static event Action<float> OnWaveProgress = delegate { };

  [Readonly] public int maxArmies;
  [Readonly] public int maxWaves;
  [Readonly] public int currentArmy;
  [Readonly] public int currentWave;
  [Readonly] public int waveProgress;

  public Army[] armies;
  [SerializeField] Spawner spawner;

  public bool WavesRemaining => currentArmy < armies.Length - 1 || currentWave < maxWaves;
  public Army GetCurrentArmy => armies[currentArmy];

  int currentWaveCount;

  void Start()
  {
    SetNextSchema();
    maxArmies = armies.Length - 1;

    Enemy.OnAnyEnemyDeath += HandleEnemyDeath;
  }

  void SetNextSchema()
  {
    Army army = armies[currentArmy];
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

  public int NextWave()
  {
    currentWave++;
    if (currentWave <= maxWaves)
    {
      return SpawnWave();
    }
    else if (currentArmy < armies.Length - 1)
    {
      currentArmy++;
      SetNextSchema();
      currentWave = 1;
      return SpawnWave();
    }

    OnWavesComplete(this);
    return 0;
  }

  void UpdateWaveProgress(int change)
  {
    currentWaveCount--;
    int waveIndex = Mathf.Clamp(currentWave, 1, maxWaves - 1);
    float total = armies[currentArmy].waveDefinitions[waveIndex].count;
    OnWaveProgress(1 - currentWaveCount / total);
  }

  void HandleEnemyDeath(Enemy enemy)
  {
    UpdateWaveProgress(-1);
  }
}
