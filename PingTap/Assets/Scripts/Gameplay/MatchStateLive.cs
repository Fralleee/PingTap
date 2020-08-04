using Fralle.Core.Interfaces;
using UnityEngine;

namespace Fralle.Gameplay
{
  public class MatchStateLive : IState
  {
    int nextUpdate = 1;
    bool spawnedTreasure;

    public void OnEnter()
    {
      //MatchManager.Instance.SpawnWave();
      //MatchManager.Instance.NewState(GameState.Live);
    }

    public void Tick()
    {
      MatchManager.Instance.waveTimer += Time.deltaTime;

      if (spawnedTreasure || !(MatchManager.Instance.waveTimer >= nextUpdate)) return;

      nextUpdate = Mathf.FloorToInt(Time.time) + 1;
      TreasureSpawn();
    }

    public void OnExit()
    {
      MatchManager.Instance.waveTimer = 0;

      spawnedTreasure = false;
    }

    void TreasureSpawn()
    {
      spawnedTreasure = MatchManager.Instance.treasureSpawner.Spawn();
    }
  }
}