using Fralle.Core.Interfaces;
using UnityEngine;

namespace Fralle.Gameplay
{
  public class MatchStatePrepare : IState
  {
    public void OnEnter()
    {
      GameManager.Instance.prepareTimer = GameManager.Instance.prepareTime;
      GameManager.Instance.SetState(GameState.Prepare);
      GameManager.Instance.enemyManager.PrepareSpawner();
    }

    public void Tick()
    {
      GameManager.Instance.prepareTimer -= Time.deltaTime;
    }

    public void OnExit()
    {
    }
  }
}