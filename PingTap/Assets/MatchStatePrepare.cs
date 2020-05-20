using Fralle;
using Fralle.Gameplay;
using UnityEngine;

public class MatchStatePrepare : IState
{
  readonly WaveManager waveManager;
  readonly MatchManager matchManager;

  public MatchStatePrepare(WaveManager waveManager, MatchManager matchManager)
  {
    this.waveManager = waveManager;
    this.matchManager = matchManager;
  }

  public void OnEnter()
  {
    waveManager.ToggleBlocker(true);

    matchManager.prepareTimer = matchManager.prepareTime;
    matchManager.NewState(GameState.Prepare);
  }

  public void Tick()
  {
    matchManager.prepareTimer -= Time.deltaTime;
  }

  public void OnExit()
  {
    waveManager.ToggleBlocker(false);
  }
}