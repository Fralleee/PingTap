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

  [Readonly] public int maxRounds;
  [Readonly] public int schemaRound;
  [Readonly] public int currentArmy;

  public Army[] armies;
  [SerializeField] Spawner spawner;

  public bool WavesRemaining => currentArmy < armies.Length - 1 || schemaRound < maxRounds;

  public Army GetCurrentArmy => armies[currentArmy];

  void Start()
  {
    SetNextSchema();
  }

  void SetNextSchema()
  {
    Army army = armies[currentArmy];
    maxRounds = army.MaxRounds;
    spawner.army = army;
    OnNewSchema(this);
  }

  int SpawnWave()
  {
    OnNewWave(this);
    return spawner.SpawnRound(schemaRound);
  }


  public int NextWave()
  {
    schemaRound++;
    if (schemaRound <= maxRounds)
    {
      return SpawnWave();
    }
    else if (currentArmy < armies.Length - 1)
    {
      currentArmy++;
      SetNextSchema();
      schemaRound = 1;
      return SpawnWave();
    }

    OnWavesComplete(this);
    return 0;
  }

}
