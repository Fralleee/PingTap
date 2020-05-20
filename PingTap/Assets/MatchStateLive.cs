using Fralle;
using Fralle.Gameplay;
using UnityEngine;

public class MatchStateLive : IState
{
  readonly WaveManager waveManager;
  readonly MatchManager matchManager;

  public MatchStateLive(WaveManager waveManager, MatchManager matchManager)
  {
    this.waveManager = waveManager;
    this.matchManager = matchManager;
  }

  public void OnEnter()
  {
    int enemyCount = waveManager.NextWave();
    matchManager.NewWave(enemyCount);

    matchManager.NewState(GameState.Live);
  }

  public void Tick()
  {
    matchManager.waveTimer += Time.deltaTime;
  }

  public void OnExit()
  {
    matchManager.waveTimer = 0;
  }
}