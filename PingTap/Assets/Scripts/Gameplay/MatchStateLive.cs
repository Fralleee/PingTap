using UnityEngine;

namespace Fralle.Gameplay
{
  public class MatchStateLive : IState
  {
    readonly WaveManager waveManager;
    readonly MatchManager matchManager;

    int nextUpdate = 1;
    bool spawnedTreasure;

    public MatchStateLive(WaveManager waveManager, MatchManager matchManager)
    {
      this.waveManager = waveManager;
      this.matchManager = matchManager;
    }

    public void OnEnter()
    {
      var enemyCount = waveManager.NextWave();
      matchManager.NewWave(enemyCount);
      matchManager.NewState(GameState.Live);
    }

    public void Tick()
    {
      matchManager.waveTimer += Time.deltaTime;

      if (spawnedTreasure || !(matchManager.waveTimer >= nextUpdate)) return;

      nextUpdate = Mathf.FloorToInt(Time.time) + 1;
      TreasureSpawn();
    }

    public void OnExit()
    {
      matchManager.waveTimer = 0;

      spawnedTreasure = false;
    }

    void TreasureSpawn()
    {
      spawnedTreasure = matchManager.treasureSpawner.Spawn();
    }
  }
}