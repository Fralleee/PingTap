using UnityEngine;

namespace Fralle.Gameplay
{
  public class MatchStatePrepare : IState
  {
    public void OnEnter()
    {
      MatchManager.Instance.prepareTimer = MatchManager.Instance.prepareTime;
      MatchManager.Instance.NewState(GameState.Prepare);
    }

    public void Tick()
    {
      MatchManager.Instance.prepareTimer -= Time.deltaTime;
    }

    public void OnExit()
    {
    }
  }
}