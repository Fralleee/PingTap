using Fralle.AI;
using Fralle.Core;
using System.Collections.Generic;
using UnityEngine;

namespace Fralle.Gameplay
{
  public class EnemyManager : MonoBehaviour
  {
    public List<SpawnWave> Waves;

    //bool spawnComplete;

    void Start()
    {
      if (Managers.Instance && Managers.Instance.Spawner)
        //Managers.Instance.Spawner.OnSpawnComplete += HandleSpawnComplete;
        ScoreController.OnAnyUnitDeath += HandleEnemyDeath;
    }

    public void PrepareSpawner()
    {
      SpawnWave wave = Waves.PopAt(0);
      Managers.Instance.Spawner.SetSpawnDefinition(wave);
    }

    public void StartSpawner()
    {
      Managers.Instance.Spawner.StartSpawning();
      //spawnComplete = false;
    }

    //void HandleSpawnComplete(int enemyCount)
    //{
    //	spawnComplete = true;
    //}

    static void HandleEnemyDeath(ScoreController enemy)
    {
      //if (spawnComplete && ScoreController.AliveCount == 0)
      //{
      EventManager.Broadcast(new GameOverEvent(true));
      //}
    }

    void OnDestroy()
    {
      //if (!Managers.Destroyed)
      //	Managers.Instance.Spawner.OnSpawnComplete -= HandleSpawnComplete;

      ScoreController.OnAnyUnitDeath -= HandleEnemyDeath;
    }
  }
}
