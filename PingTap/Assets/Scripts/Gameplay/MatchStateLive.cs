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
      GameManager.Instance.SetState(GameState.Live);
      GameManager.Instance.enemyManager.StartSpawner();
    }

    public void Tick()
    {
      GameManager.Instance.waveTimer += Time.deltaTime;

      //if (spawnedTreasure || !(GameManager.Instance.waveTimer >= nextUpdate)) return;

      //nextUpdate = Mathf.FloorToInt(Time.time) + 1;
      //TreasureSpawn();
    }

    public void OnExit()
    {
      GameManager.Instance.waveTimer = 0;

      spawnedTreasure = false;
    }

    void TreasureSpawn()
    {
      //spawnedTreasure = GameManager.Instance.treasureSpawner.Spawn();
    }
  }
}