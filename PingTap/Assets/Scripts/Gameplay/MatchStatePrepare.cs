using UnityEngine;

namespace Fralle.Gameplay
{
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

      matchManager.prepareTimer = matchManager.prepareTime;
      matchManager.NewState(GameState.Prepare);
    }

    public void Tick()
    {
      matchManager.prepareTimer -= Time.deltaTime;
    }

    public void OnExit()
    {
    }
  }
}