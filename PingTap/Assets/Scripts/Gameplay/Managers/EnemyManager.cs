using Fralle.AI;
using Fralle.Core.Attributes;
using Fralle.Core.Extensions;
using Fralle.Core.Infrastructure;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fralle.Gameplay
{
  public class EnemyManager : Singleton<GameManager>
  {
    public static event Action OnMatchEnd = delegate { };
    public static event Action OnDefeat = delegate { };
    public static event Action OnVictory = delegate { };
    public static event Action OnNewWave = delegate { };
    public static event Action<GameState> OnSetState = delegate { };

    public List<SpawnWave> waves;

    [Space(10)]
    [Readonly] public int enemiesSpawned;
    [Readonly] public int enemiesKilled;
    [Readonly] public int totalEnemies;

    [HideInInspector] public Spawner spawner;

    bool spawnComplete = false;
    public bool AllEnemiesDead => spawnComplete && enemiesSpawned - enemiesKilled == 0;

    protected override void Awake()
    {
      base.Awake();

      spawner = GetComponent<Spawner>();
      spawner.OnSpawnComplete += HandleSpawnComplete;
      Enemy.OnAnyEnemyDeath += HandleEnemyDeath;
    }

    public void PrepareSpawner()
    {
      var wave = waves.PopAt(0);
      spawner.SetSpawnDefinition(wave);
    }

    public void StartSpawner()
    {
      spawner.StartSpawning();
      spawnComplete = false;
      enemiesSpawned = 0;
      enemiesKilled = 0;
    }

    void HandleSpawnComplete(int enemyCount)
    {
      enemiesSpawned = enemyCount;
      spawnComplete = true;
    }

    void HandleEnemyDeath(Enemy enemy)
    {
      enemiesKilled += 1;
    }
  }
}